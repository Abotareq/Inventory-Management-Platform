import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/common/Button';
import ConfirmDialog from '../components/common/ConfirmDialog';
import Modal from '../components/common/Modal';
import PageHeader from '../components/layout/PageHeader';
import WarehouseForm from '../components/warehouses/WarehouseForm';
import WarehouseList from '../components/warehouses/WarehouseList';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import { useToast } from '../hooks/useToast';
import { extractErrorMessage } from '../services/apiClient';
import * as warehouseService from '../services/warehouseService';
import { isAdministrator } from '../utils/roleHelpers';

export default function WarehousesPage() {
  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();
  const canEdit = isAdministrator(user);

  const { data, loading, error, refetch } = useFetch(
    (signal) => warehouseService.getWarehouses({ signal }),
    [],
  );

  // dialog: null | { mode: 'create' } | { mode: 'edit', warehouse } | { mode: 'delete', warehouse }
  const [dialog, setDialog] = useState(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);

  const close = () => {
    setDialog(null);
    setDeleteError(null);
  };

  async function handleCreate(values) {
    await warehouseService.createWarehouse(values);
    toast.success(`Warehouse "${values.name}" created`);
    close();
    refetch();
  }

  async function handleUpdate(values) {
    await warehouseService.updateWarehouse(dialog.warehouse.id, values);
    toast.success(`Warehouse "${values.name}" updated`);
    close();
    refetch();
  }

  async function handleDelete() {
    setDeleting(true);
    setDeleteError(null);
    try {
      await warehouseService.deleteWarehouse(dialog.warehouse.id);
      toast.success(`Warehouse "${dialog.warehouse.name}" deleted`);
      close();
      refetch();
    } catch (err) {
      setDeleteError(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  }

  const count = data?.length ?? 0;

  return (
    <>
      <PageHeader
        title="Warehouses"
        meta={
          !loading &&
          !error && (
            <span>
              <span className="num">{count}</span> {count === 1 ? 'warehouse' : 'warehouses'}
            </span>
          )
        }
        actions={
          canEdit && (
            <Button variant="primary" onClick={() => setDialog({ mode: 'create' })}>
              Create warehouse
            </Button>
          )
        }
      />

      <WarehouseList
        warehouses={data}
        loading={loading}
        error={error}
        onRetry={refetch}
        canEdit={canEdit}
        onCreate={() => setDialog({ mode: 'create' })}
        onEdit={(warehouse) => setDialog({ mode: 'edit', warehouse })}
        onDelete={(warehouse) => setDialog({ mode: 'delete', warehouse })}
        onOpenStock={(warehouse) => navigate(`/stock?warehouseId=${warehouse.id}`)}
      />

      <Modal open={dialog?.mode === 'create'} onClose={close} title="Create warehouse">
        <WarehouseForm onSubmit={handleCreate} onCancel={close} />
      </Modal>

      <Modal open={dialog?.mode === 'edit'} onClose={close} title="Edit warehouse">
        {dialog?.mode === 'edit' && (
          <WarehouseForm initial={dialog.warehouse} onSubmit={handleUpdate} onCancel={close} />
        )}
      </Modal>

      <ConfirmDialog
        open={dialog?.mode === 'delete'}
        onClose={close}
        onConfirm={handleDelete}
        title="Delete warehouse"
        message={`"${dialog?.warehouse?.name}" will be removed. Any stock still assigned to it will block the delete.`}
        confirmLabel="Delete warehouse"
        cancelLabel="Keep warehouse"
        danger
        loading={deleting}
        error={deleteError}
      />
    </>
  );
}
