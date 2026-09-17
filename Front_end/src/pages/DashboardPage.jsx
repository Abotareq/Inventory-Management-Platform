import PageHeader from '../components/layout/PageHeader';
import { useAuth } from '../hooks/useAuth';

// Summary counts, recent orders and low-stock items land once the domain pages exist.
export default function DashboardPage() {
  const { user } = useAuth();
  return (
    <>
      <PageHeader title="Dashboard" meta={<span>Signed in as {user?.fullName}</span>} />
    </>
  );
}
