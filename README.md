# HGSCustomAPIs

> **Haven Game Studio — Custom APIs for Top-Down 2D Games**
> Built with Unity · Written in C# · Designed for extensibility

---

## ⚠️ Notice

> **This API is still under active development.** The game sample used to build this system is not yet released. Scripts may be modified, refactored, or extended at any time.

---

## 📖 Overview

**HGSCustomAPIs** is a modular, extensible API framework developed by Haven Game Studio for building top-down 2D games in Unity. It provides a complete foundation for character management, combat, animation, AI behavior, and developer tooling — all designed to be easy to read, extend, and integrate.

---

## 📁 Repository Structure

```
HGSCustomAPIs/
├── AI/             # AI Brain, Actions, and Decisions
├── Animator/       # Animation state handling
├── Character/      # Core character logic and abilities
├── Editor/         # Custom Unity Editor tools
├── FeedBacks/      # Feedback system (visual/audio responses)
├── Items/          # Item definitions and interactions
├── Tools/          # Developer utility scripts
└── Weapon/         # Melee and Projectile weapon systems
```

---

## 🧩 Modules

### 1. Character (`/Character`)

The foundation of all in-game entities. The `Character.cs` script is the **most critical** script in the entire API — every character (player or NPC) depends on it.

**Key features:**
- Manages core character state (health, movement, status effects)
- Acts as the central hub connecting abilities, weapons, and feedback
- Required base for any character in the game

**Extending characters:**
To create a new character, attach `Character.cs` to a GameObject and configure its parameters via the Inspector.

---

### 2. Character Abilities (`/Character`)

A flexible, **component-based ability system** built on top of `CharacterAbility.cs`.

**How it works:**
- Each ability is a separate `MonoBehaviour` that **inherits from `CharacterAbility.cs`**
- Abilities are automatically detected and managed by the `Character` component
- Designed to be readable, stackable, and easy to extend

**Creating a custom ability:**
```csharp
using HavenGameStudio;

public class MyCustomAbility : CharacterAbility
{
    protected override void Initialization()
    {
        base.Initialization();
        // Setup your ability here
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();
        // Your ability logic here
    }
}
```

Attach your new ability script to the same GameObject as `Character.cs` and it will be picked up automatically.

---

### 3. Weapon System (`/Weapon`)

Two distinct weapon types are supported out of the box:

| Type | Description |
|---|---|
| **Melee Weapon** | Close-range attacks with hit detection |
| **Projectile Weapon** | Ranged attacks that spawn and fire projectiles |

**Key points:**
- Weapons interact with the Feedback System to trigger effects on hit
- Both weapon types integrate with `CharacterAbilities` for firing/attacking logic
- Weapons can be attached to and managed by the `Character` component

---

### 4. Feedback System (`/FeedBacks`)

A centralized system for playing **visual and audio responses** to in-game events.

**Common use cases:**
- Flash effects when a character takes damage
- Sound cues when a weapon fires
- Screen shake or particle effects on death

**Integration points:**
The Feedback System hooks into:
- `Health.cs` — damage and death events
- `CharacterAbilities` — ability activation/deactivation
- `Weapon` — hit and fire events

Developers can define feedback sequences in the Inspector and trigger them from any of the above systems without additional code.

---

### 5. AI Brain (`/AI`)

A **behavior tree** implementation that serves as the decision-making core for non-player characters.

**How it works:**
- The AI Brain evaluates a set of `AIDecision` conditions every tick
- When a decision resolves to `true`, it triggers the corresponding `AIAction`
- States and transitions are managed automatically by the Brain

**Design philosophy:**
The system is intentionally modular — each decision and action is a standalone script, making it easy to build complex AI behavior by composing simple, readable components.

---

### 6. AI Actions & Decisions (`/AI`)

These are the building blocks of the AI Brain.

#### AI Action
Represents a **task the AI performs** when triggered.

Examples: Move to target, Attack, Patrol, Wait

```csharp
public class AIActionMoveToTarget : AIAction
{
    public override void PerformAction()
    {
        // Move toward the detected target
    }
}
```

#### AI Decision
Governs the AI's **reasoning process** — determines whether a condition is met.

Examples: Detect Player, Health Below Threshold, Is In Range

```csharp
public class AIDecisionDetectPlayer : AIDecision
{
    public override bool Decide()
    {
        // Return true if a player is within detection range
        return false;
    }
}
```

---

### 7. Animator (`/Animator`)

Handles animation state management for characters and objects.

- Bridges character states (moving, attacking, dying) with Unity's Animator component
- Works in sync with `Character.cs` to automatically trigger the correct animation states

---

### 8. Editor Tools (`/Editor`)

Custom Unity Editor scripts that extend the Inspector and editor workflow for Haven Game Studio projects.

These tools are for **developer use only** and do not affect runtime behavior.

---

### 9. Items (`/Items`)

Definitions and logic for in-game items.

- Provides a base item structure that can be extended for any item type (consumables, equipment, collectibles)
- Designed to integrate with the Character and Weapon systems

---

### 10. Tools (`/Tools`)

Miscellaneous developer utility scripts used throughout the project.

- Helper methods, extension utilities, and shared constants
- Used internally by other modules

---

## 🚀 Getting Started

### Requirements
- **Unity** (2D project, recommended LTS version)
- **C#** scripting backend

### Installation

> ⚠️ **Important:** Download the **entire repository** as a whole. Do **not** download individual folders — the modules are interdependent and will not function correctly if used in isolation.

1. Clone or download this repository in full.
   ```
   git clone https://github.com/HavenGameStudio/HGSCustomAPIs.git
   ```
2. Copy the **entire** extracted folder into your Unity project's `Assets/` directory.
3. Ensure your scene has a `Character` GameObject with `Character.cs` attached as the starting point.

### Minimal Setup
```
GameObject (your character)
├── Character.cs          ← Required
├── CharacterAbility.cs   ← One or more abilities
├── [Weapon].cs           ← Optional: attach a weapon
└── Animator              ← Attach Unity Animator component
```

---

## 🔧 Extending the API

| What you want to do | What to do |
|---|---|
| Add a new character ability | Create a `MonoBehaviour` inheriting `CharacterAbility` |
| Add a new AI behavior | Create an `AIAction` or `AIDecision` subclass |
| Add a new weapon type | Extend the base Weapon class |
| Trigger a feedback effect | Call the feedback from `Health`, `Ability`, or `Weapon` |

---

## 🏢 About

**Haven Game Studio** is an indie game development studio currently building a top-down 2D game. This API was developed internally to support that project and is being shared publicly to help other developers.

- 🔗 GitHub: [github.com/HavenGameStudio](https://github.com/HavenGameStudio)
- 📦 Repository: [HGSCustomAPIs](https://github.com/HavenGameStudio/HGSCustomAPIs)
- 💬 Language: C# (100%)

---

## 📄 License

No license is currently specified for this repository. Please contact Haven Game Studio before using this code in commercial projects.

---

*README generated for HGSCustomAPIs — Haven Game Studio*