namespace InvoicingSystem_SQLite.DataAccess.SQL
{
    public class PropertyInformation
    {
        public int Index { get; }
        public string ColumnName { get; }
        public object Value { get; }

        public PropertyInformation(int index, string columnName, object value)
        {
            Index = index;
            ColumnName = columnName;
            Value = value;
        }
    }
}
