using System;

namespace InvoicingSystem_SQLite.Logic
{
    public struct ValueDescription<T> where T : Enum
    {
        public string Description { get; }
        public T Value { get; }

        public ValueDescription(string description, T value)
        {
            Description = description;
            Value = value;
        }
    }
}
