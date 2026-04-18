using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models
{
    [Description("Structured content for note-type knowledge entries.")]
    public class NoteContent : KnowledgeBaseContent
    {
        [Description("Discriminator value for note content.")]
        [JsonPropertyName("type")]
        public override string Type => "note";

        [Description("Free-form text content of the note.")]
        [JsonPropertyName("text")]
        public required string Text { get; set; }
    }
}

