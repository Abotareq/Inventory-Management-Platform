import { NavLink } from 'react-router-dom';
import { APP_NAME } from '../../config/config';
import { ROLES } from '../../config/constants';
import { useAuth } from '../../hooks/useAuth';
import { hasRole } from '../../utils/roleHelpers';
import styles from './Sidebar.module.css';

// Items with `roles` are hidden from users outside those roles.
const NAV = [
  { to: '/', label: 'Dashboard', end: true },
  { to: '/orders', label: 'Orders' },
  { to: '/products', label: 'Products' },
  { to: '/stock', label: 'Stock' },
  { to: '/warehouses', label: 'Warehouses' },
  { to: '/categories', label: 'Categories', roles: [ROLES.ADMINISTRATOR] },
  { to: '/audit-logs', label: 'Audit logs', roles: [ROLES.ADMINISTRATOR] },
  { to: '/users/new', label: 'Register user', roles: [ROLES.ADMINISTRATOR] },
];

export default function Sidebar({ open, onNavigate }) {
  const { user } = useAuth();
  const items = NAV.filter((item) => !item.roles || hasRole(user, ...item.roles));

  return (
    <aside className={[styles.sidebar, open && styles.open].filter(Boolean).join(' ')}>
      <div className={styles.brand}>
        <span className={styles.brandMark} aria-hidden="true" />
        <span className={styles.brandName}>{APP_NAME}</span>
      </div>
      <nav className={styles.nav} aria-label="Main">
        <ul className={styles.list}>
          {items.map((item) => (
            <li key={item.to}>
              <NavLink
                to={item.to}
                end={item.end}
                onClick={onNavigate}
                className={({ isActive }) =>
                  [styles.link, isActive && styles.active].filter(Boolean).join(' ')
                }
              >
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
    </aside>
  );
}
