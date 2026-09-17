import { LOW_STOCK_THRESHOLD, ORDER_STATUS } from '../config/constants';
import { getCategories } from './categoryService';
import { getOrders } from './orderService';
import { getProducts } from './productService';
import { getStockByWarehouse } from './stockService';
import { getWarehouses } from './warehouseService';

// The API has no summary endpoint, so the dashboard composes a few cheap calls.
// Counts use pageSize=1 and read totalCount.

export async function getSummary(config) {
  const [products, categories, warehouses, submitted, processing, drafts] = await Promise.all([
    getProducts({ pageNumber: 1, pageSize: 1 }, config),
    getCategories(config),
    getWarehouses(config),
    getOrders({ pageNumber: 1, pageSize: 1, status: ORDER_STATUS.SUBMITTED }, config),
    getOrders({ pageNumber: 1, pageSize: 1, status: ORDER_STATUS.PROCESSING }, config),
    getOrders({ pageNumber: 1, pageSize: 1, status: ORDER_STATUS.DRAFT }, config),
  ]);
  return {
    products: products.totalCount ?? 0,
    categories: categories.length,
    warehouses: warehouses.length,
    ordersSubmitted: submitted.totalCount ?? 0,
    ordersProcessing: processing.totalCount ?? 0,
    ordersDraft: drafts.totalCount ?? 0,
  };
}

export async function getRecentOrders(config, { limit = 6 } = {}) {
  const page = await getOrders({ pageNumber: 1, pageSize: limit }, config);
  return page.items ?? [];
}

// Walks stock per warehouse (no global stock endpoint) and keeps low/out items.
export async function getLowStock(warehouses, config, { maxWarehouses = 20, pageSize = 100 } = {}) {
  const pages = await Promise.all(
    warehouses.slice(0, maxWarehouses).map((w) =>
      getStockByWarehouse(w.id, { pageNumber: 1, pageSize }, config).then((p) => p.items ?? []),
    ),
  );
  return pages
    .flat()
    .filter((s) => (Number(s.available) || 0) <= LOW_STOCK_THRESHOLD)
    .sort((a, b) => a.available - b.available);
}
