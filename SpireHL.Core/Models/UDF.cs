using Newtonsoft.Json;

namespace SpireHL.Core.Models
{
    public class UDF
    {
        public string HS { get; set; }
        public bool CTC { get; set; }
        public string PKG { get; set; }
        public bool TSC { get; set; }
        public string _6895 { get; set; }
        public bool HDca { get; set; }
        public string HSDES { get; set; }
        public string IBoxH { get; set; }
        public string IBoxL { get; set; }
        public string IBoxW { get; set; }
        public string MBoxH { get; set; }
        public string MBoxL { get; set; }
        public string MBoxW { get; set; }
        public string Width { get; set; }
        public string CTCSKU { get; set; }
        public bool HDcom { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }

        [JsonProperty("Pc/Ctn")]
        public string PcCtn { get; set; }
        public string _6895LOC { get; set; }

        [JsonProperty("CBM/Ctn")]
        public string CBMCtn { get; set; }
        public string RATE_US { get; set; }
        public string _6895QTY { get; set; }

        [JsonProperty("Box Size")]
        public string BoxSize { get; set; }

        [JsonProperty("Cube/Ctn")]
        public string CubeCtn { get; set; }
        public bool Lowesca { get; set; }
        public string RATE_CAN { get; set; }
        public bool Amazonca { get; set; }
        public string Catagory1 { get; set; }
        public string Catagory2 { get; set; }
        public string Catagory3 { get; set; }
        public bool Costcoca { get; set; }
        public bool Hayneedle { get; set; }
        public bool Oakvalley { get; set; }
        public string PKGMethod { get; set; }
        public string Wcategory { get; set; }
        public bool Amazoncom { get; set; }
        public bool Wayfairca { get; set; }
        public bool Wayfairco { get; set; }

        [JsonProperty("cdntire")]
        public string CDNTire { get; set; }

        public string MOP { get; set; }
    }
}
