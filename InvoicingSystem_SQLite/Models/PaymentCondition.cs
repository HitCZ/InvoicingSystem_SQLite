using System;
using InvoicingSystem.Logic.Constants;
using InvoicingSystem.Logic.Enumerations;
using InvoicingSystem.Logic.Extensions;

namespace InvoicingSystem_SQLite.Models
{
    public class PaymentCondition
    {
        #region Properties

        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string PaymentMethodString
        {
            get => PaymentMethod is PaymentMethod.BankTransfer
                ? Strings.PAYMENT_METHOD_TRANSFER : Strings.PAYMENT_METHOD_CASH;
            set => PaymentMethod = value.ParseEnum<PaymentMethod>();
        }

        public string BankConnection { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string VariableSymbol { get; set; } = string.Empty;

        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }

        #endregion Properties

        #region Overriden Methods

        public override string ToString()
        {
            return PaymentMethodString;
        }

        #endregion
    }
}
