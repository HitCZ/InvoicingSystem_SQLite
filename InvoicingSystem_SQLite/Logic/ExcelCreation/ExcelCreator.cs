using System.Collections.Generic;
using System.IO;
using Invoicing.Models;
using InvoicingSystem_SQLite.Logic.Constants;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InvoicingSystem_SQLite.Logic.ExcelCreation
{
    class ExcelCreator
    {
        #region Fields

        private const string WORKSHEET_NAME = Strings.WORKSHEET_NAME;
        private const string PATH = "test.xlsx";
        //private const string FONT_NAME = Strings.FONT;
        //private const float FONT_SIZE = 11f;
        /// <summary>
        ///  ((hint, position) value)
        /// </summary>
        private readonly Dictionary<KeyValuePair<string, string>, string> coordsAndValues;
        private Invoice invoice;
        private Contractor contractor;
        private Customer customer;

        #endregion Fields

        #region Constructor

        public ExcelCreator()
        {
            coordsAndValues
                = new Dictionary<KeyValuePair<string, string>, string>();
        }

        #endregion Constructor

        #region Public Methods

        // ReSharper disable once ParameterHidesMember
        public void CreateExcel(Invoice invoice)
        {
            this.invoice = invoice;
            contractor = invoice.Contractor;
            customer = invoice.Customer;
            InitDictionary();

            var excelFile = new FileInfo(PATH);
            using (var excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add(WORKSHEET_NAME);

                var worksheet = excel.Workbook.Worksheets[WORKSHEET_NAME];
                FormatWorksheet(worksheet);
                AddCells(worksheet);
                excel.SaveAs(excelFile);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initializes values for the entire excel document.
        /// </summary>
        private void InitDictionary()
        {
            FillHeaderInfo();
            FillContractorInfo();
            FillCustomerInfo();
            FillinvoicesInfo();
            FillJobDescriptionInfo();
            FillPriceInfo();
            FillSignatureInfo();
        }

        /// <summary>
        /// Adds header info into the dictionary.
        /// </summary>
        private void FillHeaderInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>("invoiceCaption", "G3"), Strings.INVOICE_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>("invoiceNum", "H3:I3"),
                $"{Strings.INVOICE_NUMBER_CAPTION}: {invoice.InvoiceNumber.ToString()}");
        }

        /// <summary>
        /// Adds contractor info into the dictionary.
        /// </summary>
        private void FillContractorInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorCaption", "C5"), Strings.CONTRACTOR_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorNameCaption", "C7"), Strings.CONTRACTOR_NAME_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorName", "D7"), contractor.ToString());
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorStreetCaption", "C8"), Strings.CONTRACTOR_STREET_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorStreet", "D8"),
                $"{contractor.Address.Street} {contractor.Address.BuildingNumber}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorZipcodeCaption", "C9"), Strings.CONTRACTOR_ZIPCODE_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorZipcode", "D9"), $"{contractor.Address.ZipCode} {contractor.Address.City}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "contractorIN", "C11"), $"{Strings.IN_CAPTION}: {contractor.IdentificationNumber}");

            // Is contractor a VAT payer??
            var isVatPayerString
                = contractor.IsVatPayer ? Strings.CONTRACTOR_VAT_PAYER : Strings.CONTRACTOR_NOT_A_VAT_PAYER;

            coordsAndValues.Add(new KeyValuePair<string, string>("contractorVATPayer", "C12"), isVatPayerString);
            coordsAndValues.Add(new KeyValuePair<string, string>("contractorCityOfEvidence", "C14"),
                $"{Strings.CONTRACTOR_CITY_OF_EVIDENCE_CAPTION} {contractor.CityOfEvidence}.");
        }

        /// <summary>
        /// Adds customer info to the dictionary.
        /// </summary>
        private void FillCustomerInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "customerCaption", "G5"), Strings.CUSTOMER_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "customerCorporationName", "G7"), customer.CorporationName);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "customerStreet", "G8"),
                $"{customer.Address.Street} {customer.Address.BuildingNumber}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "customerZipcode", "G9"),
                $"{customer.Address.ZipCode} {customer.Address.City}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "customerIN", "G11"), $"{Strings.IN_CAPTION}: {customer.IdentificationNumber},");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                // ReSharper disable once StringLiteralTypo
                "customerVATIN", "H11"), $"{Strings.CUSTOMER_VATIN_CAPTION}: {customer.VATIN}");
        }

        /// <summary>
        /// Adds payment conditions info into the dictionary.
        /// </summary>
        private void FillinvoicesInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionCaption", "C17"), Strings.CONDITION_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionPaymentMethodCaption", "C19"), Strings.CONDITION_PAYMENT_METHOD_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionPaymentMethod", "D19"),
                $"{invoice.PaymentMethod.ToString()}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionBankConnectionCaption", "C20"), Strings.CONDITION_BANK_CONNECTION_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionBankConnection", "D20"),
                $"{invoice.BankConnection}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionAccountNumberCaption", "C21"), Strings.CONDITION_ACCOUNT_NUMBER_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionAccountNumber", "D21"),
                $"{invoice.AccountNumber}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionVariableSymbolCaption", "C22"), Strings.CONDITION_VARIABLE_SYMBOL_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionVariableSymbol", "D22"),
                $"{invoice.VariableSymbol}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionDateOfIssueCaption", "G19"), Strings.CONDITION_DATE_OF_ISSUE_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionDateOfIssue", "H19"),
                $"{invoice.DateOfIssue:dd.MM.yyy}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionDueDateCaption", "G21"), Strings.CONDITION_DUE_DATE_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "conditionDueDate", "H21"), $"{invoice.DueDate:dd.MM.yyy}");
        }

        /// <summary>
        /// Adds job description to the dictionary.
        /// </summary>
        private void FillJobDescriptionInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "descriptionCaption", "C25"), Strings.JOB_DESCRIPTION_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "description", "C26:H26"), $"{invoice.JobDescription}");
        }

        /// <summary>
        /// Adds price info to the dictionary.
        /// </summary>
        private void FillPriceInfo()
        {
            //coordsAndValues.Add(new KeyValuePair<string, string>(
            //   "priceCaption", "G25"), $"Cena: {invoice.Price}");
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "totalCaption", "G30"), Strings.PRICE_TOTAL_CAPTION);
            coordsAndValues.Add(new KeyValuePair<string, string>(
                "total", "H30"), $"{invoice.Price} {Strings.PRICE_TOTAL_CURRENCY}");
        }

        /// <summary>
        /// AddsSignature info to the dictionary.
        /// </summary>
        private void FillSignatureInfo()
        {
            coordsAndValues.Add(new KeyValuePair<string, string>("signatureCaption", "G39"),
                $"{Strings.SIGNATURE_CAPTION} {contractor}");

            coordsAndValues.Add(new KeyValuePair<string, string>("signature", "G40"), Strings.SIGNATURE);
        }

        private void FormatWorksheet(ExcelWorksheet worksheet)
        {
            worksheet.Cells["B17"].AutoFitColumns();
            const ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;

            // left border
            worksheet.Cells["B4:B47"].Style.Border.Left.Style = borderStyle;
            // top border
            worksheet.Cells["B4:H4"].Style.Border.Top.Style = borderStyle;
            // header border
            worksheet.Cells["F3:H3"].Style.Border.BorderAround(borderStyle);
            // right border
            worksheet.Cells["H3:H47"].Style.Border.Right.Style = borderStyle;
            // bottom border
            worksheet.Cells["B47:H47"].Style.Border.Bottom.Style = borderStyle;
            // first fourth border
            worksheet.Cells["B15:H15"].Style.Border.Bottom.Style = borderStyle;
            // second fourth border
            worksheet.Cells["B23:H23"].Style.Border.Bottom.Style = borderStyle;
            // third fourth border
            worksheet.Cells["B27:H27"].Style.Border.Bottom.Style = borderStyle;
        }

        //private void MoveContent(List<string> cellNames, Direction direction, int offset)
        //{
        //    var list = FilterContent(cellNames);

        //}

        //private IEnumerable<KeyValuePair<string, string>> FilterContent(List<string> cellNames)
        //{
        //    var list = new List<KeyValuePair<string, string>>();

        //    foreach (var name in cellNames)
        //    {
        //        var record = coordsAndValues.FirstOrDefault(
        //            c => c.Key.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Key;
        //        list.Add(new KeyValuePair<string, string>(record.Key, record.Value));
        //    }

        //    return list;
        //}

        private void AddCells(ExcelWorksheet worksheet)
        {
            foreach (var item in coordsAndValues)
            {
                var coordinate = item.Key.Value;
                var value = item.Value;

                if (coordinate.Contains(":"))
                    worksheet.Cells[coordinate].Merge = true;
                worksheet.Cells[coordinate].Value = value;
            }
            //worksheet.Cells.AutoFitColumns();
        }

        #endregion Methods
    }
}
