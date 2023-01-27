using System;
using LiteDB;
using System.Globalization;
using System.Text.Json;

namespace LiteDbExplorer
{
    public class BsonToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return "";
            var bson = (BsonDocument)value;
            return JsonPrettify(bson.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public string JsonPrettify(string json)
        {
            using var jDoc = JsonDocument.Parse(json);
            return System.Text.Json.JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}

