const formatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: 'USD',
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

export function formatCurrency(value) {
  const n = Number(value);
  if (!Number.isFinite(n)) return '—';
  return formatter.format(n);
}

// Products created before the price field existed carry price 0.
export function isUnpriced(value) {
  return !Number.isFinite(Number(value)) || Number(value) === 0;
}
