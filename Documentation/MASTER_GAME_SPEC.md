# Master Game Specification

## Product

**Kingdom Last Stand** is a portrait-oriented mobile tower-defense, merge, and roguelite game. The player recruits units, drags them between battlefield slots, merges matching units, chooses one of three temporary upgrades, and activates one hero ultimate. A normal level lasts approximately two to five minutes.

## Release content target

- 30 campaign levels across Forest, Desert, and Frozen Kingdom regions
- 4 heroes: Warrior, Ranger, Mage, and Guardian
- 8 unit families: Archer, Knight, Mage, Spearman, Bomber, Priest, Engineer, and Assassin
- 5 elements: Fire, Ice, Lightning, Poison, and Wind
- 16 final evolution branches
- 6 mechanically distinct bosses
- 8–10 visible synergies
- 30–40 temporary battle upgrades
- 5 kingdom buildings
- Campaign, endless survival, collection, daily rewards, quests, and offline income

## Core loop

Start level → recruit and place units → defeat waves → earn coins → merge units → choose upgrades → evolve units → defeat boss → receive rewards → upgrade kingdom and heroes → unlock the next stage.

## Product principles

1. A first-time player understands the primary action within 30 seconds.
2. Controls remain limited to tap, drag, choose, and ultimate.
3. Depth comes from combinations, not menu complexity.
4. Progress must remain possible without watching ads or paying.
5. Bosses require readable mechanics, not merely larger health pools.
6. All balance and content data should be editable without changing combat code.

## Technical direction

- Unity 6 LTS and Universal Render Pipeline
- ScriptableObject content definitions
- Focused runtime systems rather than a monolithic manager
- Object pooling for enemies, projectiles, VFX, and damage indicators
- Versioned, centralized save model with migration and corrupted-save fallback
- Abstract analytics, advertisements, cloud save, and platform services
- Development mock advertisements until production monetization is approved
- Automated EditMode and PlayMode tests plus device QC

## Required player-facing screens

Boot, kingdom/home, world map, hero selection, battle, upgrade selection, evolution selection, victory, defeat, collection, quests, daily rewards, settings, privacy/consent, and support/legal.

## Monetization boundary

Rewarded advertisements may offer an optional reward multiplier, revive, offline-income multiplier, or bonus chest. Advertising must never be required to progress. Production SDK integration occurs only after the core game is stable and requires privacy consent, store declarations, and verified test ads before release configuration.

## Definition of complete

The game is complete only when every release gate passes, both store builds are produced, no game-breaking issue remains, progression and saves survive upgrade testing, all content counts are met or explicitly approved, store metadata and privacy disclosures are ready, and the owner accepts the release candidate after device playthrough.

