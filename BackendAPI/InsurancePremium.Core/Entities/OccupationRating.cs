namespace InsurancePremium.Core.Entities
{
    public class OccupationRating
    {
        public int Id { get; set; }

        public string RatingName { get; set; } = string.Empty;

        public decimal Factor { get; set; }
    }
}
