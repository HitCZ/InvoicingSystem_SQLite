using System;

namespace InvoicingSystem_SQLite.Logic.Exceptions
{
    public class InvalidPropertyInformationException : Exception
    {
        public InvalidPropertyInformationException(string msg) : base(msg)
        {
        }
    }
}
