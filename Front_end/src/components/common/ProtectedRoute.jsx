import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { hasRole } from '../../utils/roleHelpers';

// Wraps routes. Unauthenticated users go to /login (remembering where they
// were). Authenticated users lacking one of `roles` go to /forbidden.
export default function ProtectedRoute({ roles, children }) {
  const { isAuthenticated, user } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  if (roles && roles.length > 0 && !hasRole(user, ...roles)) {
    return <Navigate to="/forbidden" replace />;
  }

  return children ?? <Outlet />;
}
