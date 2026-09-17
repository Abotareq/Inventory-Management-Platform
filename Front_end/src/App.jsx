import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import ProtectedRoute from './components/common/ProtectedRoute';
import ToastStack from './components/common/ToastStack';
import AppLayout from './components/layout/AppLayout';
import { AuthProvider } from './contexts/AuthContext';
import { ToastProvider } from './contexts/ToastContext';
import DashboardPage from './pages/DashboardPage';
import ForbiddenPage from './pages/ForbiddenPage';
import LoginPage from './pages/LoginPage';
import NotFoundPage from './pages/NotFoundPage';

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ToastProvider>
          <Routes>
            <Route path="/login" element={<LoginPage />} />

            <Route element={<ProtectedRoute />}>
              <Route element={<AppLayout />}>
                <Route index element={<DashboardPage />} />
                <Route path="/forbidden" element={<ForbiddenPage />} />
                <Route path="*" element={<NotFoundPage />} />
              </Route>
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
          <ToastStack />
        </ToastProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
