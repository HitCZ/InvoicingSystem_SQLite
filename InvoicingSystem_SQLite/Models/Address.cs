using InvoicingSystem.Logic.Constants;

namespace InvoicingSystem_SQLite.Models
{
    public class Address
    {
        #region Properties

        public int Id { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public string Country { get; set; } = Strings.DEFAULT_COUNTRY;

        #endregion Properties

        #region Overriden Methods
        
        public override string ToString()
        {
            return $"{Street} {BuildingNumber}, {ZipCode} {City}; {Country}";
        }

        #endregion
    }
}
