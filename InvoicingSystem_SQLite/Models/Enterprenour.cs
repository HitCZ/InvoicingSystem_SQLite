namespace InvoicingSystem_SQLite.Models
{
    // ReSharper disable once IdentifierTypo
    public abstract class Enterprenour
    {
        #region Properties

        public int Id { get; set; }
        public int IdAddress { get; set; }
        public virtual Address Address { get; set; }
        // Identification number - 'IČO'
        // ReSharper disable once InconsistentNaming
        public int IN { get; set; }
        // ReSharper disable once CommentTypo
        // Value added tax identification number - 'DIČO'
        // ReSharper disable once IdentifierTypo
        // ReSharper disable once InconsistentNaming
        public string VATIN { get; set; }

        #endregion Properties
    }
}
