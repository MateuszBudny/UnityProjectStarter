# Graveyard Idle - Project Mandates & Architecture

## Project Overview
- **Genre:** [TODO]
- **Theme:** [TODO]
- **Goal:** [TODO]

## Architectural Standards
- **Modular Code:** High priority. Logic should be broken down into small, reusable, and decoupled components.
- **LowkeyFramework:** Always check `Assets/LowkeyFramework` first for existing utilities, physics helpers, UI handlers, or systems before implementing new ones.
- **Dependency Management:** **ScriptableObject-based Injection**. 
    - Use **Runtime Sets** or **Reference SOs** for scene-object discovery.
    - Use **Variable SOs** for shared state (e.g., Currency, Player Stats).
    - Avoid `public static Instance` (Singletons) unless using the "Manager" naming convention (see below).
- **Data-Driven Design:** Strictly use **ScriptableObjects** for configuration (costs, speeds, upgrade curves, item definitions).
- **Asynchronous Logic:** Standard **Unity Coroutines**.
- **Decoupling:** Use **NodeCanvas Signals** (SO-based events) to separate core logic from UI, VFX, and SFX.
- **Rendering:** Universal Render Pipeline (URP). Use **Shader Graph** for custom materials.
- **Debugging:** Use `Debug.Log()` generously throughout the codebase to trace execution flow, state changes, and event triggers. This is CRITICAL for remote debugging and verifying behavior via MCP. All debug logs MUST be kept in the code and committed; do not remove them unless they are explicitly marked as temporary.

## Code Conventions (The "Senior Dev" Style)
- **No `var`:** Always use explicit types.
- **Attributes:** `[SerializeField]` and other attributes must be on their own line.
- **Headers:** Add an empty line before every `[Header]`.
- **Braces `{}`:** 
    - Use braces even for single-line `if` statements.
    - **Exception:** Early `return` or `break` at the start of a function to reduce nesting (e.g., `if (...) return;`) should be on two lines and without braces if appropriate.
    - Single-line lambdas and properties are acceptable without braces.
- **Spacing:** 
    - Use empty lines between different categories of members, between methods, and between semantic groups of code lines.
    - Generally, a closing brace `}` should be followed by an empty line (except after `if` when there is an `else` after it).
- **Tooling:** Adhere to `.editorconfig` rules and IDE warnings.
- **Singletons:** Classes with "Manager" in the name are considered Singletons and should inherit from `SingleBehaviour`.

### Member Ordering within Classes:
1. **Variables:**
    - `[SerializeField]` or `public` fields exposed to the Inspector first.
    - `static` fields.
    - `public` properties.
    - `private` properties.
    - `private` fields.
2. **Unity Lifecycle Methods:**
    - `Awake` -> `Start` -> `OnEnable`.
    - Internal events (`OnTrigger`, `OnCollision`, etc.). "Enter" methods always before "Exit".
    - `OnDisable` -> `OnDestroy` (at the very end).
3. **Custom Methods:**
    - Order: `public` then `private` (flexible, but prioritized by importance).
    - Lifecycle: Init/Setup/Restart -> High-level/Core Logic -> Cleanup/Closing -> Helper/Implementation details.

## Technical Stack
- **Engine:** [TODO WITH VERSION].
- **Tooling:** 
    - NodeCanvas (Signals for event-driven logic).
    - **MCP For Unity:** Enables Gemini to interact directly with the Unity Editor (hierarchy, inspector, logs, prefabs).
- **Input:** Input System Package (Actions-based).
- **Physics:** 3D Physics.
- **Animation:** Animator Controller + Juice (DOTween/LeanTween).

## Workflow & "Vibe Coding" Rules
- **Plan-First:** For every new feature, enter `Plan Mode` to discuss architecture and class structure before implementation.
- **Script Validation:** After adding, removing, or modifying any script, always trigger a Unity refresh to check for syntax errors. If compilation succeeds, enter Play Mode to verify there are no runtime errors. Once tests are finished, Play Mode MUST be closed. Any identified issues (syntax or runtime) MUST be fixed immediately.
- **Editor Interaction:** Use **MCP For Unity** tools to inspect the scene, create objects, and verify logic at runtime.
- **Surgical Updates:** Implement features in small, testable increments.
- **Testing:** Core logic (economy, stacking, upgrades) MUST include unit tests.

## Senior Dev Peer-Review & Pro-activity
- **Pro-active Challenge:** Gemini MUST act as a senior peer. If a user directive is architecturally unsound, redundant (exists in framework), or "doesn't make sense" for a [TODO GENRE OF A GAME] game, Gemini MUST flag it and propose a better alternative BEFORE implementing.
- **The Power of "No":** If the user is unsure (e.g., "is there anything more we should add?") and Gemini believes the current state is optimal, Gemini MUST explicitly say "No" rather than inventing unnecessary work. Gemini must only implement questionable ideas if the user explicitly demands it after a warning.
- **Systemic Ideas:** If Gemini identifies an opportunity for a cleaner abstraction, a performance optimization (e.g., object pooling), or a better "feel" (e.g., adding a specific juice effect), it MUST proactively suggest it.
- **Architectural Veto:** If a requested change would break the "ScriptableObject-based Injection" or "Decoupling" mandates, Gemini MUST pause and explain why the change is dangerous.
- **Workflow Optimization Prompts:** If Gemini identifies a recurring preference, global style, or complex multi-step workflow that would benefit from being persisted via `/memory` or automated via a custom `/skill`, Gemini MUST proactively prompt the user to use those features to optimize the session.

## Visual Validation & "Feel" Checks
- **Visual Feedback:** After any UI, VFX, or animation change, Gemini MUST use `mcp_unity_manage_camera(action="screenshot")` or `screenshot_multiview` to check the results (if it is possible for Gemini to see it).
- **Vibe Critique:** The user (Senior Dev) will provide "Vibe Critiques" (e.g., "Movement feels floaty"). Gemini MUST translate these into technical fixes (e.g., "Increase gravity," "Adjust LayoutGroup padding").

## Atomic Feature Commits & Reverts
- **Atomic Commits:** Every sub-feature (e.g., "Basic Player Movement," "Currency Signal Hookup") MUST be committed separately.
- **No-Fix Reverts:** If a "Vibe Experiment" fails to improve the feel after 2 attempts, Gemini MUST proactively suggest a `git revert` to the last "Known Good State" instead of trying to patch the bad experiment.

## Placeholder-First Implementation
- **Standard Primitives:** Use Unity primitives (Cubes for graves, Spheres for pick-ups) with distinct colors via `mcp_unity_manage_material` to ensure systems are "visually identifiable" in Scene View immediately.
- **No-Blocker Asset Rule:** Don't let missing assets stall the vibe; build the system logic first using placeholders.

## Context Hygiene & Session Management
- **Proactive Restart Prompting:** Gemini MUST monitor context usage and suggest a session restart (Process Restart) in the following scenarios:
    - **Context Threshold:** When usage reaches **60-70%** to maintain speed and prevent hallucinations.
    - **Milestone Completion:** Immediately after a successful `git commit` or completion of a major feature/bugfix.
    - **Noise Accumulation:** After a long diagnostic or "trial-and-error" session that has filled the history with technical noise.
    - **Task Switching:** When moving from one major system (e.g., Economy) to a completely different one (e.g., VFX).
- **Restart Benefits:** Restarting ensures Gemini works from the **Ground Truth** (files on disk) rather than session memory, resetting the token budget and ensuring 100% adherence to current mandates.

## Aggressive Loop Detection & Escalation
- **Identifying Loops:** Gemini MUST monitor for repetitive tool calls (e.g., more than 2 failed attempts to find/edit/read the same target) or circular reasoning.
- **Immediate Pause:** If a loop is detected, Gemini MUST stop immediately and provide a **"Loop/Constraint Warning"** to the user.
- **Feasibility Assessment:** If a solution appears mathematically impossible, architecturally unsound, or excessively complex to implement via AI, Gemini MUST flag this as a **"High-Complexity/Impossible Block"** and propose a simplified pivot or ask for manual intervention.
- **No Persistence in Failure:** Do not attempt more than 3 distinct strategies for the same bug/feature without asking for senior developer (user) intervention.
- **Environment Blocks:** If the code is correct but the Unity Editor state (logs, hierarchy) does not reflect the changes after a refresh, Gemini MUST flag this as an **"Environment Desync"** rather than retrying the code change.

## MCP Payload & Pagination Discipline
- **Pagination First:** Gemini MUST use `page_size` (e.g., 25-50) and `cursor` for all `manage_scene(action="get_hierarchy")` and `manage_asset(action="search")` calls to prevent context bloat.
- **Metadata First:** When querying components with `manage_gameobject(action="get_components")`, Gemini MUST start with `include_properties=false` and only request full properties (with small `page_size`) when strictly needed.

## Performance & Feel
- **Mandatory Object Pooling:** Never use `Instantiate()` or `Destroy()` for recurring gameplay objects (currency drops, visitors, visual effects). Gemini MUST use or build an Object Pool (checking `LowkeyFramework` first).
- **The "Juice" Baseline:** Every interactive UI element MUST have a DOTween scale punch. Every currency collection MUST use a trail or jump sequence (leveraging `LowkeyFramework/DOTween`). Every major progression milestone MUST trigger haptic feedback and a particle burst.

## Scene Hierarchy & Prefab Hygiene
- **Root Organizers:** All instantiated scene objects MUST be logically grouped under empty root objects (e.g., `[ENVIRONMENT]`, `[SYSTEMS]`, `[UI]`, `[DYNAMIC]`). Never leave loose objects scattered at the root of the hierarchy.
- **Prefab First:** If an object will exist more than once (e.g., a Grave, a Visitor, a Resource Drop), Gemini MUST create it as a Prefab (`mcp_unity_manage_prefabs`) and instantiate the Prefab, rather than duplicating in-scene GameObjects.
- **Save Prefab Overrides:** When modifying an existing prefab instance in the scene to achieve a "vibe" change, Gemini MUST explicitly save those changes back to the Prefab asset.
- **Internal Object Structure:** When creating a new complex object, do NOT put everything on the root GameObject.
    - **Root:** Holds the core logic components. Keep the most important and defining components at the top of the inspector list.
    - **Child "VisualPhysics":** An empty child GameObject named exactly `VisualPhysics` that acts as the root for all visual and physical representation. It MUST always have a scale of `(1, 1, 1)`, position `(0, 0, 0)`, and NO components other than `Transform`.
    - **Colliders & Meshes:** All colliders and `MeshRenderer`s MUST be placed on children of `VisualPhysics`. (If it makes sense, a collider and mesh renderer can share the same child object).

## UI & Input
- **Canvas Rebuild Optimization:** A single massive Canvas causes huge performance spikes on mobile whenever one element changes. Gemini MUST separate UI into at least two canvases:
    - **Static Canvas:** For menus, buttons, and backgrounds that rarely change.
    - **Dynamic Canvas:** For rapidly updating elements (e.g., the Currency counter, floating text, progress bars).
- **Input:** Use the existing Input System Package. Do not hardcode keyboard/mouse controls (`Input.GetAxis`) in the player controller.
- **Input System SO Lifecycle:** When using ScriptableObjects to handle Input System actions (e.g., `InputReaderSO`), do NOT rely on the SO's internal `OnEnable`/`OnDisable` to toggle `_inputActions.Enable()`. SOs stay in memory in the Editor, causing them to miss Play Mode transitions. Instead, expose public `EnableInput()`/`DisableInput()` methods on the SO and call them explicitly from the `OnEnable`/`OnDisable` of the scene's `MonoBehaviour` (e.g., `PlayerController`).

## Engine & Editor Specifics
- **Prefab Updates via MCP:** When updating an existing prefab asset from a modified scene instance using `manage_prefabs` (`action="create_from_gameobject"`), you MUST use `unlink_if_instance: true` and `allow_overwrite: true`.
- **Batch Execution Syntax:** When using `mcp_unity_batch_execute`, the tool names inside the payload MUST match the internal tool names without the `mcp_unity_` prefix (e.g., use `manage_gameobject`, `manage_components`, `manage_material`).
