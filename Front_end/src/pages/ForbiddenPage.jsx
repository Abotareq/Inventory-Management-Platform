import Button from '../components/common/Button';
import EmptyState from '../components/common/EmptyState';
import { useAuth } from '../hooks/useAuth';
import { roleLabel } from '../utils/roleHelpers';

export default function ForbiddenPage() {
  const { user } = useAuth();
  return (
    <EmptyState
      title="You can't open this page"
      description={`Your role (${roleLabel(user?.role)}) doesn't include access here. Ask an administrator if you need it.`}
      action={<Button to="/">Go to dashboard</Button>}
    />
  );
}
