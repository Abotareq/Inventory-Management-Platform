import { useCallback, useMemo, useState } from 'react';
import { DEFAULT_PAGE_SIZE } from '../config/constants';

export function usePagination({ initialPage = 1, initialPageSize = DEFAULT_PAGE_SIZE } = {}) {
  const [pageNumber, setPageNumber] = useState(initialPage);
  const [pageSize, setPageSizeState] = useState(initialPageSize);

  const setPage = useCallback((n) => setPageNumber(Math.max(1, n)), []);
  const next = useCallback(() => setPageNumber((p) => p + 1), []);
  const prev = useCallback(() => setPageNumber((p) => Math.max(1, p - 1)), []);
  const reset = useCallback(() => setPageNumber(1), []);
  const setPageSize = useCallback((size) => {
    setPageSizeState(size);
    setPageNumber(1);
  }, []);

  return useMemo(
    () => ({ pageNumber, pageSize, setPage, next, prev, reset, setPageSize }),
    [pageNumber, pageSize, setPage, next, prev, reset, setPageSize],
  );
}
