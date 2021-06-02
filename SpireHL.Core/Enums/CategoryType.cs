using SpireHL.Core.Converters;
using System.ComponentModel;

namespace SpireHL.Core.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum CategoryType
    {
        [Description("No filter")]
        Default,

        [Description("Animal Statues ~ FIG-AML")]
        AnimalStatue,

        [Description("Fairy Garden & Fountains - FTN")]
        Fountain,

        [Description("Led lights & PH items")]
        LedPH,

        [Description("Garden statue & Decor")]
        Decor
    }
}
