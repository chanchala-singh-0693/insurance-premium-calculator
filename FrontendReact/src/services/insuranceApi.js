const BASE_URL = 'https://localhost:55454/api';

/**
 * Fetches all available occupations from the backend.
 * @returns {Promise<Array<{id: number, name: string, rating: string}>>}
 */
export async function fetchOccupations() {
  const response = await fetch(`${BASE_URL}/occupation`);
  if (!response.ok) {
    throw new Error(`Failed to fetch occupations: ${response.statusText}`);
  }
  return response.json();
}

/**
 * Submits a premium calculation request to the backend.
 *
 * @param {{
 *   name: string,
 *   ageNextBirthday: number,
 *   dateOfBirth: string,
 *   occupationId: number,
 *   deathSumInsured: number
 * }} requestData
 *
 * @returns {Promise<{
 *   memberName: string,
 *   occupationName: string,
 *   occupationRating: string,
 *   occupationFactor: number,
 *   monthlyPremium: number,
 *   calculatedAt: string
 * }>}
 */
export async function calculatePremium(requestData) {
  const response = await fetch(`${BASE_URL}/premiumcalculation/calculate`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(requestData),
  });

  if (!response.ok) {
    // Try to extract the server's error message
    const error = await response.json().catch(() => ({ message: response.statusText }));
    throw new Error(error.message || 'Premium calculation failed.');
  }

  return response.json();
}
export async function fetchHistory() {
  const res = await fetch(`${BASE_URL}/premiumcalculation/history`);
  if (!res.ok) throw new Error('Failed to load history.');
  return res.json();
}