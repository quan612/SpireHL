

namespace SpireHL.Core.Repository
{
    public class BaseInventoryRepository : BaseRepository
    {
        protected string BaseCatalogSQL = @"
                            SELECT 
                                  inv.part_no PartNo
                                , inv.description Description
                                , uoms.sell_prices[1]::numeric(15,2) Price1 
                                , uoms.sell_prices[2]::numeric(15,2) Price2
                                , uoms.sell_prices[3]::numeric(15,2) Price3
                                , uoms.weight::numeric(11,2) Weight

                                , inv.onhand_qty OnHandQty
                                , inv.backorder_qty BackorderQty
                                , inv.committed_qty CommittedQty
                                , inv.reorder_qty ReorderQty
                                , inv.current_cost CurrentCost
                                , inv.average_cost AverageCost
                                , inv.purchase_qty as PurchaseQty

                                , inv.misc_1 as InventoryType
                                , to_char(po_due_date, 'MM/DD/YYYY') as ArrivalDate
                                , inv.min_buy_qty as MinOrder
                                , inv.udf_data as UDFData
                                
                                , inv.image_path as ProductImage
                                , inv.product_code as ProductCode
                                , v.reference as VendorCode
                                , iuc.upc_code as UPC

                                , inv.last_sale_date LastSaleDate

                                FROM public.inventory inv 
                                    JOIN public.inventory_uoms uoms ON inv.part_no = uoms.part_no and uoms.whse = '00' and inv.whse = '00'
                                    Left JOIN vendors v ON v.vendor_no = inv.preferred_vendor
                                    JOIN inventory_upc_codes iuc on iuc.part_no = inv.part_no and iuc.whse='00'
                            
                            where 1=1
-- and inv.part_no = '20118' 
-- or inv.part_no = '37371-96'  
-- or inv.part_no = '78523-B'
";

        protected string BaseSaleAnalysisSQL = @"
                            SELECT 
	                     --         inv.part_no PartNo, inv.description Description
                         --     , uoms.sell_prices[1]::numeric(15,2) Price1 
                         --     , uoms.sell_prices[2]::numeric(15,2) Price2
                         --     , uoms.sell_prices[3]::numeric(15,2) Price3
                         --     , uoms.weight::numeric(11,2) Weight
                         --     , inv.onhand_qty OnHandQty
                         --     , inv.purchase_qty as PurchaseQty
                         --     , inv.misc_1 as InventoryType
                         --     , to_char(po_due_date, 'MM/DD/YYYY') as ArrivalDate
                         --     , inv.min_buy_qty as MinOrder
                         --     , inv.udf_data as UDFData
                         --     
                         --     , inv.image_path as ProductImage
                         --     , inv.product_code as ProductCode
                         --     , v.reference as VendorCode
                         --     , iuc.upc_code as UPC
								
 row_to_json(sta.*) as SaleAnalysisData

                                FROM public.inventory inv 
                                    JOIN public.inventory_uoms uoms ON inv.part_no = uoms.part_no and uoms.whse = '00' and inv.whse = '00'
                                    JOIN vendors v ON v.vendor_no = inv.preferred_vendor
                                    JOIN inventory_upc_codes iuc on iuc.part_no = inv.part_no and iuc.whse='00'
                                    JOIN inventory_statistics sta on sta.part_no = inv.part_no and sta.whse = '00'
                            where 1=1 
--and sta.part_no = '20118'
--or sta.part_no = '37371-96' 
--or sta.part_no = '78523-B'";

        protected string BaseSaleAnalysisTest = @"
                            SELECT 
	                             inv.part_no PartNo, inv.description Description
                             , uoms.sell_prices[1]::numeric(15,2) Price1 
                             , uoms.sell_prices[2]::numeric(15,2) Price2
                             , uoms.sell_prices[3]::numeric(15,2) Price3
                             , uoms.weight::numeric(11,2) Weight
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
				, row_to_json(sta.*) as SaleAnalysisData				
--, sta.year_end SaleAnalysisYearEnd
--, sta.period SaleAnalysisPeriod
--, sta.qty SaleAnalysisQty
--, sta.total_sell SaleAnalysisTotalSell

                                FROM public.inventory inv 
                                    JOIN public.inventory_uoms uoms ON inv.part_no = uoms.part_no and uoms.whse = '00' and inv.whse = '00'
                                    JOIN vendors v ON v.vendor_no = inv.preferred_vendor
                                    JOIN inventory_upc_codes iuc on iuc.part_no = inv.part_no and iuc.whse='00'
                                    JOIN inventory_statistics sta on sta.part_no = inv.part_no and sta.whse = '00'
                            where 1=1 
							and sta.part_no = '20118' or sta.part_no = '37371-96'";
        public BaseInventoryRepository()
        {

        }
    }
}
