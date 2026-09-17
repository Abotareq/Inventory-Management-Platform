import Badge from '../common/Badge';
import { LOW_STOCK_THRESHOLD } from '../../config/constants';

export function stockLevel(available) {
  const n = Number(available) || 0;
  if (n <= 0) return { tone: 'danger', label: 'Out of stock' };
  if (n <= LOW_STOCK_THRESHOLD) return { tone: 'accent', label: 'Low' };
  return { tone: 'success', label: 'In stock' };
}

export default function StockLevelBadge({ available }) {
  const { tone, label } = stockLevel(available);
  return <Badge tone={tone}>{label}</Badge>;
}
