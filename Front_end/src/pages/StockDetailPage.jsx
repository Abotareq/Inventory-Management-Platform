import { useState } from 'react';
import { Link, useLocation, useParams } from 'react-router-dom';
import Button from '../components/common/Button';
import Pagination from '../components/common/Pagination';
import Tabs, { TabPanel } from '../components/common/Tabs';
import PageHeader from '../components/layout/PageHeader';
import StockHistory from '../components/stock/StockHistory';
import StockLevelBadge from '../components/stock/StockLevelBadge';
import StockReservationHistory from '../components/stock/StockReservationHistory';
import { useFetch } from '../hooks/useFetch';
import { useLookups } from '../hooks/useLookups';
import { usePagination } from '../hooks/usePagination';
import * as stockService from '../services/stockService';
import styles from './StockDetailPage.module.css';

const TABS = [
  { id: 'adjustments', label: 'Adjustments' },
  { id: 'reservations', label: 'Reservations' },
];

export default function StockDetailPage() {
  const { stockId } = useParams();
  const location = useLocation();
  // The API has no GET /stocks/{id}; the browser passes the row through router state.
  const stock = location.state?.stock ?? null;

  const lookups = useLookups();
  const product = stock ? lookups.productsById[stock.productId] : null;
  const warehouse = stock ? lookups.warehousesById[stock.warehouseId] : null;

  const [tab, setTab] = useState('adjustments');

  const adjPaging = usePagination();
  const resPaging = usePagination();

  const adjustments = useFetch(
    (signal) =>
      stockService.getStockHistory(
        stockId,
        { pageNumber: adjPaging.pageNumber, pageSize: adjPaging.pageSize },
        { signal },
      ),
    [stockId, adjPaging.pageNumber, adjPaging.pageSize],
  );

  const reservations = useFetch(
    (signal) =>
      stockService.getStockReservations(
        stockId,
        { pageNumber: resPaging.pageNumber, pageSize: resPaging.pageSize },
        { signal },
      ),
    [stockId, resPaging.pageNumber, resPaging.pageSize],
  );

  const backTo = stock
    ? `/stock?warehouseId=${stock.warehouseId}`
    : '/stock';

  return (
    <>
      <PageHeader
        title={product ? product.name : 'Stock record'}
        meta={
          <>
            {product?.sku && <span className="num">{product.sku}</span>}
            {warehouse && <span>{warehouse.name}</span>}
            <span className="num" title={stockId}>
              Stock {stockId.split('-')[0]}
            </span>
          </>
        }
        actions={
          <Button variant="ghost" to={backTo}>
            Back to stock
          </Button>
        }
      />

      {stock ? (
        <dl className={styles.summary}>
          <div className={styles.stat}>
            <dt>On hand</dt>
            <dd className="num">{stock.quantity}</dd>
          </div>
          <div className={styles.stat}>
            <dt>Reserved</dt>
            <dd className="num">{stock.reserved}</dd>
          </div>
          <div className={styles.stat}>
            <dt>Available</dt>
            <dd className="num">{stock.available}</dd>
          </div>
          <div className={styles.stat}>
            <dt>Level</dt>
            <dd>
              <StockLevelBadge available={stock.available} />
            </dd>
          </div>
        </dl>
      ) : (
        <p className={styles.note}>
          Current quantities aren't shown when this page is opened directly.{' '}
          <Link to="/stock">Open it from the stock browser</Link> to see on-hand, reserved and
          available counts alongside the history.
        </p>
      )}

      <Tabs
        tabs={TABS.map((t) => ({
          ...t,
          count:
            t.id === 'adjustments'
              ? adjustments.data?.totalCount
              : reservations.data?.totalCount,
        }))}
        value={tab}
        onChange={setTab}
        label="Stock history"
      />

      <TabPanel id="adjustments" active={tab === 'adjustments'}>
        <StockHistory
          rows={adjustments.data?.items}
          loading={adjustments.loading}
          error={adjustments.error}
          onRetry={adjustments.refetch}
        />
        <Pagination
          pageNumber={adjPaging.pageNumber}
          pageSize={adjPaging.pageSize}
          totalCount={adjustments.data?.totalCount ?? 0}
          onPageChange={adjPaging.setPage}
        />
      </TabPanel>

      <TabPanel id="reservations" active={tab === 'reservations'}>
        <StockReservationHistory
          rows={reservations.data?.items}
          loading={reservations.loading}
          error={reservations.error}
          onRetry={reservations.refetch}
        />
        <Pagination
          pageNumber={resPaging.pageNumber}
          pageSize={resPaging.pageSize}
          totalCount={reservations.data?.totalCount ?? 0}
          onPageChange={resPaging.setPage}
        />
      </TabPanel>
    </>
  );
}
