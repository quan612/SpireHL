using System.Collections.Generic;

namespace CatalogModule.Contracts
{
    public interface IWordCatalogService
    {
        string MakeCatalog<T>(IEnumerable<T> items);
    }
}
