using System;
using System.ComponentModel.Composition;

namespace InvoicingSystem_SQLite.Logic.Validators
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class DateValidator
    {
        public uint MinimalAllowedYear => 2000;
        public uint MaximalAllowedYear => (uint)DateTime.Today.Year + 1;

        public bool Validate(DateTime date) => date.Year >= MinimalAllowedYear && date.Year <= MaximalAllowedYear;
    }
}
