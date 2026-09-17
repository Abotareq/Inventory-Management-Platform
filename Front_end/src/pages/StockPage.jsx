import { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Button from '../components/common/Button';
import ConfirmDialog from '../components/common/ConfirmDialog';
import EmptyState from '../components/common/EmptyState';
import ErrorMessage from '../components/common/ErrorMessage';
import Modal from '../components/common/Modal';
import Pagination from '../components/common/Pagination';
import PageHeader from '../components/layout/PageHeader';
import StockAdjustForm from '../components/stock/StockAdjustForm';
import StockAssignForm from '../components/stock/StockAssignForm';
import StockList from '../components/stock/StockList';
import StockPicker from '../components/stock/StockPicker';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import { useLookups } from '../hooks/useLookups';
import { usePagination } from '../hooks/usePagination';
import { useToast } from '../hooks/useToast';
import { extractErrorMessage } from '../services/apiClient';
import * as stockService from '../services/stockService';
import { isAdministrator, isWarehouseOperator } from '../utils/roleHelpers';

export default function StockPage() {
  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();
  const [params, setParams] = useSearchParams();

  const warehouseId = params.get('warehouseId') || '';
  const productId = params.get('productId') || '';
  const mode = warehouseId ? 'warehouse' : productId ? 'product' : null;

  const canAssign = isAdministrator(user);
  const canDelete = isAdministrator(user);
  const canAdjust = isWarehouseOperator(user);

  const lookups = useLookups();
  const paging = usePagination();
  const { pageNumber, pageSize } = paging;

  const stock = useFetch(
    (signal) =>
      mode === 'warehouse'
        ? stockService.getStockByWarehouse(warehouseId, { pageNumber, pageSize }, { signal })
        : stockService.getStockByProduct(productId, { pageNumber, pageSize }, { signal }),
    [mode, warehouseId, productId, pageNumber, pageSize],
    { enabled: Boolean(mode) },
  );

  function selectWarehouse(id) {
    paging.reset();
    setParams(id ? { warehouseId: id } : {});
  }

  function selectProduct(id) {
    paging.reset();
    setParams(id ? { productId: id } : {});
  }

  // dialog: null | { mode: 'assign' } | { mode: 'adjust', stock } | { mode: 'delete', stock }
  const [dialog, setDialog] = useState(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);

  const close = () => {
    setDialog(null);
    setDeleteError(null);
  };

  async function handleAssign(values) {
    await stockService.assignProductToWarehouse(values);
    const p = lookups.productsById[values.productId];
    const w = lookups.warehousesById[values.warehouseId];
    toast.success(`${p?.name ?? 'Product'} assigned to ${w?.name ?? 'warehouse'}`);
    close();
    // Jump to the dimension the user just touched so the new row is visible.
    if (mode === 'product' && values.productId === productId) stock.refetch();
    else if (mode === 'warehouse' && values.warehouseId === warehouseId) stock.refetch();
    else selectWarehouse(values.warehouseId);
  }

  async function handleAdjust(values) {
    await stockService.adjustStock(values);
    const verb = values.amount > 0 ? 'added to' : 'removed from';
    toast.success(`${Math.abs(values.amount)} ${verb} stock`);
    close();
    stock.refetch();
  }

  async function handleDelete() {
    setDeleting(true);
    setDeleteError(null);
    try {
      await stockService.deleteStock(dialog.stock.stockId);
      toast.success('Stock record deleted');
      close();
      stock.refetch();
    } catch (err) {
      setDeleteError(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  }

  const subject =
    mode === 'warehouse'
      ? lookups.warehousesById[warehouseId]
      : mode === 'product'
        ? lookups.productsById[productId]
        : null;

  const totalCount = stock.data?.totalCount ?? 0;

  const assignAction = canAssign ? (
    <Button variant="primary" onClick={() => setDialog({ mode: 'assign' })}>
      Assign product
    </Button>
  ) : null;

  return (
    <>
      <PageHeader
        title="Stock"
        meta={
          subject && (
            <span>
              {mode === 'warehouse' ? 'Warehouse' : 'Product'}: <strong>{subject.name}</strong>
              {mode === 'warehouse' && subject.location ? ` · ${subject.location}` : ''}
              {mode === 'product' && subject.sku ? (
                <>
                  {' · '}
                  <span className="num">{subject.sku}</span>
                </>
              ) : null}
            </span>
          )
        }
        actions={assignAction}
      />

      {lookups.error ? (
        <ErrorMessage
          title="Couldn't load products and warehouses"
          message={lookups.error}
          onRetry={lookups.refetch}
        />
      ) : (
        <StockPicker
          warehouses={lookups.warehouses}
          products={lookups.products}
          warehouseId={warehouseId}
          productId={productId}
          onWarehouseChange={selectWarehouse}
          onProductChange={selectProduct}
        />
      )}

      {!mode ? (
        <EmptyState
          title="Pick a warehouse or a product"
          description="Stock is tracked per product per warehouse. Choose one above to see what's on hand, reserved, and available."
        />
      ) : (
        <>
          <StockList
            mode={mode}
            stocks={stock.data?.items}
            productsById={lookups.productsById}
            warehousesById={lookups.warehousesById}
            loading={stock.loading || lookups.loading}
            error={stock.error}
            onRetry={stock.refetch}
            canAdjust={canAdjust}
            canDelete={canDelete}
            onAdjust={(row) => setDialog({ mode: 'adjust', stock: row })}
            onDelete={(row) => setDialog({ mode: 'delete', stock: row })}
            onOpenDetail={(row) => navigate(`/stock/${row.stockId}`, { state: { stock: row } })}
            emptyAction={assignAction}
          />
          <Pagination
            pageNumber={pageNumber}
            pageSize={pageSize}
            totalCount={totalCount}
            onPageChange={paging.setPage}
          />
        </>
      )}

      <Modal open={dialog?.mode === 'assign'} onClose={close} title="Assign product to warehouse">
        {dialog?.mode === 'assign' && (
          <StockAssignForm
            products={lookups.products}
            warehouses={lookups.warehouses}
            initial={{ productId, warehouseId }}
            onSubmit={handleAssign}
            onCancel={close}
          />
        )}
      </Modal>

      <Modal open={dialog?.mode === 'adjust'} onClose={close} title="Adjust stock">
        {dialog?.mode === 'adjust' && (
          <StockAdjustForm
            stock={dialog.stock}
            product={lookups.productsById[dialog.stock.productId]}
            warehouse={lookups.warehousesById[dialog.stock.warehouseId]}
            onSubmit={handleAdjust}
            onCancel={close}
          />
        )}
      </Modal>

      <ConfirmDialog
        open={dialog?.mode === 'delete'}
        onClose={close}
        onConfirm={handleDelete}
        title="Delete stock record"
        message={
          dialog?.stock
            ? `This removes the stock record for ${lookups.productsById[dialog.stock.productId]?.name ?? 'this product'} in ${lookups.warehousesById[dialog.stock.warehouseId]?.name ?? 'this warehouse'}. Records with quantity on hand or active reservations can't be deleted.`
            : ''
        }
        confirmLabel="Delete stock record"
        cancelLabel="Keep record"
        danger
        loading={deleting}
        error={deleteError}
      />
    </>
  );
}
