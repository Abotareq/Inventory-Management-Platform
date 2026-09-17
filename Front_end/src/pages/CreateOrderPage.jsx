import { useNavigate } from 'react-router-dom';
import Button from '../components/common/Button';
import ErrorMessage from '../components/common/ErrorMessage';
import LoadingSpinner from '../components/common/LoadingSpinner';
import PageHeader from '../components/layout/PageHeader';
import CreateOrderForm from '../components/orders/CreateOrderForm';
import { useLookups } from '../hooks/useLookups';
import { useToast } from '../hooks/useToast';
import * as orderService from '../services/orderService';

export default function CreateOrderPage() {
  const navigate = useNavigate();
  const toast = useToast();
  const lookups = useLookups();

  async function handleSubmit(payload) {
    const order = await orderService.createOrder(payload);
    toast.success('Order created as a draft. Submit it when it’s ready.');
    navigate(`/orders/${order.orderId}`, { replace: true });
  }

  return (
    <>
      <PageHeader
        title="Create order"
        meta={<span>New orders start as drafts. You can submit them from the order page.</span>}
        actions={
          <Button variant="ghost" to="/orders">
            Back to orders
          </Button>
        }
      />

      {lookups.error ? (
        <ErrorMessage
          title="Couldn't load products and warehouses"
          message={lookups.error}
          onRetry={lookups.refetch}
        />
      ) : lookups.loading ? (
        <LoadingSpinner label="Loading products and warehouses" />
      ) : (
        <CreateOrderForm
          products={lookups.products}
          warehouses={lookups.warehouses}
          onSubmit={handleSubmit}
          onCancel={() => navigate('/orders')}
        />
      )}
    </>
  );
}
