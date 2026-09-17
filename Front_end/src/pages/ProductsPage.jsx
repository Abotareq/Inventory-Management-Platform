import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Button from '../components/common/Button';
import ConfirmDialog from '../components/common/ConfirmDialog';
import Modal from '../components/common/Modal';
import Pagination from '../components/common/Pagination';
import PageHeader from '../components/layout/PageHeader';
import ProductFilters from '../components/products/ProductFilters';
import ProductForm from '../components/products/ProductForm';
import ProductList from '../components/products/ProductList';
import { useAuth } from '../hooks/useAuth';
import { useFetch } from '../hooks/useFetch';
import { usePagination } from '../hooks/usePagination';
import { useToast } from '../hooks/useToast';
import { extractErrorMessage } from '../services/apiClient';
import * as categoryService from '../services/categoryService';
import * as productService from '../services/productService';
import { isAdministrator } from '../utils/roleHelpers';

export default function ProductsPage() {
  const { user } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();
  const canEdit = isAdministrator(user);

  const paging = usePagination();
  const { pageNumber, pageSize } = paging;

  const products = useFetch(
    (signal) => productService.getProducts({ pageNumber, pageSize }, { signal }),
    [pageNumber, pageSize],
  );
  const categories = useFetch((signal) => categoryService.getCategories({ signal }), []);

  const categoriesById = useMemo(
    () => Object.fromEntries((categories.data ?? []).map((c) => [c.id, c])),
    [categories.data],
  );

  const [search, setSearch] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');

  const visible = useMemo(() => {
    const items = products.data?.items ?? [];
    const q = search.trim().toLowerCase();
    return items.filter((p) => {
      if (categoryFilter && p.categoryId !== categoryFilter) return false;
      if (!q) return true;
      return p.name.toLowerCase().includes(q) || p.sku.toLowerCase().includes(q);
    });
  }, [products.data, search, categoryFilter]);

  const filtered = Boolean(search.trim() || categoryFilter);

  // dialog: null | { mode: 'create' } | { mode: 'edit', product } | { mode: 'delete', product }
  const [dialog, setDialog] = useState(null);
  const [deleting, setDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);

  const close = () => {
    setDialog(null);
    setDeleteError(null);
  };

  async function handleCreate(values) {
    await productService.createProduct(values);
    toast.success(`Product "${values.name}" created`);
    close();
    products.refetch();
  }

  async function handleUpdate(values) {
    await productService.updateProduct(dialog.product.id, values);
    toast.success(`Product "${values.name}" updated`);
    close();
    products.refetch();
  }

  async function handleDelete() {
    setDeleting(true);
    setDeleteError(null);
    try {
      await productService.deleteProduct(dialog.product.id);
      toast.success(`Product "${dialog.product.name}" deleted`);
      close();
      products.refetch();
    } catch (err) {
      setDeleteError(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  }

  const totalCount = products.data?.totalCount ?? 0;

  return (
    <>
      <PageHeader
        title="Products"
        meta={
          !products.loading &&
          !products.error && (
            <span>
              <span className="num">{totalCount}</span> {totalCount === 1 ? 'product' : 'products'}
            </span>
          )
        }
        actions={
          canEdit && (
            <Button variant="primary" onClick={() => setDialog({ mode: 'create' })}>
              Create product
            </Button>
          )
        }
      />

      <ProductFilters
        search={search}
        onSearchChange={setSearch}
        categoryId={categoryFilter}
        onCategoryChange={setCategoryFilter}
        categories={categories.data}
        pageSize={pageSize}
        onPageSizeChange={paging.setPageSize}
      />

      <ProductList
        products={visible}
        categoriesById={categoriesById}
        loading={products.loading}
        error={products.error}
        onRetry={products.refetch}
        canEdit={canEdit}
        filtered={filtered}
        onCreate={() => setDialog({ mode: 'create' })}
        onEdit={(product) => setDialog({ mode: 'edit', product })}
        onDelete={(product) => setDialog({ mode: 'delete', product })}
        onOpenStock={(product) => navigate(`/stock?productId=${product.id}`)}
      />

      <Pagination
        pageNumber={pageNumber}
        pageSize={pageSize}
        totalCount={totalCount}
        onPageChange={paging.setPage}
      />

      <Modal open={dialog?.mode === 'create'} onClose={close} title="Create product" size="lg">
        <ProductForm categories={categories.data} onSubmit={handleCreate} onCancel={close} />
      </Modal>

      <Modal open={dialog?.mode === 'edit'} onClose={close} title="Edit product" size="lg">
        {dialog?.mode === 'edit' && (
          <ProductForm
            initial={dialog.product}
            categories={categories.data}
            onSubmit={handleUpdate}
            onCancel={close}
          />
        )}
      </Modal>

      <ConfirmDialog
        open={dialog?.mode === 'delete'}
        onClose={close}
        onConfirm={handleDelete}
        title="Delete product"
        message={`"${dialog?.product?.name}" (${dialog?.product?.sku}) will be removed. Stock records or orders that reference it will block the delete.`}
        confirmLabel="Delete product"
        cancelLabel="Keep product"
        danger
        loading={deleting}
        error={deleteError}
      />
    </>
  );
}
