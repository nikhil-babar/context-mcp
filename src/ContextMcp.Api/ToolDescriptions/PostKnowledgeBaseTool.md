**Tool Name:** `post-knowledge`

**Purpose**

Stores one structured knowledge item (pattern, algorithm, constraint, convention, data, decision, or note). Use when you have reusable knowledge worth recalling later.

The request **must** match the flat shape below. There is **no** top-level `type` field: the kind of item is determined by **`content.type`**.

---

# Request shape

```json
{
  "id": "string (optional)",

  "title": "string",

  "summary": "string",

  "tags": ["string"],

  "importance": "low | medium | high",

  "content": { }
}
```

**Required:** `title`, `summary`, `tags`, `importance`, `content`.

**`content`** must be an object whose first required field is **`type`**, one of:

`pattern` | `algorithm` | `constraint` | `convention` | `data` | `decision` | `note`

The remaining properties on `content` depend on that `type`. Use **snake_case** property names as shown below.

---

# Content schemas (inside `content`)

## pattern

```json
{
  "type": "pattern",

  "problem": "string",

  "context": ["string"],

  "triggers": ["string"],

  "solution_steps": ["string"],

  "tradeoffs": ["string"],

  "complexity": "low | medium | high"
}
```

## algorithm

```json
{
  "type": "algorithm",

  "problem_statement": "string",

  "inputs": ["string"],

  "outputs": ["string"],

  "steps": ["string"],

  "time_complexity": "string",

  "space_complexity": "string"
}
```

## constraint

```json
{
  "type": "constraint",

  "rule": "string",

  "affected_components": ["string"],

  "enforcement": "hard | soft",

  "violation_effect": "string",

  "metric": {
    "name": "string",
    "max_value": 0,
    "unit": "string"
  }
}
```

## convention

```json
{
  "type": "convention",

  "rule": "string",

  "applies_to": ["string"],

  "examples": ["string"],

  "enforcement_level": "strict | recommended"
}
```

## data

```json
{
  "type": "data",

  "entity": "string",

  "attributes": {},

  "source_system": "string",

  "valid_from": "ISO-8601 timestamp (optional)"
}
```

## decision

```json
{
  "type": "decision",

  "decision": "string",

  "context": "string",

  "rationale": "string"
}
```

## note

```json
{
  "type": "note",

  "text": "string"
}
```

---

# Example (convention)

```json
{
  "title": "MCP post-knowledge connectivity check",
  "summary": "User requested invoking post-knowledge again; stores a minimal convention node.",
  "tags": ["mcp", "test"],
  "importance": "low",
  "content": {
    "type": "convention",
    "rule": "Use the post-knowledge tool with a request object matching title, summary, tags, importance, and type-specific content under content.",
    "applies_to": ["cursor-agent"],
    "examples": [
      "Call user-context-mcp post-knowledge with content.type convention and content.rule set."
    ],
    "enforcement_level": "recommended"
  }
}
```

---

# Rules

* Do not add properties outside the schemas above.
* **`content.type`** selects the schema; keep it consistent with the fields you send.
* Prefer structured fields over long unstructured prose.
