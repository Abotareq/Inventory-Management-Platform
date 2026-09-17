import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import { ORDER_STATUSES } from '../../config/constants';
import styles from './OrderFilters.module.css';

// Server-side filters for GET /orders. `values`: { status, fromDate, toDate, customerId }.
export default function OrderFilters({ values, onChange, onClear }) {
  const set = (field) => (e) => onChange({ ...values, [field]: e.target.value });
  const active = Object.values(values).some(Boolean);

  return (
    <div className={styles.filters} role="search" aria-label="Filter orders">
      <Select
        label="Status"
        value={values.status}
        onChange={set('status')}
        placeholder="Any status"
        options={ORDER_STATUSES.map((s) => ({ value: s, label: s }))}
        className={styles.status}
      />
      <Input label="From" type="date" value={values.fromDate} onChange={set('fromDate')} className={styles.date} />
      <Input label="To" type="date" value={values.toDate} onChange={set('toDate')} className={styles.date} />
      <Input
        label="Customer ID"
        value={values.customerId}
        onChange={set('customerId')}
        placeholder="Full customer ID"
        mono
        className={styles.customer}
      />
      {active && (
        <Button variant="ghost" size="sm" onClick={onClear} className={styles.clear}>
          Clear filters
        </Button>
      )}
    </div>
  );
}
