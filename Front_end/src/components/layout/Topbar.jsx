import { useNavigate } from 'react-router-dom';
import Badge from '../common/Badge';
import Button from '../common/Button';
import { useAuth } from '../../hooks/useAuth';
import { roleLabel } from '../../utils/roleHelpers';
import styles from './Topbar.module.css';

const ROLE_TONE = {
  Administrator: 'accent',
  WarehouseOperator: 'info',
  SalesAgent: 'success',
  Manager: 'neutral',
};

export default function Topbar({ onToggleSidebar, sidebarOpen }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login', { replace: true });
  }

  return (
    <header className={styles.topbar}>
      <button
        type="button"
        className={styles.menu}
        onClick={onToggleSidebar}
        aria-label={sidebarOpen ? 'Close navigation' : 'Open navigation'}
        aria-expanded={sidebarOpen}
      >
        <span aria-hidden="true" />
        <span aria-hidden="true" />
        <span aria-hidden="true" />
      </button>

      <div className={styles.spacer} />

      {user && (
        <div className={styles.user}>
          <span className={styles.name}>{user.fullName}</span>
          <Badge tone={ROLE_TONE[user.role] ?? 'neutral'}>{roleLabel(user.role)}</Badge>
        </div>
      )}
      <Button variant="ghost" size="sm" onClick={handleLogout}>
        Sign out
      </Button>
    </header>
  );
}
