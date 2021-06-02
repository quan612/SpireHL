using Dapper;
using SpireHL.Core.Models;
using SpireHL.Core.Repository;
using System.Collections.Generic;
using System.Linq;

namespace CatalogModule.Repository
{
    public class SaleOrderRepository : BaseRepository
    {
        public SaleOrderRepository()
        {

        }

        public List<SpireSaleOrderItem> GetSaleOrderItems(string order_no)
        {
            var sql = @"
                        SELECT inv.part_no PartNo, inv.description Description
                                , uoms.sell_prices[1]::numeric(15,2) Price1 
                                , uoms.sell_prices[2]::numeric(15,2) Price2
                                , uoms.sell_prices[3]::numeric(15,2) Price3
                                , inv.onhand_qty OnHandQty
                                , inv.purchase_qty as PurchaseQty
                                , inv.misc_1 as InventoryType
                                , to_char(po_due_date, 'MM/DD/YYYY') as ArrivalDate
                                , inv.min_buy_qty as MinOrder
                                , inv.udf_data as UDFData
                                
                                , inv.image_path as ProductImage
                                , inv.product_code as ProductCode
                                , v.reference as VendorCode
                                , iuc.upc_code as UPC

                                , soi.order_no OrderNo
                                , soi.order_qty OrderQty
                                , soi.unit_price UnitPrice

                                FROM public.inventory inv 
                                    JOIN public.inventory_uoms uoms ON inv.part_no = uoms.part_no and uoms.whse = '00' and inv.whse = '00'
                                    JOIN vendors v ON v.vendor_no = inv.preferred_vendor
                                    JOIN inventory_upc_codes iuc on iuc.part_no = inv.part_no and iuc.whse='00'
                                    JOIN public.sales_order_items soi on soi.part_no = inv.part_no

where 1=1 and order_no =@OrderNo
                            
                        ";
            using (var connection = GetConnection())
            {
                connection.Open();
                var parameters = new { OrderNo = order_no };

                var result = connection.Query<SpireSaleOrderItem>(sql, parameters);
                return result.ToList();
            }
        }

        public List<SpireSaleOrderItem> GetSaleHistoryItems(string invoice_no)
        {
            var sql = @"
                        SELECT inv.part_no PartNo, inv.description Description
                                , uoms.sell_prices[1]::numeric(15,2) Price1 
                                , uoms.sell_prices[2]::numeric(15,2) Price2
                                , uoms.sell_prices[3]::numeric(15,2) Price3
                                , inv.onhand_qty OnHandQty
                                , inv.purchase_qty as PurchaseQty
                                , inv.misc_1 as InventoryType
                                , to_char(po_due_date, 'MM/DD/YYYY') as ArrivalDate
                                , inv.min_buy_qty as MinOrder
                                , inv.udf_data as UDFData
                                
                                , inv.image_path as ProductImage
                                , inv.product_code as ProductCode
                                , v.reference as VendorCode
                                , iuc.upc_code as UPC

                                , shi.invoice_no InvoiceNo
                                , shi.order_qty OrderQty
                                , shi.unit_price UnitPrice

                                FROM public.inventory inv 
                                    JOIN public.inventory_uoms uoms ON inv.part_no = uoms.part_no and uoms.whse = '00' and inv.whse = '00'
                                    JOIN vendors v ON v.vendor_no = inv.preferred_vendor
                                    JOIN inventory_upc_codes iuc on iuc.part_no = inv.part_no and iuc.whse='00'
                                    JOIN public.sales_history_items shi on shi.part_no = inv.part_no

                            where 1=1 and shi.invoice_no = @InvoiceNumber
                            
                        ";
            using (var connection = GetConnection())
            {
                connection.Open();
                var parameters = new { InvoiceNumber = invoice_no };

                var result = connection.Query<SpireSaleOrderItem>(sql, parameters);
                return result.ToList();
            }
        }
    }
}
