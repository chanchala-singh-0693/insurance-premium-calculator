import { useState } from 'react';
import PremiumForm from './components/PremiumForm';
import CalculationHistory from './components/CalculationHistory';
import './App.css';



export default function App() {
  const [activeTab,   setActiveTab]   = useState('calculate');
  const [refreshKey,  setRefreshKey]  = useState(0);
  const handleNewResult = () => setRefreshKey(k => k + 1);
  return (
    <div className="app-wrapper">
      <div className="app-card">

        {/* Header */}
        <div className="app-header">
          <h1>Insurance Premium Calculator</h1>
        </div>

        {/* Tabs */}
        <div className="tabs">
          <button
            className={activeTab === 'calculate' ? 'btn-tab tab active' : 'btn-tab tab'}
            onClick={() => setActiveTab('calculate')}>
            Calculate
          </button>
          <button
            className={activeTab === 'history' ? 'btn-tab tab active' : 'btn-tab tab'}
            onClick={() => { setActiveTab('history'); setRefreshKey(k => k + 1); }}>
            History
          </button>
        </div>

        {/* Tab Content */}
        <div className="tab-content">
          {activeTab === 'calculate'
            ? <PremiumForm onNewResult={handleNewResult} />
            : <CalculationHistory refreshKey={refreshKey} />
          }
        </div>

      </div>
    </div>
  );
}
