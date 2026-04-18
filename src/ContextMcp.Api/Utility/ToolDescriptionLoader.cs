namespace ContextMcp.Api.Utility
{
    public static class ToolDescriptions
    {
        public const string PostKnowledgeBaseToolDescription =
            "**Tool Name:** `post-knowledge`\n" +
            "**Purpose**\n" +
            "Stores one structured knowledge item (pattern, algorithm, constraint, convention, data, decision, or note). Use when you have reusable knowledge worth recalling later.\n" +
            "The request **must** match the flat shape below. There is **no** top-level `type` field: the kind of item is determined by **`content.type`**.\n" +
            "---\n" +
            "# Request shape\n" +
            "```json\n" +
            "{\n" +
            "  \"id\": \"string (optional)\",\n" +
            "  \"title\": \"string\",\n" +
            "  \"summary\": \"string\",\n" +
            "  \"tags\": [\"string\"],\n" +
            "  \"importance\": \"low | medium | high\",\n" +
            "  \"content\": { }\n" +
            "}\n" +
            "```\n" +
            "**Required:** `title`, `summary`, `tags`, `importance`, `content`.\n" +
            "**`content`** must be an object whose first required field is **`type`**, one of:\n" +
            "`pattern` | `algorithm` | `constraint` | `convention` | `data` | `decision` | `note`\n" +
            "The remaining properties on `content` depend on that `type`. Use **snake_case** property names as shown below.\n" +
            "---\n" +
            "# Content schemas (inside `content`)\n" +
            "## pattern\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"pattern\",\n" +
            "  \"problem\": \"string\",\n" +
            "  \"context\": [\"string\"],\n" +
            "  \"triggers\": [\"string\"],\n" +
            "  \"solution_steps\": [\"string\"],\n" +
            "  \"tradeoffs\": [\"string\"],\n" +
            "  \"complexity\": \"low | medium | high\"\n" +
            "}\n" +
            "```\n" +
            "## algorithm\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"algorithm\",\n" +
            "  \"problem_statement\": \"string\",\n" +
            "  \"inputs\": [\"string\"],\n" +
            "  \"outputs\": [\"string\"],\n" +
            "  \"steps\": [\"string\"],\n" +
            "  \"time_complexity\": \"string\",\n" +
            "  \"space_complexity\": \"string\"\n" +
            "}\n" +
            "```\n" +
            "## constraint\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"constraint\",\n" +
            "  \"rule\": \"string\",\n" +
            "  \"affected_components\": [\"string\"],\n" +
            "  \"enforcement\": \"hard | soft\",\n" +
            "  \"violation_effect\": \"string\",\n" +
            "  \"metric\": {\n" +
            "    \"name\": \"string\",\n" +
            "    \"max_value\": 0,\n" +
            "    \"unit\": \"string\"\n" +
            "  }\n" +
            "}\n" +
            "```\n" +
            "## convention\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"convention\",\n" +
            "  \"rule\": \"string\",\n" +
            "  \"applies_to\": [\"string\"],\n" +
            "  \"examples\": [\"string\"],\n" +
            "  \"enforcement_level\": \"strict | recommended\"\n" +
            "}\n" +
            "```\n" +
            "## data\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"data\",\n" +
            "  \"entity\": \"string\",\n" +
            "  \"attributes\": {},\n" +
            "  \"source_system\": \"string\",\n" +
            "  \"valid_from\": \"ISO-8601 timestamp (optional)\"\n" +
            "}\n" +
            "```\n" +
            "## decision\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"decision\",\n" +
            "  \"decision\": \"string\",\n" +
            "  \"context\": \"string\",\n" +
            "  \"rationale\": \"string\"\n" +
            "}\n" +
            "```\n" +
            "## note\n" +
            "```json\n" +
            "{\n" +
            "  \"type\": \"note\",\n" +
            "  \"text\": \"string\"\n" +
            "}\n" +
            "```\n" +
            "---\n" +
            "# Example (convention)\n" +
            "```json\n" +
            "{\n" +
            "  \"title\": \"MCP post-knowledge connectivity check\",\n" +
            "  \"summary\": \"User requested invoking post-knowledge again; stores a minimal convention node.\",\n" +
            "  \"tags\": [\"mcp\", \"test\"],\n" +
            "  \"importance\": \"low\",\n" +
            "  \"content\": {\n" +
            "    \"type\": \"convention\",\n" +
            "    \"rule\": \"Use the post-knowledge tool with a request object matching title, summary, tags, importance, and type-specific content under content.\",\n" +
            "    \"applies_to\": [\"cursor-agent\"],\n" +
            "    \"examples\": [\n" +
            "      \"Call user-context-mcp post-knowledge with content.type convention and content.rule set.\"\n" +
            "    ],\n" +
            "    \"enforcement_level\": \"recommended\"\n" +
            "  }\n" +
            "}\n" +
            "```\n" +
            "---\n" +
            "# Rules\n" +
            "* Do not add properties outside the schemas above.\n" +
            "* **`content.type`** selects the schema; keep it consistent with the fields you send.\n" +
            "* Prefer structured fields over long unstructured prose.";

        public const string PostKnowledgeBaseToolKnowledgeBaseParameterDescription =
            "Single knowledge item: required title, summary, tags, importance, and content. The content object must include type (pattern, algorithm, constraint, convention, data, decision, or note) plus that type's fields using snake_case JSON names (e.g. applies_to, enforcement_level for convention). Optional id updates an existing entry.";
    }
}