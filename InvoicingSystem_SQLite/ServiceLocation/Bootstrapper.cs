using Prism.Mef;
using System.ComponentModel.Composition.Hosting;

namespace InvoicingSystem_SQLite.ServiceLocation
{
    class Bootstrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }
    }
}
