using System.Text.Json;
using System.Text.Json.Serialization;
using ContextMcp.Api.Models;

namespace ContextMcp.Api.Utility
{
    public sealed class KnowledgeBaseContentJsonConverter : JsonConverter<KnowledgeBaseContent>
    {
        public override KnowledgeBaseContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!TryGetDiscriminator(root, options, out var discriminator) || string.IsNullOrEmpty(discriminator))
                throw new JsonException("Missing 'type' discriminator for knowledge base content.");

            var concreteType = KnowledgebaseRegistry.Resolve(discriminator);
            if (concreteType is null)
                throw new JsonException($"Unknown knowledge base content type '{discriminator}'.");

            var innerOptions = CopyOptionsWithoutThisConverter(options);
            var result = JsonSerializer.Deserialize(root, concreteType, innerOptions);
            if (result is null)
                throw new JsonException($"Failed to deserialize content as '{concreteType.Name}'.");

            return (KnowledgeBaseContent)result;
        }

        public override void Write(Utf8JsonWriter writer, KnowledgeBaseContent value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        private static bool TryGetDiscriminator(JsonElement root, JsonSerializerOptions options, out string? discriminator)
        {
            var name = options.PropertyNamingPolicy?.ConvertName("Type") ?? "type";
            if (root.TryGetProperty(name, out var prop) && prop.ValueKind == JsonValueKind.String)
            {
                discriminator = prop.GetString();
                return true;
            }

            if (root.TryGetProperty("type", out prop) && prop.ValueKind == JsonValueKind.String)
            {
                discriminator = prop.GetString();
                return true;
            }

            discriminator = null;
            return false;
        }

        private static JsonSerializerOptions CopyOptionsWithoutThisConverter(JsonSerializerOptions options)
        {
            var copy = new JsonSerializerOptions(options);
            for (var i = copy.Converters.Count - 1; i >= 0; i--)
            {
                if (copy.Converters[i] is KnowledgeBaseContentJsonConverter)
                    copy.Converters.RemoveAt(i);
            }

            return copy;
        }
    }
}
