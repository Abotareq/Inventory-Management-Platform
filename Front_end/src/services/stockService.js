import apiClient from './apiClient';
import { DEFAULT_PAGE_SIZE } from '../config/constants';

function paged({ pageNumber = 1, pageSize = DEFAULT_PAGE_SIZE } = {}) {
  return { pageNumber, pageSize };
}

export async function assignProductToWarehouse({ productId, warehouseId }) {
  const { data } = await apiClient.post('/stocks/assign', { productId, warehouseId });
  return data;
}

export async function adjustStock({ productId, warehouseId, amount, reason }) {
  const { data } = await apiClient.post('/stocks/adjust', {
    productId,
    warehouseId,
    amount: Number(amount),
    reason,
  });
  return data;
}

export async function deleteStock(stockId) {
  await apiClient.delete(`/stocks/${stockId}`);
}

export async function getStockByWarehouse(warehouseId, paging, config) {
  const { data } = await apiClient.get(`/stocks/warehouse/${warehouseId}`, {
    ...config,
    params: paged(paging),
  });
  return data;
}

export async function getStockByProduct(productId, paging, config) {
  const { data } = await apiClient.get(`/stocks/product/${productId}`, {
    ...config,
    params: paged(paging),
  });
  return data;
}

export async function getStockHistory(stockId, paging, config) {
  const { data } = await apiClient.get(`/stocks/${stockId}/history`, {
    ...config,
    params: paged(paging),
  });
  return data;
}

export async function getStockReservations(stockId, paging, config) {
  const { data } = await apiClient.get(`/stocks/${stockId}/reservations`, {
    ...config,
    params: paged(paging),
  });
  return data;
}
