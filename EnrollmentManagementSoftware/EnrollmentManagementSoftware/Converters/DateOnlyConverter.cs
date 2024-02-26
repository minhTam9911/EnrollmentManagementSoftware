
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnrollmentManagementSoftware.Converters;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public string formatDate = "yyyy-MM-dd";
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString(), formatDate, CultureInfo.InvariantCulture);
    }


    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(formatDate));
    }

 
}
