namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    internal class InsertInformation
    {
        public int Index { get; }
        public string ColumnName { get; }
        public object Value { get; }

        public InsertInformation(int index, string columnName, object value)
        {
            Index = index;
            ColumnName = columnName;
            Value = value;
        }
    }
}
