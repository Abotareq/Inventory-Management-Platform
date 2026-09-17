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

// Walks every page so selects can list all products. The API caps pageSize at 100.
export async function getAllProducts(config, { maxPages = 20 } = {}) {
  const pageSize = 100;
  const all = [];
  for (let pageNumber = 1; pageNumber <= maxPages; pageNumber += 1) {
    const page = await getProducts({ pageNumber, pageSize }, config);
    all.push(...(page.items ?? []));
    if (all.length >= (page.totalCount ?? 0) || (page.items ?? []).length < pageSize) break;
  }
  return all;
}
