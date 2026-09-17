// GUIDs are long; show the first block for scanning, keep the full value in a title.
export function shortId(id) {
  if (!id || typeof id !== 'string') return '—';
  return id.split('-')[0];
}
