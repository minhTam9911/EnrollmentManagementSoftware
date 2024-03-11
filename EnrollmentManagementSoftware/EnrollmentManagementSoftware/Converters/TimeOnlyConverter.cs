
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnrollmentManagementSoftware.Converters;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    public string formatTime = "hh:mm tt";

	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return TimeOnly.ParseExact(reader.GetString(), formatTime, CultureInfo.InvariantCulture);
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(formatTime));
	}
}

