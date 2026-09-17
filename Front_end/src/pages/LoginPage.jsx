import { Navigate, useLocation, useNavigate } from 'react-router-dom';
import LoginForm from '../components/auth/LoginForm';
import { APP_NAME } from '../config/config';
import { useAuth } from '../hooks/useAuth';
import styles from './LoginPage.module.css';

export default function LoginPage() {
  const { isAuthenticated, sessionExpired, login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname ?? '/';

  if (isAuthenticated) {
    return <Navigate to={from} replace />;
  }

  async function handleLogin(credentials) {
    await login(credentials);
    navigate(from, { replace: true });
  }

  return (
    <main className={styles.page}>
      <div className={styles.card}>
        <div className={styles.brand}>
          <span className={styles.mark} aria-hidden="true" />
          <span className={styles.name}>{APP_NAME}</span>
        </div>
        <h1 className={styles.title}>Sign in</h1>
        <p className={styles.subtitle}>Use your employee account to continue.</p>
        <LoginForm
          onLogin={handleLogin}
          notice={sessionExpired ? 'Your session ended. Sign in again to continue.' : null}
        />
      </div>
    </main>
  );
}
