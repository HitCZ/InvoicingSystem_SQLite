namespace InvoicingSystem_SQLite.Models
{
    public class Invoice
    {
        #region Properties

        public int Id { get; set; }
        public int IdContractor { get; set; }
        public int IdCustomer { get; set; }
        public int IdPaymentCondition { get; set; }
        public int InvoiceNumber { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentCondition PaymentCondition { get; set; }
        public string JobDescription { get; set; }
        public decimal Price { get; set; }

        #endregion Properties

        #region Overriden Methods

        public override string ToString()
        {
            return InvoiceNumber.ToString();
        }

        #endregion Overriden Methods
    }
}
