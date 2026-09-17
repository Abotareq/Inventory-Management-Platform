import Input from '../common/Input';
import Select from '../common/Select';
import { PAGE_SIZE_OPTIONS } from '../../config/constants';
import styles from './ProductFilters.module.css';

// The products endpoint only pages; search and category narrow the loaded page.
export default function ProductFilters({
  search,
  onSearchChange,
  categoryId,
  onCategoryChange,
  categories,
  pageSize,
  onPageSizeChange,
}) {
  return (
    <div className={styles.filters} role="search" aria-label="Filter products on this page">
      <Input
        label="Search this page"
        type="search"
        placeholder="Name or SKU"
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
        className={styles.search}
      />
      <Select
        label="Category"
        value={categoryId}
        onChange={(e) => onCategoryChange(e.target.value)}
        placeholder="All categories"
        options={(categories ?? []).map((c) => ({ value: c.id, label: c.name }))}
        className={styles.category}
      />
      <Select
        label="Rows per page"
        value={pageSize}
        onChange={(e) => onPageSizeChange(Number(e.target.value))}
        options={PAGE_SIZE_OPTIONS.map((n) => ({ value: n, label: String(n) }))}
        className={styles.pageSize}
      />
    </div>
  );
}
