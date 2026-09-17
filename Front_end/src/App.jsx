import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import ProtectedRoute from './components/common/ProtectedRoute';
import ToastStack from './components/common/ToastStack';
import AppLayout from './components/layout/AppLayout';
import { ROLES } from './config/constants';
import { AuthProvider } from './contexts/AuthContext';
import { ToastProvider } from './contexts/ToastContext';
import AuditLogsPage from './pages/AuditLogsPage';
import CategoriesPage from './pages/CategoriesPage';
import CreateOrderPage from './pages/CreateOrderPage';
import DashboardPage from './pages/DashboardPage';
import ForbiddenPage from './pages/ForbiddenPage';
import LoginPage from './pages/LoginPage';
import NotFoundPage from './pages/NotFoundPage';
import OrderDetailPage from './pages/OrderDetailPage';
import OrdersPage from './pages/OrdersPage';
import ProductsPage from './pages/ProductsPage';
import RegisterUserPage from './pages/RegisterUserPage';
import StockDetailPage from './pages/StockDetailPage';
import StockPage from './pages/StockPage';
import WarehousesPage from './pages/WarehousesPage';

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
                <Route path="/orders" element={<OrdersPage />} />
                <Route path="/orders/:id" element={<OrderDetailPage />} />
                <Route path="/products" element={<ProductsPage />} />
                <Route path="/warehouses" element={<WarehousesPage />} />
                <Route path="/stock" element={<StockPage />} />
                <Route path="/stock/:stockId" element={<StockDetailPage />} />

                <Route element={<ProtectedRoute roles={[ROLES.SALES_AGENT]} />}>
                  <Route path="/orders/new" element={<CreateOrderPage />} />
                </Route>

                <Route element={<ProtectedRoute roles={[ROLES.ADMINISTRATOR]} />}>
                  <Route path="/categories" element={<CategoriesPage />} />
                  <Route path="/audit-logs" element={<AuditLogsPage />} />
                  <Route path="/users/new" element={<RegisterUserPage />} />
                </Route>

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
