using InventoryManagement.Enums;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InventoryManagement.JsonConverters
{
    internal class ItemStatusTypeConverter : JsonConverter<ItemStatusTypes>
    {
        public override ItemStatusTypes Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            
            switch(value)
            {
                case "register":
                    return ItemStatusTypes.RegisterItem;
                case "unregister":
                    return ItemStatusTypes.UnregisterItem;
                case "lend":
                    return ItemStatusTypes.LendItem;
                case "unlend":
                    return ItemStatusTypes.UnlendItem;
                default:
                    break;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ItemStatusTypes value, JsonSerializerOptions options)
        {
            string result;
            if(options.PropertyNamingPolicy == null)
            {
                result = value.ToString();
            }
            else
            {
                result = options.PropertyNamingPolicy.ConvertName($"{value}");
            }
            writer.WriteStringValue(result);
        }
    }
}
