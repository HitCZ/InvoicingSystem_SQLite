namespace InvoicingSystem.Logic.Constants
{
    public static class Strings
    {
        #region Address

        public const string DEFAULT_COUNTRY = "Česká republika";

        #endregion Address

        #region Payment Condition

        public const string PAYMENT_METHOD_TRANSFER = "Převodem";
        public const string PAYMENT_METHOD_CASH = "Hotově";

        #endregion Payment Condition

        #region Invoice

        public const string WORKSHEET_NAME = "Faktura";
        public const string FONT = "Arial CE";
        public const string INVOICE_CAPTION = "Faktura";
        public const string INVOICE_NUMBER_CAPTION = "číslo";

        public const string IN_CAPTION = "IČ";

        // Contractor section
        public const string CONTRACTOR_CAPTION = "Dodavatel";
        public const string CONTRACTOR_NAME_CAPTION = "Vaše jméno";
        public const string CONTRACTOR_STREET_CAPTION = "Ulice, č.p.";
        public const string CONTRACTOR_ZIPCODE_CAPTION = "PSČ, město";
        public const string CONTRACTOR_VAT_PAYER = "Plátce DPH";
        public const string CONTRACTOR_NOT_A_VAT_PAYER = "Neplátce DPH";
        public const string CONTRACTOR_CITY_OF_EVIDENCE_CAPTION = "Podnikatel zapsán v živ. rejstříku MŮ";

        // Customer section
        public const string CUSTOMER_CAPTION = "Odběratel";
        // ReSharper disable once IdentifierTypo
        public const string CUSTOMER_VATIN_CAPTION = "DIČ";

        // Payment conditions section
        public const string CONDITION_CAPTION = "Platební podmínky";
        public const string CONDITION_PAYMENT_METHOD_CAPTION = "Forma úhrady: ";
        public const string CONDITION_BANK_CONNECTION_CAPTION = "Bankovní spojení: ";
        public const string CONDITION_ACCOUNT_NUMBER_CAPTION = "Číslo účtu: ";
        public const string CONDITION_VARIABLE_SYMBOL_CAPTION = "Variabilní symbol: ";
        public const string CONDITION_DATE_OF_ISSUE_CAPTION = "Datum vystavení: ";
        public const string CONDITION_DUE_DATE_CAPTION = "Datum splatnost: ";

        // Job description
        public const string JOB_DESCRIPTION_CAPTION = "Fakturujeme vám: ";

        // Price info
        public const string PRICE_TOTAL_CAPTION = "Celkem: ";
        public const string PRICE_TOTAL_CURRENCY = "Kč";

        // Signature info
        public const string SIGNATURE_CAPTION = "Vystavil: ";
        public const string SIGNATURE = "Podpis: ";

        #endregion Invoice
    }
}
