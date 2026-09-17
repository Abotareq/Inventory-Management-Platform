import { useState } from 'react';
import Button from '../common/Button';
import ConfirmDialog from '../common/ConfirmDialog';
import { ORDER_STATUS, ROLES } from '../../config/constants';
import { useAuth } from '../../hooks/useAuth';
import { hasRole } from '../../utils/roleHelpers';
import styles from './OrderActionButtons.module.css';

// Valid transitions per status. Cancel is allowed from any state before Completed.
const TRANSITIONS = {
  [ORDER_STATUS.DRAFT]: [
    { action: 'submit', label: 'Submit order', roles: [ROLES.SALES_AGENT], variant: 'primary' },
  ],
  [ORDER_STATUS.SUBMITTED]: [
    { action: 'beginProcessing', label: 'Start processing', roles: [ROLES.WAREHOUSE_OPERATOR], variant: 'primary' },
  ],
  [ORDER_STATUS.PROCESSING]: [
    { action: 'complete', label: 'Complete order', roles: [ROLES.WAREHOUSE_OPERATOR], variant: 'primary' },
  ],
};

const CANCELLABLE = [ORDER_STATUS.DRAFT, ORDER_STATUS.SUBMITTED, ORDER_STATUS.PROCESSING];
const CANCEL_ROLES = [ROLES.SALES_AGENT, ROLES.WAREHOUSE_OPERATOR];

// `onAction(action)` performs the transition and may throw; the busy state is per button.
export default function OrderActionButtons({ order, onAction, busy, cancelError, onCancelErrorClear }) {
  const { user } = useAuth();
  const [confirmCancel, setConfirmCancel] = useState(false);

  if (!order) return null;

  const primary = (TRANSITIONS[order.status] ?? []).filter((t) => hasRole(user, ...t.roles));
  const canCancel = CANCELLABLE.includes(order.status) && hasRole(user, ...CANCEL_ROLES);

  if (primary.length === 0 && !canCancel) return null;

  const releasesStock = order.status === ORDER_STATUS.PROCESSING;

  async function handleCancel() {
    try {
      await onAction('cancel');
      setConfirmCancel(false);
    } catch {
      // Error is shown inside the dialog via `cancelError`.
    }
  }

  return (
    <div className={styles.actions}>
      {primary.map((t) => (
        <Button
          key={t.action}
          variant={t.variant}
          onClick={() => onAction(t.action)}
          loading={busy === t.action}
          disabled={Boolean(busy) && busy !== t.action}
        >
          {t.label}
        </Button>
      ))}
      {canCancel && (
        <Button
          variant="danger"
          onClick={() => setConfirmCancel(true)}
          disabled={Boolean(busy)}
        >
          Cancel order
        </Button>
      )}

      <ConfirmDialog
        open={confirmCancel}
        onClose={() => {
          setConfirmCancel(false);
          onCancelErrorClear?.();
        }}
        onConfirm={handleCancel}
        title="Cancel order"
        message={
          releasesStock
            ? 'This order is being processed. Cancelling it releases the reserved stock back to the warehouses.'
            : 'This order has not reserved any stock yet. Cancelling it cannot be undone.'
        }
        confirmLabel="Cancel order"
        cancelLabel="Keep order"
        danger
        loading={busy === 'cancel'}
        error={cancelError}
      />
    </div>
  );
}
