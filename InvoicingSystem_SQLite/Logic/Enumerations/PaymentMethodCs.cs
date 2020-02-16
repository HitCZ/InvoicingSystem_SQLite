using System.ComponentModel;

namespace InvoicingSystem_SQLite.Logic.Enumerations
{
    public enum PaymentMethodCs
    {
        [Description("Převodem")]
        BankTransfer,
        [Description("Hotově")]
        Cash
    }
}
