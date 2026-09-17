import apiClient from './apiClient';

export async function login({ email, password }) {
  const { data } = await apiClient.post('/auth/login', { email, password });
  return data;
}

export async function register({ fullName, email, password, role }) {
  const { data } = await apiClient.post('/auth/register', {
    fullName,
    email,
    password,
    role,
  });
  return data;
}
