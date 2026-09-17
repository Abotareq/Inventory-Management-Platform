import styles from './Tabs.module.css';

// tabs: [{ id, label, count? }]; controlled by `value` / `onChange`.
export default function Tabs({ tabs, value, onChange, label = 'Sections' }) {
  return (
    <div className={styles.tabs} role="tablist" aria-label={label}>
      {tabs.map((tab) => {
        const selected = tab.id === value;
        return (
          <button
            key={tab.id}
            type="button"
            role="tab"
            id={`tab-${tab.id}`}
            aria-selected={selected}
            aria-controls={`panel-${tab.id}`}
            tabIndex={selected ? 0 : -1}
            className={[styles.tab, selected && styles.selected].filter(Boolean).join(' ')}
            onClick={() => onChange(tab.id)}
            onKeyDown={(e) => {
              if (e.key !== 'ArrowRight' && e.key !== 'ArrowLeft') return;
              e.preventDefault();
              const idx = tabs.findIndex((t) => t.id === value);
              const nextIdx =
                e.key === 'ArrowRight'
                  ? (idx + 1) % tabs.length
                  : (idx - 1 + tabs.length) % tabs.length;
              onChange(tabs[nextIdx].id);
              document.getElementById(`tab-${tabs[nextIdx].id}`)?.focus();
            }}
          >
            {tab.label}
            {tab.count !== undefined && <span className={[styles.count, 'num'].join(' ')}>{tab.count}</span>}
          </button>
        );
      })}
    </div>
  );
}

export function TabPanel({ id, active, children }) {
  if (!active) return null;
  return (
    <div role="tabpanel" id={`panel-${id}`} aria-labelledby={`tab-${id}`} tabIndex={0}>
      {children}
    </div>
  );
}
