import { useMemo } from 'react';
import Button from '../components/common/Button';
import LowStockList from '../components/dashboard/LowStockList';
import RecentOrders from '../components/dashboard/RecentOrders';
import StatTile from '../components/dashboard/StatTile';
import PageHeader from '../components/layout/PageHeader';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import { useLookups } from '../hooks/useLookups';
import * as dashboardService from '../services/dashboardService';
import { isSalesAgent, roleLabel } from '../utils/roleHelpers';
import styles from './DashboardPage.module.css';

export default function DashboardPage() {
  const { user } = useAuth();
  const lookups = useLookups();

  const summary = useFetch((signal) => dashboardService.getSummary({ signal }), []);
  const recent = useFetch((signal) => dashboardService.getRecentOrders({ signal }), []);
  const lowStock = useFetch(
    (signal) => dashboardService.getLowStock(lookups.warehouses, { signal }),
    [lookups.warehouses],
    { enabled: lookups.warehouses.length > 0 },
  );

  const lowRows = useMemo(
    () =>
      (lowStock.data ?? []).map((s) => ({
        ...s,
        product: lookups.productsById[s.productId],
        warehouse: lookups.warehousesById[s.warehouseId],
      })),
    [lowStock.data, lookups.productsById, lookups.warehousesById],
  );

  const s = summary.data;
  const openOrders = s ? s.ordersSubmitted + s.ordersProcessing : null;

  return (
    <>
      <PageHeader
        title="Dashboard"
        meta={<span>{user?.fullName} · {roleLabel(user?.role)}</span>}
        actions={
          isSalesAgent(user) && (
            <Button variant="primary" to="/orders/new">
              Create order
            </Button>
          )
        }
      />

      <div className={styles.tiles}>
        <StatTile
          label="Open orders"
          value={openOrders}
          hint={s ? `${s.ordersSubmitted} submitted · ${s.ordersProcessing} processing` : undefined}
          to="/orders?status=Submitted"
          tone="accent"
          loading={summary.loading}
        />
        <StatTile
          label="Draft orders"
          value={s?.ordersDraft}
          to="/orders?status=Draft"
          loading={summary.loading}
        />
        <StatTile
          label="Low stock items"
          value={lookups.warehouses.length === 0 && !lookups.loading ? 0 : lowStock.data?.length}
          hint="At or below 5 available"
          tone={lowStock.data?.length ? 'danger' : undefined}
          to="/stock"
          loading={lowStock.loading || lookups.loading}
        />
        <StatTile label="Products" value={s?.products} to="/products" loading={summary.loading} />
        <StatTile label="Warehouses" value={s?.warehouses} to="/warehouses" loading={summary.loading} />
        <StatTile label="Categories" value={s?.categories} to="/categories" loading={summary.loading} />
      </div>

      {summary.error && (
        <p className={styles.warn} role="alert">
          Some counts couldn't be loaded: {summary.error}
        </p>
      )}

      <div className={styles.columns}>
        <section className={styles.section} aria-labelledby="recent-heading">
          <div className={styles.sectionHead}>
            <h2 id="recent-heading" className={styles.sectionTitle}>Recent orders</h2>
            <Button variant="ghost" size="sm" to="/orders">View all</Button>
          </div>
          <RecentOrders
            orders={recent.data}
            loading={recent.loading}
            error={recent.error}
            onRetry={recent.refetch}
            canCreate={isSalesAgent(user)}
          />
        </section>

        <section className={styles.section} aria-labelledby="low-heading">
          <div className={styles.sectionHead}>
            <h2 id="low-heading" className={styles.sectionTitle}>Low stock</h2>
            <Button variant="ghost" size="sm" to="/stock">Browse stock</Button>
          </div>
          <LowStockList
            rows={lowRows}
            loading={lowStock.loading || lookups.loading}
            error={lowStock.error || lookups.error}
            onRetry={() => {
              lookups.refetch();
              lowStock.refetch();
            }}
          />
        </section>
      </div>
    </>
  );
}
