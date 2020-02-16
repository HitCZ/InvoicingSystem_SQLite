namespace InvoicingSystem_SQLite.Logic
{
    public struct ValueDescription
    {
        public string Description { get; }
        public object Value { get; }

        public ValueDescription(object value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
