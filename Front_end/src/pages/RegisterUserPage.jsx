import { useState } from 'react';
import RegisterUserForm from '../components/auth/RegisterUserForm';
import Badge from '../components/common/Badge';
import Button from '../components/common/Button';
import PageHeader from '../components/layout/PageHeader';
import { useToast } from '../hooks/useToast';
import * as authService from '../services/authService';
import { roleLabel } from '../utils/roleHelpers';
import styles from './RegisterUserPage.module.css';

// There is no "list users" endpoint, so this page keeps a log of accounts
// created in this session as confirmation.
export default function RegisterUserPage() {
  const toast = useToast();
  const [created, setCreated] = useState([]);

  async function handleRegister(values) {
    const user = await authService.register(values);
    toast.success(`User ${user.fullName} registered as ${roleLabel(user.role)}`);
    setCreated((list) => [user, ...list]);
  }

  return (
    <>
      <PageHeader
        title="Register user"
        meta={<span>Create an employee account and assign its role. Share the temporary password with them directly.</span>}
      />

      <div className={styles.layout}>
        <RegisterUserForm onSubmit={handleRegister} />

        {created.length > 0 && (
          <section className={styles.created} aria-labelledby="created-heading">
            <h2 id="created-heading" className={styles.createdTitle}>
              Registered this session
            </h2>
            <ul className={styles.list}>
              {created.map((u) => (
                <li key={u.userId} className={styles.item}>
                  <div className={styles.who}>
                    <span className={styles.name}>{u.fullName}</span>
                    <span className={styles.email}>{u.email}</span>
                  </div>
                  <Badge tone="success">{roleLabel(u.role)}</Badge>
                </li>
              ))}
            </ul>
            <Button variant="ghost" size="sm" to="/audit-logs">
              View in audit log
            </Button>
          </section>
        )}
      </div>
    </>
  );
}
