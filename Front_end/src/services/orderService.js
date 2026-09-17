import apiClient from './apiClient';
import { DEFAULT_PAGE_SIZE } from '../config/constants';

// One idempotency key per submission attempt. The caller creates it once and
// reuses it if the same attempt is retried, so a retry never duplicates an order.
export function createIdempotencyKey() {
  return crypto.randomUUID();
}

export async function createOrder({ customerId, items, idempotencyKey }) {
  const { data } = await apiClient.post('/orders', {
    customerId,
    items: items.map((i) => ({
      productId: i.productId,
      warehouseId: i.warehouseId,
      quantity: Number(i.quantity),
    })),
    idempotencyKey: idempotencyKey ?? createIdempotencyKey(),
  });
  return data;
}

export async function submitOrder(id) {
  const { data } = await apiClient.post(`/orders/${id}/submit`);
  return data;
}

export async function beginProcessingOrder(id) {
  const { data } = await apiClient.post(`/orders/${id}/begin-processing`);
  return data;
}

export async function completeOrder(id) {
  const { data } = await apiClient.post(`/orders/${id}/complete`);
  return data;
}

export async function cancelOrder(id) {
  const { data } = await apiClient.post(`/orders/${id}/cancel`);
  return data;
}

export async function getOrder(id, config) {
  const { data } = await apiClient.get(`/orders/${id}`, config);
  return data;
}

export async function getOrders(
  {
    pageNumber = 1,
    pageSize = DEFAULT_PAGE_SIZE,
    customerId,
    status,
    fromDate,
    toDate,
  } = {},
  config,
) {
  const params = { pageNumber, pageSize };
  if (customerId) params.customerId = customerId;
  if (status) params.status = status;
  if (fromDate) params.fromDate = fromDate;
  if (toDate) params.toDate = toDate;
  const { data } = await apiClient.get('/orders', { ...config, params });
  return data;
}

export async function getOrderHistory(
  id,
  { pageNumber = 1, pageSize = DEFAULT_PAGE_SIZE } = {},
  config,
) {
  const { data } = await apiClient.get(`/orders/${id}/history`, {
    ...config,
    params: { pageNumber, pageSize },
  });
  return data;
}
