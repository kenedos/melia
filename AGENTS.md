# Agent instructions

This repository's canonical AI-agent entry point is **[CLAUDE.md](CLAUDE.md)**.

Read it first — it contains the build commands, codebase map, and hard rules
(Laima package override semantics, data tiers, migration policy).

Then use **[doc/dev/README.md](doc/dev/README.md)** as the index of developer
documentation: subsystem guides (skill/buff/pad handlers, data system,
persistence, scripting, networking), known issues, and the maintenance table
that maps code areas to their owning docs.

Step-by-step procedures with code templates and checklists live in
`.claude/skills/*/SKILL.md` — they are plain markdown and useful to any tool,
not just Claude Code.

When you change a subsystem's behavior, update its `doc/dev/` page and the
related skill checklist in the same change.
