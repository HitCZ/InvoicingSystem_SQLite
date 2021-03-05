using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoicing.Models;
using InvoicingSystem_SQLite.DataAccess.SQL;
using InvoicingSystem_SQLite.Logic.Constants;
using Moq;

namespace InvoicingSystemTests.Mocks
{
    public static class MockProvider
    {
        #region Public Methods

        public static Mock<ITypeToTableMappingManager> GetMockedMappingManager()
        {
            var mappingManagerMock = new Mock<ITypeToTableMappingManager>();
            mappingManagerMock.Setup(m => m.GetTableNameByType(typeof(Address))).Returns(Constants.ADDRESS_TABLE_NAME);
            mappingManagerMock.Setup(m => m.GetTableNameByType(typeof(BankInformation))).Returns(Constants.BANK_INFORMATION_TABLE_NAME);
            mappingManagerMock.Setup(m => m.GetTableNameByType(typeof(Contractor))).Returns(Constants.CONTRACTOR_TABLE_NAME);
            mappingManagerMock.Setup(m => m.GetTableNameByType(typeof(Customer))).Returns(Constants.CUSTOMER_TABLE_NAME);
            mappingManagerMock.Setup(m => m.GetTableNameByType(typeof(Invoice))).Returns(Constants.INVOICE_TABLE_NAME);

            return mappingManagerMock;
        }

        #endregion Public Methods
    }
}
