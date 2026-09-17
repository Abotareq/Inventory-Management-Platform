import apiClient from './apiClient';

export async function getCategories(config) {
  const { data } = await apiClient.get('/categories', config);
  return data;
}

export async function getCategory(id, config) {
  const { data } = await apiClient.get(`/categories/${id}`, config);
  return data;
}

export async function createCategory({ name }) {
  const { data } = await apiClient.post('/categories', { name });
  return data;
}

export async function updateCategory(id, { name }) {
  const { data } = await apiClient.put(`/categories/${id}`, { name });
  return data;
}

export async function deleteCategory(id) {
  await apiClient.delete(`/categories/${id}`);
}
