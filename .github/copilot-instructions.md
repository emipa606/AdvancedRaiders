# Advanced Raiders (Continued) - Copilot Instructions

## Mod Overview
**Advanced Raiders (Continued)** is a continuation and enhancement of the original mod by saloid. This mod introduces 7 new raider types, enriching the raid mechanics in RimWorld with unique abilities and interactions. It aims to make raids more challenging and dynamic, offering a fresh experience to seasoned players.

## Key Features and Systems
- **New Raider Types:** 
  - *Mercenary Medic:* Carries combat stimulants that can revive downed allies into formidable foes.
  - *Technician:* Equipped with bluescreen belts, became invisible to turrets, which they can sabotage.
  - *Pacifier:* Uses a shock baton to administer PTSD to downed pawns, demoralizing your colony.
  - *Bulldozer:* Acts as a human shield, drawing attacks to themselves.
  - *Tribal Medic:* Resurrects downed allies with altered attributes.
  - *Beastmaster:* Commands battle animals that exhibit varied behaviors upon his incapacitation.
  - *Commander Chief:* Buffs allies and prevents allies from fleeing while active.

- **New Items and Mechanics:** 
  - Introduce combat stimulants and bluescreen belts.
  - Enhance game depth with new research projects to utilize raider technology for your colony.
  
- **Animal Behaviors:** 
  - Beastmaster-related animals react dynamically based on his status.

## Coding Patterns and Conventions
- **Naming Conventions:** 
  - Classes and methods follow PascalCase.
  - Members and variables use camelCase for consistency.
  
- **Modular Components:** 
  - Utilize C# classes within the `Comps` folder for component-specific functionality such as `CompReinforcementsSummoner`.
  
- **Defining XML:** 
  - Organized within the `Defs` folder, XML files define abilities, items, thoughts, and research in a structured manner.
  
- **Error Handling:** 
  - Ensure error logs are uploaded with the Log Uploader tool for efficient troubleshooting.
  
## XML Integration
- **Defs and Properties:** 
  - XML files utilize `ThingDef`, `AbilityDef`, and others for in-game entities.
  - Structure XML to categorize within individual files for Apparel, Abilities, Research Projects, etc.
  
- **Research Implementation:** 
  - Projects tied to new raid interactions, enhancing player engagement with mod features.

## Harmony Patching
- **Dynamic Patching:** 
  - Utilize Harmony to modify existing RimWorld methods, as seen in `Building_TurretGun_IsValidTarget`.
  - Example Harmony Method: `Postfix` allows modification post vanilla execution, perfect for altering turret behavior with Technicians.

## Suggestions for Copilot
- **Automated Code Snippets:**
  - Generate commonly used code constructs for components and Harmony patches to ensure DRY practices.
  
- **XML Template Suggestions:**
  - Auto-generate XML templates for new raider types and corresponding abilities or items.
  
- **Harmony Patch Assistance:**
  - Suggest pre-defined snippets for Prefix and Postfix methods to streamline mod interaction patches.
  
- **Debugging and Optimization:**
  - Propose optimization logic and debugging helpers to ensure smooth gameplay.

By adhering to these structures and engaging with suggestions, you can expand and maintain the Advanced Raiders mod effectively.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

