import { ROLES, ROLE_LABELS } from '../config/constants';

export function hasRole(user, ...roles) {
  if (!user?.role) return false;
  if (roles.length === 0) return true;
  return roles.flat().includes(user.role);
}

export function isAdministrator(user) {
  return hasRole(user, ROLES.ADMINISTRATOR);
}

export function isWarehouseOperator(user) {
  return hasRole(user, ROLES.WAREHOUSE_OPERATOR);
}

export function isSalesAgent(user) {
  return hasRole(user, ROLES.SALES_AGENT);
}

export function isManager(user) {
  return hasRole(user, ROLES.MANAGER);
}

export function roleLabel(role) {
  return ROLE_LABELS[role] ?? role ?? 'Unknown role';
}
