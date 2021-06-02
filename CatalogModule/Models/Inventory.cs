using SpireHL.Core.Contracts;

namespace CatalogModule.Models
{
    public class Inventory : IInventory
    {
        public int Id { get; }
        public string Name { get; }
        public string Query { get; }
        public Inventory(int id, string name, string query)
        {
            Id = id;
            Name = name;
            Query = query;
        }
    }
}
