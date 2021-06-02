using SpireHL.Core.Contracts;
using System.Collections.Generic;

namespace CatalogModule.Models
{
    public class InventoryList : List<Inventory>
    {
        public InventoryList()
        {
            Clear();
            Add(new Inventory(1, "00 Inventory On Hand", @" and inv.hold = 0 -- 'Active'
                                                            and (inv.misc_1 like '%S%' 
                                                            and inv.misc_1 <> 'SAM' 
                                                            and inv.misc_1 <> 'MENARDS')
                                                            and inv.onhand_qty > 0"));

            Add(new Inventory(2, "Current Catalogue", 
                @" and inv.hold = 0 -- 'Active'
                   and (inv.misc_1 like '%S%' and inv.misc_1 <> 'SAM' and inv.misc_1 <> 'MENARDS') 
                   --fetch first 200 rows only"));

            Add(new Inventory(3, "SX and No Stock", @" and inv.misc_1 like '%SX%' 
                                                       and (inv.onhand_qty - inv.committed_qty) < 1 "));

            Add(new Inventory(4, "CTC Corp#", @" and inv.misc_1 <> '%X%' 
                                                 and inv.udf_data -> 'cdntire' is not null "));

            Add(new Inventory(5, "Steel Book", @" and inv.misc_1 like 'S%' "));

            Add(new Inventory(6, "Slow Mover No Sale Since Jan 2020", 
                @"and inv.hold = 0
                  and inv.misc_1 <> 'X' and inv.misc_1 <> 'SAM' and inv.misc_1 <> 'MERNARDS' 
                  and inv.onhand_qty > 0
                  and inv.product_code <> 'FSH' "));

            Add(new Inventory(7, "Upload To SZ", @" 
                                                    and inv.hold = 0
                                                    and inv.misc_1 like '%S%' 
                                                    and inv.misc_1 <> 'SAM' and inv.misc_1 <> 'MERNARDS' 
                                                    and inv.misc_1 not like '%DT%' 
                                                    and inv.misc_1 not like '%C%' 
                                                    and inv.misc_1 not like '%PATENT%' 
                                                    and inv.misc_1 not like '%A%'"));

            Add(new Inventory(8, "Current Catalog Analysis", @" and inv.hold = 0
                                                                and inv.misc_1 like '%S%'
                                                                and inv.misc_1 <> 'SAM' "));
        }

        public IInventory InventoryOnHand => this.Find(e => e.Name == "00 Inventory On Hand");
        public IInventory CurrentCatalog => this.Find(e => e.Name == "Current Catalogue");
        public IInventory SXNoStock => this.Find(e => e.Name == "SX and No Stock");
        public IInventory CTCCorp => this.Find(e => e.Name == "CTC Corp#");
        public IInventory SteelBook => this.Find(e => e.Name == "Steel Book");
        public IInventory SlowMover => this.Find(e => e.Name == "Slow Mover No Sale Since Jan 2020");
        public IInventory UploadToSZ => this.Find(e => e.Name == "Upload To SZ");
        public IInventory CurrentCatalogAnalysis => this.Find(e => e.Name == "Current Catalog Analysis");

    }
}
