import Button from '../components/common/Button';
import EmptyState from '../components/common/EmptyState';

export default function NotFoundPage() {
  return (
    <EmptyState
      title="Page not found"
      description="The address may be wrong, or the page was moved."
      action={<Button to="/">Go to dashboard</Button>}
    />
  );
}
