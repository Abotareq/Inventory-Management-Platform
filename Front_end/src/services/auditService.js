import apiClient from './apiClient';
import { DEFAULT_PAGE_SIZE } from '../config/constants';

export async function getAuditLogs(
  { pageNumber = 1, pageSize = DEFAULT_PAGE_SIZE, entityName, entityId } = {},
  config,
) {
  const params = { pageNumber, pageSize };
  if (entityName) params.entityName = entityName;
  if (entityId) params.entityId = entityId;
  const { data } = await apiClient.get('/audit-logs', { ...config, params });
  return data;
}
