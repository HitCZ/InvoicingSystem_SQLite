//using Invoicing.Models;
//using InvoicingSystem_SQLite.DataAccess.Interfaces;
//using InvoicingSystem_SQLite.DataAccess.QueryExecution;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Composition;

//namespace InvoicingSystem_SQLite.DataAccess.SQL
//{
//    public class AddressSqlProvider : SqlDataProvider<Address>, IAddressProvider
//    {
//        #region Constructor

//        [ImportingConstructor]
//        public AddressSqlProvider(IQueryExecutor queryExecutor) : base(queryExecutor)
//        {
//        }

//        #endregion Constructor

//        #region IAddressProvider

//        public IEnumerable<Address> GetAddressesByStreet(string street)
//        {
//            var query = $"SELECT * FROM {tableName} WHERE Street LIKE {street}";
//            var result = queryExecutor.ExecuteQueryWitMultipleResults<Address>(query);

//            return result;
//        }

//        public IEnumerable<Address> GetAddressesByBuildingNumber(string buildingNumber)
//        {
//            var query = $"SELECT * FROM {tableName} WHERE BuildingNumber LIKE {buildingNumber}";
//            var result = queryExecutor.ExecuteQueryWitMultipleResults<Address>(query);

//            return result;
//        }

//        public IEnumerable<Address> GetAddressesByCity(string city)
//        {
//            var query = $"SELECT * FROM {tableName} WHERE City LIKE {city}";
//            var result = queryExecutor.ExecuteQueryWitMultipleResults<Address>(query);

//            return result;
//        }

//        public IEnumerable<Address> GetAddressesByZipCode(string zipCode)
//        {
//            var query = $"SELECT * FROM {tableName} WHERE BuildingNumber LIKE {zipCode}";
//            var result = queryExecutor.ExecuteQueryWitMultipleResults<Address>(query);

//            return result;
//        }

//        #endregion IAddressProvider

//        #region SqlDataProvider

//        public override int CreateOrUpdate(Address item)
//        {
//            var query = $"INSERT INTO {tableName} (Id, Street, BuildingNumber, City, Country, ZipCode) " +
//                        $"VALUES ({item.Id}, {item.Street}, {item.BuildingNumber}, {item.City}, {item.Country}, {item.ZipCode}";
//            var feedBack = queryExecutor.ExecuteQueryWithFeedback(query);

//            return feedBack;
//        }

//        public override int CreateOrUpdate(IEnumerable<Address> items)
//        {
//            throw new NotImplementedException();
//        }

//        public override int Delete(Address item)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion SqlDataProvider
//    }
//}
