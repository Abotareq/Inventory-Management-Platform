import { AuthProvider } from './contexts/AuthContext';
import { ToastProvider } from './contexts/ToastContext';
import ToastStack from './components/common/ToastStack';

// Routing and layout land in the next stage; this keeps the foundation buildable.
export default function App() {
  return (
    <AuthProvider>
      <ToastProvider>
        <main style={{ padding: 24 }}>
          <h1>Inventory</h1>
          <p>App shell coming next.</p>
        </main>
        <ToastStack />
      </ToastProvider>
    </AuthProvider>
  );
}
