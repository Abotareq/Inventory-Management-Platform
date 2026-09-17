import { useState } from 'react';
import { useParams } from 'react-router-dom';
import Button from '../components/common/Button';
import EmptyState from '../components/common/EmptyState';
import ErrorMessage from '../components/common/ErrorMessage';
import LoadingSpinner from '../components/common/LoadingSpinner';
import PageHeader from '../components/layout/PageHeader';
import OrderActionButtons from '../components/orders/OrderActionButtons';
import OrderDetail from '../components/orders/OrderDetail';
import OrderHistoryTimeline from '../components/orders/OrderHistoryTimeline';
import OrderStatusBadge from '../components/orders/OrderStatusBadge';
import { useFetch } from '../hooks/useFetch';
import { useLookups } from '../hooks/useLookups';
import { useToast } from '../hooks/useToast';
import { extractErrorMessage, isConflictError } from '../services/apiClient';
import * as orderService from '../services/orderService';
import { formatDateTime } from '../utils/formatDate';
import { shortId } from '../utils/formatId';
import styles from './OrderDetailPage.module.css';

// Toast text keeps the action's name: "Complete order" → "Order completed".
const ACTIONS = {
  submit: { run: orderService.submitOrder, done: 'Order submitted' },
  beginProcessing: { run: orderService.beginProcessingOrder, done: 'Order processing started' },
  complete: { run: orderService.completeOrder, done: 'Order completed' },
  cancel: { run: orderService.cancelOrder, done: 'Order cancelled' },
};

export default function OrderDetailPage() {
  const { id } = useParams();
  const toast = useToast();
  const lookups = useLookups();

  const order = useFetch((signal) => orderService.getOrder(id, { signal }), [id]);
  const history = useFetch(
    (signal) => orderService.getOrderHistory(id, { pageNumber: 1, pageSize: 100 }, { signal }),
    [id],
  );

  const [busy, setBusy] = useState(null);
  const [cancelError, setCancelError] = useState(null);

  async function handleAction(action) {
    const { run, done } = ACTIONS[action];
    setBusy(action);
    setCancelError(null);
    try {
      const updated = await run(id);
      if (updated?.orderId) order.setData(updated);
      else order.refetch();
      history.refetch();
      toast.success(done);
    } catch (err) {
      const message = extractErrorMessage(err);
      if (action === 'cancel') {
        setCancelError(message);
      } else {
        toast.error(message, { title: isConflictError(err) ? "Couldn't apply that change" : 'Error' });
        // A conflict usually means someone else moved the order; show the current state.
        if (isConflictError(err)) {
          order.refetch();
          history.refetch();
        }
      }
      throw err;
    } finally {
      setBusy(null);
    }
  }

  if (order.error) {
    const notFound = /doesn't exist/.test(order.error);
    return (
      <>
        <PageHeader title="Order" actions={<Button variant="ghost" to="/orders">Back to orders</Button>} />
        {notFound ? (
          <EmptyState
            title="Order not found"
            description="It may have been removed, or the link is wrong."
            action={<Button to="/orders">Go to orders</Button>}
          />
        ) : (
          <ErrorMessage title="Couldn't load this order" message={order.error} onRetry={order.refetch} />
        )}
      </>
    );
  }

  if (order.loading || !order.data) {
    return <LoadingSpinner label="Loading order" />;
  }

  const o = order.data;

  return (
    <>
      <PageHeader
        title={
          <span className={styles.title}>
            Order <span className="num">{shortId(o.orderId)}</span>
            <OrderStatusBadge status={o.status} />
          </span>
        }
        meta={
          <>
            <span>
              Customer <span className="num" title={o.customerId}>{shortId(o.customerId)}</span>
            </span>
            <span>
              Created <span className="num">{formatDateTime(o.createdAt)}</span>
            </span>
            <span className="num" title={o.orderId}>
              {o.orderId}
            </span>
          </>
        }
        actions={
          <>
            <OrderActionButtons
              order={o}
              onAction={handleAction}
              busy={busy}
              cancelError={cancelError}
              onCancelErrorClear={() => setCancelError(null)}
            />
            <Button variant="ghost" to="/orders">
              Back to orders
            </Button>
          </>
        }
      />

      <div className={styles.layout}>
        <section className={styles.items} aria-labelledby="items-heading">
          <h2 id="items-heading" className={styles.sectionTitle}>
            Items <span className={[styles.count, 'num'].join(' ')}>{o.items?.length ?? 0}</span>
          </h2>
          <OrderDetail
            order={o}
            productsById={lookups.productsById}
            warehousesById={lookups.warehousesById}
          />
        </section>

        <aside className={styles.history} aria-labelledby="history-heading">
          <h2 id="history-heading" className={styles.sectionTitle}>
            History
          </h2>
          <OrderHistoryTimeline
            entries={history.data?.items}
            loading={history.loading}
            error={history.error}
            onRetry={history.refetch}
          />
        </aside>
      </div>
    </>
  );
}
