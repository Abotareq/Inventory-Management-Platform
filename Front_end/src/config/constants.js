export const ROLES = Object.freeze({
  ADMINISTRATOR: 'Administrator',
  WAREHOUSE_OPERATOR: 'WarehouseOperator',
  SALES_AGENT: 'SalesAgent',
  MANAGER: 'Manager',
});

export const ROLE_LABELS = Object.freeze({
  [ROLES.ADMINISTRATOR]: 'Administrator',
  [ROLES.WAREHOUSE_OPERATOR]: 'Warehouse operator',
  [ROLES.SALES_AGENT]: 'Sales agent',
  [ROLES.MANAGER]: 'Manager',
});

export const ALL_ROLES = Object.freeze(Object.values(ROLES));

export const ORDER_STATUS = Object.freeze({
  DRAFT: 'Draft',
  SUBMITTED: 'Submitted',
  PROCESSING: 'Processing',
  COMPLETED: 'Completed',
  CANCELLED: 'Cancelled',
});

export const ORDER_STATUSES = Object.freeze(Object.values(ORDER_STATUS));

// Badge tone per order status. Tones map to --color-* tokens.
export const ORDER_STATUS_TONE = Object.freeze({
  [ORDER_STATUS.DRAFT]: 'neutral',
  [ORDER_STATUS.SUBMITTED]: 'accent',
  [ORDER_STATUS.PROCESSING]: 'info',
  [ORDER_STATUS.COMPLETED]: 'success',
  [ORDER_STATUS.CANCELLED]: 'danger',
});

export const RESERVATION_ACTION = Object.freeze({
  RESERVED: 'Reserved',
  RELEASED: 'Released',
  COMMITTED: 'Committed',
});

export const RESERVATION_ACTION_TONE = Object.freeze({
  [RESERVATION_ACTION.RESERVED]: 'info',
  [RESERVATION_ACTION.RELEASED]: 'neutral',
  [RESERVATION_ACTION.COMMITTED]: 'success',
});

export const DEFAULT_PAGE_SIZE = 20;
export const PAGE_SIZE_OPTIONS = Object.freeze([10, 20, 50]);

export const STORAGE_KEYS = Object.freeze({
  ACCESS_TOKEN: 'imp.accessToken',
  REFRESH_TOKEN: 'imp.refreshToken',
  USER: 'imp.user',
});

// Stock at or below this available quantity is flagged as low on the dashboard.
export const LOW_STOCK_THRESHOLD = 5;
