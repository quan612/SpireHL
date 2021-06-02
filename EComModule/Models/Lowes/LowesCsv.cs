using EComModule.FieldDelimited;

namespace EComModule.Models.Lowes
{
    public class LowesCsv
    {
        [DelimitedField(Order = 1)]
        public string PKG_PACKAGE_ID { get; set; }

        [DelimitedField(Order = 2)]
        public string SHPTO_NAME { get; set; }

        [DelimitedField(Order = 3)]
        public string SHPTO_ADDRESS_1 { get; set; }

        [DelimitedField(Order = 4)]
        public string SHPTO_ADDRESS_2 { get; set; }

        [DelimitedField(Order = 5)]
        public string SHPTO_ADDRESS_3 { get; set; }

        [DelimitedField(Order = 6)]
        public string SHPTO_CITY { get; set; }

        [DelimitedField(Order = 7)]
        public string SHPTO_STATE_PROV { get; set; }

        [DelimitedField(Order = 8)]
        public string SHPTO_POSTAL_CODE { get; set; }

        [DelimitedField(Order = 9)]
        public string SHPTO_COUNTRY_ID { get; set; }

        [DelimitedField(Order = 10)]
        public string SHPTO_TELEPHONE { get; set; }

        [DelimitedField(Order = 11)]
        public string PKG_SERVICE_TYPE { get; set; }

        [DelimitedField(Order = 12)]
        public string PKG_WEIGHT_ACTUAL { get; set; }

        [DelimitedField(Order = 13)]
        public string PKG_CUSTOM1 { get; set; }

        [DelimitedField(Order = 14)]
        public string PKG_CUSTOM2 { get; set; }

        [DelimitedField(Order = 15)]
        public string PKG_CUSTOM3 { get; set; }

        [DelimitedField(Order = 16)]
        public string PKG_CUSTOM4 { get; set; }

        [DelimitedField(Order = 17)]
        public string PKG_CUSTOM5 { get; set; }

        [DelimitedField(Order = 18)]
        public string SHPTO_RESIDENTIAL { get; set; }

        [DelimitedField(Order = 19)]
        public string UOL_SOURCE { get; set; }

        [DelimitedField(Order = 20)]
        public string SHPTO_ATTN_LINE { get; set; }

        [DelimitedField(Order = 21)]
        public string SHPTO_COMPANY { get; set; }

        [DelimitedField(Order = 22)]
        public string MERCHANT_ID { get; set; }
    }
}
