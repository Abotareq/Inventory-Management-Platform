import { useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';
import AuditFilters from '../components/audit/AuditFilters';
import AuditLogList from '../components/audit/AuditLogList';
import Pagination from '../components/common/Pagination';
import PageHeader from '../components/layout/PageHeader';
import { DEFAULT_PAGE_SIZE } from '../config/constants';
import { useFetch } from '../hooks/useFetch';
import * as auditService from '../services/auditService';

export default function AuditLogsPage() {
  const [params, setParams] = useSearchParams();

  const filters = useMemo(
    () => ({ entityName: params.get('entityName') ?? '', entityId: params.get('entityId') ?? '' }),
    [params],
  );
  const pageNumber = Math.max(1, Number(params.get('page')) || 1);
  const pageSize = DEFAULT_PAGE_SIZE;

  const logs = useFetch(
    (signal) =>
      auditService.getAuditLogs(
        {
          pageNumber,
          pageSize,
          entityName: filters.entityName || undefined,
          entityId: filters.entityId.trim() || undefined,
        },
        { signal },
      ),
    [pageNumber, pageSize, filters.entityName, filters.entityId],
  );

  function applyFilters(next) {
    const clean = {};
    if (next.entityName) clean.entityName = next.entityName;
    if (next.entityId) clean.entityId = next.entityId;
    setParams(clean);
  }

  function setPage(n) {
    const next = Object.fromEntries(params.entries());
    if (n > 1) next.page = String(n);
    else delete next.page;
    setParams(next);
  }

  const filtered = Boolean(filters.entityName || filters.entityId);
  const totalCount = logs.data?.totalCount ?? 0;

  return (
    <>
      <PageHeader
        title="Audit logs"
        meta={
          !logs.loading &&
          !logs.error && (
            <span>
              <span className="num">{totalCount}</span> {totalCount === 1 ? 'entry' : 'entries'}
              {filtered ? ' matching' : ''}
            </span>
          )
        }
      />

      <AuditFilters values={filters} onChange={applyFilters} onClear={() => setParams({})} />

      <AuditLogList
        logs={logs.data?.items}
        loading={logs.loading}
        error={logs.error}
        onRetry={logs.refetch}
        filtered={filtered}
        onFilterEntity={(entityName, entityId) => applyFilters({ entityName, entityId })}
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
