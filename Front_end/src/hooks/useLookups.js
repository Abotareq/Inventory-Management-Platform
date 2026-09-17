import { useMemo } from 'react';
import { useFetch } from './useFetch';
import * as productService from '../services/productService';
import * as warehouseService from '../services/warehouseService';

// Loads all products and warehouses once and exposes id → record maps.
// Used wherever stock/order rows only carry ids.
export function useLookups({ products = true, warehouses = true } = {}) {
  const productsQuery = useFetch(
    (signal) => productService.getAllProducts({ signal }),
    [],
    { enabled: products },
  );
  const warehousesQuery = useFetch(
    (signal) => warehouseService.getWarehouses({ signal }),
    [],
    { enabled: warehouses },
  );

  const productsById = useMemo(
    () => Object.fromEntries((productsQuery.data ?? []).map((p) => [p.id, p])),
    [productsQuery.data],
  );
  const warehousesById = useMemo(
    () => Object.fromEntries((warehousesQuery.data ?? []).map((w) => [w.id, w])),
    [warehousesQuery.data],
  );

  return {
    products: productsQuery.data ?? [],
    warehouses: warehousesQuery.data ?? [],
    productsById,
    warehousesById,
    loading: productsQuery.loading || warehousesQuery.loading,
    error: productsQuery.error || warehousesQuery.error,
    refetch: () => {
      productsQuery.refetch();
      warehousesQuery.refetch();
    },
  };
}
