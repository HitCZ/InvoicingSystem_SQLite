namespace InvoicingSystem_SQLite.Models
{
    public class Contractor : Enterprenour
    {
        #region Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CityOfEvidence { get; set; }
        // Value added tax payer
        public bool IsVatPayer { get; set; }

        #endregion Properties

        #region Overriden Methods

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        #endregion Overriden Methods
    }
}
