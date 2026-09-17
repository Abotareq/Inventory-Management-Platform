import { useState } from 'react';
import CategoryForm from '../components/categories/CategoryForm';
import CategoryList from '../components/categories/CategoryList';
import Button from '../components/common/Button';
import ConfirmDialog from '../components/common/ConfirmDialog';
import Modal from '../components/common/Modal';
import PageHeader from '../components/layout/PageHeader';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import { useToast } from '../hooks/useToast';
import { extractErrorMessage } from '../services/apiClient';
import * as categoryService from '../services/categoryService';
import { isAdministrator } from '../utils/roleHelpers';

export default function CategoriesPage() {
  const { user } = useAuth();
  const toast = useToast();
  const canEdit = isAdministrator(user);

  const { data, loading, error, refetch } = useFetch(
    (signal) => categoryService.getCategories({ signal }),
    [],
  );

  // dialog: null | { mode: 'create' } | { mode: 'edit', category } | { mode: 'delete', category }
  const [dialog, setDialog] = useState(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);

  const close = () => {
    setDialog(null);
    setDeleteError(null);
  };

  async function handleCreate(values) {
    await categoryService.createCategory(values);
    toast.success(`Category "${values.name}" created`);
    close();
    refetch();
  }

  async function handleRename(values) {
    await categoryService.updateCategory(dialog.category.id, values);
    toast.success(`Category renamed to "${values.name}"`);
    close();
    refetch();
  }

  async function handleDelete() {
    setDeleting(true);
    setDeleteError(null);
    try {
      await categoryService.deleteCategory(dialog.category.id);
      toast.success(`Category "${dialog.category.name}" deleted`);
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
        title="Categories"
        meta={!loading && !error && <span><span className="num">{count}</span> {count === 1 ? 'category' : 'categories'}</span>}
        actions={
          canEdit && (
            <Button variant="primary" onClick={() => setDialog({ mode: 'create' })}>
              Create category
            </Button>
          )
        }
      />

      <CategoryList
        categories={data}
        loading={loading}
        error={error}
        onRetry={refetch}
        canEdit={canEdit}
        onCreate={() => setDialog({ mode: 'create' })}
        onEdit={(category) => setDialog({ mode: 'edit', category })}
        onDelete={(category) => setDialog({ mode: 'delete', category })}
      />

      <Modal open={dialog?.mode === 'create'} onClose={close} title="Create category">
        <CategoryForm onSubmit={handleCreate} onCancel={close} />
      </Modal>

      <Modal open={dialog?.mode === 'edit'} onClose={close} title="Rename category">
        {dialog?.mode === 'edit' && (
          <CategoryForm initial={dialog.category} onSubmit={handleRename} onCancel={close} />
        )}
      </Modal>

      <ConfirmDialog
        open={dialog?.mode === 'delete'}
        onClose={close}
        onConfirm={handleDelete}
        title="Delete category"
        message={`"${dialog?.category?.name}" will be removed. Products still assigned to it will block the delete.`}
        confirmLabel="Delete category"
        cancelLabel="Keep category"
        danger
        loading={deleting}
        error={deleteError}
      />
    </>
  );
}
