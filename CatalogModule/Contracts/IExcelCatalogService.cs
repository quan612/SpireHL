using System.Collections.Generic;

namespace CatalogModule.Contracts
{
    public interface IExcelCatalogService
    {
        string MakeCatalog<T>(IEnumerable<T> items);
        string MakeCatalogFromObjectCollectionUsingTemplateMaker<T>(IEnumerable<T> items);
        string MakeCatalogFromObjectCollectionUsingImportData<T>(IEnumerable<T> items);
    }
}
