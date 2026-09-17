import apiClient from './apiClient';
import { DEFAULT_PAGE_SIZE } from '../config/constants';

export async function getProducts(
  { pageNumber = 1, pageSize = DEFAULT_PAGE_SIZE } = {},
  config,
) {
  const { data } = await apiClient.get('/products', {
    ...config,
    params: { pageNumber, pageSize },
  });
  return data;
}

export async function getProduct(id, config) {
  const { data } = await apiClient.get(`/products/${id}`, config);
  return data;
}

function toPayload({ name, sku, description, categoryId, price }) {
  return {
    name,
    sku,
    description: description ?? '',
    categoryId: categoryId || null,
    price: Number(price) || 0,
  };
}

export async function createProduct(values) {
  const { data } = await apiClient.post('/products', toPayload(values));
  return data;
}

export async function updateProduct(id, values) {
  const { data } = await apiClient.put(`/products/${id}`, toPayload(values));
  return data;
}

export async function deleteProduct(id) {
  await apiClient.delete(`/products/${id}`);
}
