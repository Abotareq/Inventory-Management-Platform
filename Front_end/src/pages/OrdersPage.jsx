import { useMemo } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Button from '../components/common/Button';
import Pagination from '../components/common/Pagination';
import PageHeader from '../components/layout/PageHeader';
import OrderFilters from '../components/orders/OrderFilters';
import OrderList from '../components/orders/OrderList';
import { DEFAULT_PAGE_SIZE } from '../config/constants';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import * as orderService from '../services/orderService';
import { isSalesAgent } from '../utils/roleHelpers';

const FILTER_KEYS = ['status', 'fromDate', 'toDate', 'customerId'];

// Filters and page live in the query string so the list survives a refresh
// and "back" from an order detail returns to the same view.
export default function OrdersPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [params, setParams] = useSearchParams();
  const canCreate = isSalesAgent(user);

  const filters = useMemo(
    () => Object.fromEntries(FILTER_KEYS.map((k) => [k, params.get(k) ?? ''])),
    [params],
  );
  const pageNumber = Math.max(1, Number(params.get('page')) || 1);
  const pageSize = DEFAULT_PAGE_SIZE;

  const orders = useFetch(
    (signal) =>
      orderService.getOrders(
        {
          pageNumber,
          pageSize,
          status: filters.status || undefined,
          fromDate: filters.fromDate || undefined,
          toDate: filters.toDate ? `${filters.toDate}T23:59:59` : undefined,
          customerId: filters.customerId.trim() || undefined,
        },
        { signal },
      ),
    [pageNumber, pageSize, filters.status, filters.fromDate, filters.toDate, filters.customerId],
  );

  function applyFilters(next) {
    const clean = {};
    FILTER_KEYS.forEach((k) => {
      if (next[k]) clean[k] = next[k];
    });
    setParams(clean); // page resets to 1
  }

  function setPage(n) {
    const next = Object.fromEntries(params.entries());
    if (n > 1) next.page = String(n);
    else delete next.page;
    setParams(next);
  }

  const filtered = FILTER_KEYS.some((k) => filters[k]);
  const totalCount = orders.data?.totalCount ?? 0;

  return (
    <>
      <PageHeader
        title="Orders"
        meta={
          !orders.loading &&
          !orders.error && (
            <span>
              <span className="num">{totalCount}</span> {totalCount === 1 ? 'order' : 'orders'}
              {filtered ? ' matching' : ''}
            </span>
          )
        }
        actions={
          canCreate && (
            <Button variant="primary" to="/orders/new">
              Create order
            </Button>
          )
        }
      />

      <OrderFilters values={filters} onChange={applyFilters} onClear={() => setParams({})} />

      <OrderList
        orders={orders.data?.items}
        loading={orders.loading}
        error={orders.error}
        onRetry={orders.refetch}
        filtered={filtered}
        canCreate={canCreate}
        onCreate={() => navigate('/orders/new')}
        onOpen={(order) => navigate(`/orders/${order.orderId}`)}
      />

      <Pagination
        pageNumber={pageNumber}
        pageSize={pageSize}
        totalCount={totalCount}
        onPageChange={setPage}
      />
    </>
  );
}
