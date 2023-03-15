using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PetStok.Entities
{
    public class StokKaydi
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime tarih { get; set; }
        public int stok { get; set; }
    }
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string dateString = reader.GetString();
                DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                return date;
            }
            else
            {
                throw new JsonException();
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
