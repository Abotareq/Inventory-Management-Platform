import Badge from '../common/Badge';
import { ORDER_STATUS_TONE } from '../../config/constants';

export default function OrderStatusBadge({ status }) {
  return <Badge tone={ORDER_STATUS_TONE[status] ?? 'neutral'}>{status ?? 'Unknown'}</Badge>;
}
