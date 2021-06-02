using System;
using System.Linq;

namespace EComModule.FieldDelimited
{
    /// <summary>
    /// Represents a serializer that will serialize arbitrary objects to files with specific column separators.
    /// </summar>
    public class DelimitedSerializer
    {
        /// <summary>
        /// The string to be used to separate columns.
        /// </summary>
        public string ColumnDelimiter { get; set; }

        /// <summary>
        /// The string to be used to wrap around the field.
        /// </summary>
        public bool HasDoubleQuotes { get; set; }

        public bool HasQuotes { get; set; }

        public DelimitedSerializer(bool hasDoubleQuotes = false, bool hasQuotes = false)
        {
            HasDoubleQuotes = hasDoubleQuotes;
            HasQuotes = hasQuotes;
        }

        private readonly string doubleQuotes = "\"";
        private readonly string quotes = "\'";


        public string Serialize<T>(T oldRecords, T newRecords)
        {
            if (string.IsNullOrEmpty(ColumnDelimiter))
                throw new ArgumentException($"The property '{nameof(ColumnDelimiter)}' cannot be null or an empty string.");

            // Quan: Using getType() instead of typeOf() in order to get type of instance during runtime, not at compile
            var type = oldRecords.GetType();
            var properties = type.GetProperties()
                .Where(x => Attribute.IsDefined(x, typeof(DelimitedFieldAttribute)))
                .OrderBy(x =>
                    ((DelimitedFieldAttribute)x.GetCustomAttributes(typeof(DelimitedFieldAttribute), true)[0]).Order);

            var result = "";
            result += string.Join(ColumnDelimiter, properties
                .Select(x =>
                {
                    var newValue = x.GetValue(newRecords) != null ?
                        x.GetValue(newRecords) :
                        x.GetValue(oldRecords);

                    if (HasDoubleQuotes) return doubleQuotes + newValue.ToString() + doubleQuotes;
                    if (HasQuotes) return quotes + newValue.ToString() + quotes;
                    return newValue;
                }));

            return result;
        }

        /// <summary>
        /// Returns an instance of the <see cref="DelimitedSerializer"/> setup for Tab-Separated Value files.
        /// </summary>
        public static DelimitedSerializer TsvSerializer => new DelimitedSerializer { ColumnDelimiter = "\t" };

        /// <summary>
        /// Returns an instance of the <see cref="DelimitedSerializer"/> setup for Comma-Separated Value files.
        /// </summary>
        public static DelimitedSerializer CsvSerializer => new DelimitedSerializer { ColumnDelimiter = "," };

        public static DelimitedSerializer CsvSerializerWithDoubleQuotes =>
            new DelimitedSerializer { ColumnDelimiter = ",", HasDoubleQuotes = true };

        /// <summary>
        /// Returns an instance of the <see cref="DelimitedSerializer"/> setup for Pipe-Separated Value files.
        /// </summary>
        public static DelimitedSerializer PsvSerializer => new DelimitedSerializer { ColumnDelimiter = "|" };
    }
}
