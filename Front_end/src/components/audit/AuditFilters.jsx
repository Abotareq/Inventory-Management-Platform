import Button from '../common/Button';
import Input from '../common/Input';
import Select from '../common/Select';
import styles from './AuditFilters.module.css';

const ENTITIES = ['Order', 'Product', 'Category', 'Warehouse', 'Stock'];

export default function AuditFilters({ values, onChange, onClear }) {
  const set = (field) => (e) => onChange({ ...values, [field]: e.target.value });
  const active = Boolean(values.entityName || values.entityId);
  const known = !values.entityName || ENTITIES.includes(values.entityName);

  return (
    <div className={styles.filters} role="search" aria-label="Filter audit log">
      <Select
        label="Entity"
        value={known ? values.entityName : ''}
        onChange={set('entityName')}
        placeholder="Any entity"
        options={ENTITIES.map((e) => ({ value: e, label: e }))}
        className={styles.entity}
      />
      <Input
        label="Entity ID"
        value={values.entityId}
        onChange={set('entityId')}
        placeholder="Full record ID"
        mono
        className={styles.id}
      />
      {active && (
        <Button variant="ghost" size="sm" onClick={onClear} className={styles.clear}>
          Clear filters
        </Button>
      )}
    </div>
  );
}
