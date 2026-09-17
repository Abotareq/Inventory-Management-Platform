import { useCallback, useEffect, useRef, useState } from 'react';
import { extractErrorMessage } from '../services/apiClient';

// Runs `fetcher(signal)` whenever `deps` change. Stale responses are ignored
// and in-flight requests are aborted on re-run/unmount.
//
//   const { data, loading, error, refetch } = useFetch(
//     (signal) => getProducts({ pageNumber }, { signal }),
//     [pageNumber],
//   );
export function useFetch(fetcher, deps = [], { enabled = true } = {}) {
  const [state, setState] = useState({ data: null, loading: enabled, error: null });
  const fetcherRef = useRef(fetcher);
  fetcherRef.current = fetcher;
  const [tick, setTick] = useState(0);

  useEffect(() => {
    if (!enabled) {
      setState({ data: null, loading: false, error: null });
      return undefined;
    }
    const controller = new AbortController();
    let active = true;

    setState((s) => ({ ...s, loading: true, error: null }));

    fetcherRef
      .current(controller.signal)
      .then((data) => {
        if (active) setState({ data, loading: false, error: null });
      })
      .catch((err) => {
        if (!active || controller.signal.aborted) return;
        setState({ data: null, loading: false, error: extractErrorMessage(err) });
      });

    return () => {
      active = false;
      controller.abort();
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [enabled, tick, ...deps]);

  const refetch = useCallback(() => setTick((t) => t + 1), []);

  // Allows local updates without a round trip.
  const setData = useCallback((updater) => {
    setState((s) => ({
      ...s,
      data: typeof updater === 'function' ? updater(s.data) : updater,
    }));
  }, []);

  return { ...state, refetch, setData };
}
