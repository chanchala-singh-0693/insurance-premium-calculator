import { useState, useEffect } from 'react';
import { fetchHistory } from '../services/insuranceApi';


export default function CalculationHistory({ refreshKey }) {
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error,   setError]   = useState('');

  useEffect(() => {
    setLoading(true);
    fetchHistory()
      .then(setHistory)
      .catch(() => setError('Could not load history from database.'))
      .finally(() => setLoading(false));
  }, [refreshKey]);   // re-fetch when a new calculation is saved

  const fmt = (n) => new Intl.NumberFormat('en-In', { style: 'currency', currency: 'INR' }).format(n);
  const fmtDate = (d) => new Date(d).toLocaleString('en-In');

  if (loading) return <p className="muted">Loading history from database…</p>;
  if (error)   return <div className="alert-error">{error}</div>;
  if (history.length === 0) return (
    <div className="empty-state">
      <p>No calculations yet.</p>
      <p className="muted">Submit a form to see records here.</p>
    </div>
  );

  return (
    <div className="history-wrap">
      <p className="muted" style={{ marginBottom: '16px' }}>
        {history.length} record{history.length !== 1 ? 's' : ''} stored in SQL Server — newest first
      </p>
      <div className="table-scroll">
        <table className="history-table">
          <thead>
            <tr>
              <th>#</th>
              <th>Member</th>
              <th>Occupation</th>
              <th>Death Cover</th>
              <th>Monthly Premium</th>
              <th>Calculated At</th>
            </tr>
          </thead>
          <tbody>
            {history.map(h => (
              <tr key={h.id}>
                <td>{h.id}</td>
                <td>{h.memberName}</td>
                <td>{h.occupationName}</td>
                <td>{fmt(h.deathSumInsured)}</td>
                <td className="premium-cell">{fmt(h.monthlyPremium)}</td>
                <td className="date-cell">{fmtDate(h.calculatedAt)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
