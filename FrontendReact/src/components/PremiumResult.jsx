/**
 *
 * @param {{ result: {
 *   memberName: string,
 *   occupationName: string,
 *   occupationRating: string,
 *   occupationFactor: number,
 *   monthlyPremium: number,
 *   calculatedAt: string
 * }}} props
 */
export default function PremiumResult({ result }) {
  const formattedPremium = new Intl.NumberFormat('en-IN', {
    style: 'currency',
    currency: 'INR',
  }).format(result.monthlyPremium);

  const formattedDate = new Date(result.calculatedAt).toLocaleString('en-IN');

  return (
    <div className="result-card">
      <h2 className="result-title">Monthly Premium Result</h2>

      <div className="result-premium">{formattedPremium}<span className="result-per-month"> / month</span></div>

      <table className="result-table">
        <tbody>
          <tr>
            <th>Member Name</th>
            <td>{result.memberName}</td>
          </tr>
          <tr>
            <th>Occupation</th>
            <td>{result.occupationName}</td>
          </tr>
          <tr>
            <th>Occupation Rating</th>
            <td>{result.occupationRating}</td>
          </tr>
          <tr>
            <th>Rating Factor</th>
            <td>{result.occupationFactor}</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}
