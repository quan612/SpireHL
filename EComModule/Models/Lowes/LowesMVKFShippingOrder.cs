namespace EComModule.Models.Lowes
{
    public class LowesMVKFShippingOrder
    {
        public string SPI { get { return "S"; } set { } }
        public string IAQ { get { return "A"; } set { } }
        public string OrderId { get; set; }
        public string Carrier { get { return "PURL"; } set { } }
        public string Attention { get; set; }
        public string CustName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get { return "CA"; } set { } }
        public string Phone { get; set; }
        public string Service { get { return "GND"; } set { } }
        public string CustPackage { get { return "U"; } set { } }
        public string PaymentTerms { get { return "P"; } set { } }
        public string PCS { get { return "1"; } set { } }

        public string WGT { get; set; }
        public string Reference { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Notes { get; set; }
        public string PrinterId
        {
            get { return "E0100010519 - 01"; }
            set { }
        }
    }
}
