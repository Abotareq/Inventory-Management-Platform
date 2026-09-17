import apiClient from './apiClient';

export async function getWarehouses(config) {
  const { data } = await apiClient.get('/warehouses', config);
  return data;
}

export async function getWarehouse(id, config) {
  const { data } = await apiClient.get(`/warehouses/${id}`, config);
  return data;
}

export async function createWarehouse({ name, location }) {
  const { data } = await apiClient.post('/warehouses', { name, location });
  return data;
}

export async function updateWarehouse(id, { name, location }) {
  const { data } = await apiClient.put(`/warehouses/${id}`, { name, location });
  return data;
}

export async function deleteWarehouse(id) {
  await apiClient.delete(`/warehouses/${id}`);
}
