import { useState, useEffect } from 'react';
import { fetchOccupations, calculatePremium } from '../services/insuranceApi';
import PremiumResult from './PremiumResult';

export default function PremiumForm() {
  const [formData, setFormData] = useState({
    name: '',
    ageNextBirthday: '',
    dateOfBirth: '',
    occupationId: '',
    deathSumInsured: '',
  });

  const [occupations, setOccupations] = useState([]);
  const [result, setResult] = useState(null);
  const [errors, setErrors] = useState({});
  const [apiError, setApiError] = useState('');
  const [loading, setLoading] = useState(false);
  const [loadingOccupations, setLoadingOccupations] = useState(true);

  useEffect(() => {
    fetchOccupations()
      .then(data => setOccupations(data))
      .catch(() => setApiError('Failed to load occupations. Please refresh the page.'))
      .finally(() => setLoadingOccupations(false));
  }, []);

  useEffect(() => {
    if (
      formData.occupationId &&
      formData.name &&
      formData.ageNextBirthday &&
      formData.dateOfBirth &&
      formData.deathSumInsured
    ) {
      handleCalculate();
    }
  }, [formData.occupationId]);

  const validate = () => {
    const newErrors = {};

    if (!formData.name.trim()) newErrors.name = 'Name is required.';
    else if (formData.name.trim().length < 2) newErrors.name = 'Name must be at least 2 characters.';

    const age = parseInt(formData.ageNextBirthday, 10);
    if (!formData.ageNextBirthday) newErrors.ageNextBirthday = 'Age Next Birthday is required.';
    else if (isNaN(age) || age < 1 || age > 120) newErrors.ageNextBirthday = 'Age must be between 1 and 120.';

    if (!formData.dateOfBirth) {
      newErrors.dateOfBirth = 'Date of Birth is required.';
    } else if (!/^(0[1-9]|1[0-2])\/\d{4}$/.test(formData.dateOfBirth)) {
      newErrors.dateOfBirth = 'Date of Birth must be in mm/YYYY format (e.g., 06/1990).';
    }

    if (!formData.occupationId) newErrors.occupationId = 'Please select an occupation.';

    const cover = parseFloat(formData.deathSumInsured);
    if (!formData.deathSumInsured) newErrors.deathSumInsured = 'Death Sum Insured is required.';
    else if (isNaN(cover) || cover <= 0) newErrors.deathSumInsured = 'Death Sum Insured must be greater than zero.';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    // Clear field-level error on change
    if (errors[name]) setErrors(prev => ({ ...prev, [name]: '' }));
    setApiError('');
  };

  const handleCalculate = async (e) => {
    if (e) e.preventDefault();
    if (!validate()) return;

    setLoading(true);
    setApiError('');
    setResult(null);

    try {
      const data = await calculatePremium({
        name: formData.name.trim(),
        ageNextBirthday: parseInt(formData.ageNextBirthday, 10),
        dateOfBirth: formData.dateOfBirth,
        occupationId: parseInt(formData.occupationId, 10),
        deathSumInsured: parseFloat(formData.deathSumInsured),
      });
      setResult(data);
    } catch (err) {
      setApiError(err.message || 'An unexpected error occurred. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-container">
      <h1 className="form-title">Insurance Premium Calculator</h1>
      <p className="form-subtitle">Fill in all fields below to calculate your monthly premium.</p>

      {apiError && <div className="alert alert-error">{apiError}</div>}

      <form onSubmit={handleCalculate} noValidate>

        {/* Name */}
        <div className="form-group">
          <label htmlFor="name">Full Name <span className="required">*</span></label>
          <input
            id="name"
            name="name"
            type="text"
            value={formData.name}
            onChange={handleChange}
            placeholder="enter your full name"
            className={errors.name ? 'input-error' : ''}
          />
          {errors.name && <span className="error-msg">{errors.name}</span>}
        </div>

        {/* Age Next Birthday */}
        <div className="form-group">
          <label htmlFor="ageNextBirthday">Age Next Birthday <span className="required">*</span></label>
          <input
            id="ageNextBirthday"
            name="ageNextBirthday"
            type="number"
            min="1"
            max="120"
            value={formData.ageNextBirthday}
            onChange={handleChange}
            placeholder="enter your age at next birthday"
            className={errors.ageNextBirthday ? 'input-error' : ''}
          />
          {errors.ageNextBirthday && <span className="error-msg">{errors.ageNextBirthday}</span>}
        </div>

        
        <div className="form-group">
          <label htmlFor="dateOfBirth">Date of Birth (mm/YYYY) <span className="required">*</span></label>
          <input
            id="dateOfBirth"
            name="dateOfBirth"
            type="text"
            value={formData.dateOfBirth}
            onChange={handleChange}
            placeholder="06/1990"
            className={errors.dateOfBirth ? 'input-error' : ''}
          />
          {errors.dateOfBirth && <span className="error-msg">{errors.dateOfBirth}</span>}
        </div>

        
        <div className="form-group">
          <label htmlFor="occupationId">Usual Occupation <span className="required">*</span></label>
          {loadingOccupations ? (
            <p className="loading-text">Loading occupations…</p>
          ) : (
            <select
              id="occupationId"
              name="occupationId"
              value={formData.occupationId}
              onChange={handleChange}
              className={errors.occupationId ? 'input-error' : ''}
            >
              <option value="">— Select an occupation —</option>
              {occupations.map(occ => (
                <option key={occ.id} value={occ.id}>
                  {occ.name} ({occ.rating})
                </option>
              ))}
            </select>
          )}
          {errors.occupationId && <span className="error-msg">{errors.occupationId}</span>}
        </div>

        
        <div className="form-group">
          <label htmlFor="deathSumInsured">Death Sum Insured <span className="required">*</span></label>
          <input
            id="deathSumInsured"
            name="deathSumInsured"
            type="number"
            min="1"
            step="1000"
            value={formData.deathSumInsured}
            onChange={handleChange}
            placeholder="enter the death sum insured"
            className={errors.deathSumInsured ? 'input-error' : ''}
          />
          {errors.deathSumInsured && <span className="error-msg">{errors.deathSumInsured}</span>}
        </div>

        <button type="submit" className="btn-primary" disabled={loading}>
          {loading ? 'Calculating…' : 'Calculate Premium'}
        </button>
      </form>
      {result && <PremiumResult result={result} />}
    </div>
  );
}
