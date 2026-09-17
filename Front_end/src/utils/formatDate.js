const dateTime = new Intl.DateTimeFormat('en-GB', {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
  hour: '2-digit',
  minute: '2-digit',
  hour12: false,
});

const dateOnly = new Intl.DateTimeFormat('en-GB', {
  year: 'numeric',
  month: '2-digit',
  day: '2-digit',
});

function toDate(value) {
  if (!value) return null;
  const d = value instanceof Date ? value : new Date(value);
  return Number.isNaN(d.getTime()) ? null : d;
}

function parts(fmt, d) {
  return Object.fromEntries(fmt.formatToParts(d).map((p) => [p.type, p.value]));
}

// 24-hour, zero-padded, ISO-like ordering: "2026-08-24 14:58"
export function formatDateTime(value) {
  const d = toDate(value);
  if (!d) return '—';
  const p = parts(dateTime, d);
  return `${p.year}-${p.month}-${p.day} ${p.hour}:${p.minute}`;
}

export function formatDate(value) {
  const d = toDate(value);
  if (!d) return '—';
  const p = parts(dateOnly, d);
  return `${p.year}-${p.month}-${p.day}`;
}

// For <input type="date"> values.
export function toDateInputValue(value) {
  const d = toDate(value);
  return d ? formatDate(d) : '';
}

export function formatRelative(value) {
  const d = toDate(value);
  if (!d) return '—';
  const minutes = Math.round((Date.now() - d.getTime()) / 60_000);
  if (minutes < 1) return 'just now';
  if (minutes < 60) return `${minutes} min ago`;
  const hours = Math.round(minutes / 60);
  if (hours < 24) return `${hours} h ago`;
  const days = Math.round(hours / 24);
  if (days < 7) return `${days} d ago`;
  return formatDate(d);
}
