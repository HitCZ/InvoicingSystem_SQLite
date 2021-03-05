using Invoicing.Enumerations;
using Invoicing.Models;
using System;

namespace InvoicingSystem_SQLite.Logic.TestData
{
    public static class InvoiceMocksProvider
    {
        public static Invoice GetInvoice()
        {
            var invoice = new Invoice
            {
                BankInformation = new BankInformation
                {
                    AccountNumber = "0123456789/0800",
                    BankConnection = "0123456789",
                    VariableSymbol = null
                },
                Contractor = new Contractor
                {
                    Address = new Address
                    {
                        BuildingNumber = "01",
                        City = "Praha",
                        Street = "Pařížská",
                        ZipCode = 25136,
                        Country = "Česká republika"
                    },
                    CityOfEvidence = "Kladno",
                    FirstName = "Josef",
                    LastName = "Novák",
                    IdentificationNumber = 100000001,
                    IsVatPayer = false,
                    VATIN = "CZ100000001"
                },
                Currency = Currency.CZK,
                Customer = new Customer
                {
                    VATIN = "CZ100000002",
                    IdentificationNumber = 100000002,
                    Address = new Address
                    {
                        BuildingNumber = "02",
                        City = "Praha",
                        Street = "Pařížská",
                        ZipCode = 25136,
                        Country = "Česká republika"
                    },
                    CorporationName = "Blabla"
                },
                DateOfIssue = DateTime.Today,
                DueDate = DateTime.Today.AddDays(2),
                InvoiceNumber = 202000001,
                JobDescription = "Fakturujeme Vám práce za uplynulý měsíc.",
                PaymentMethod = PaymentMethod.BankTransfer,
                Price = 30000
            };

            return invoice;
        }
    }
}
