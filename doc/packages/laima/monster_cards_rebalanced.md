# Rebalanced Monster Cards Design Document

**Design Principles:**
- **RED (ATK)**: Damage effects only (+% damage, +ATK, +CRIT damage, damage conditionals)
- **GREEN (STAT)**: Stat bonuses only (+STR, INT, DEX, CON, SPR)
- **BLUE (DEF)**: Defense effects only (-% damage taken, +DEF, +MDEF, +HP%, property resistances)
- **PURPLE (UTIL)**: Utility effects (debuff resistance, recovery, on-hit effects, misc bonuses)

**Scaling Convention:** ★ = card star level (1-10)

---

## RED GROUP (ATK) - Damage Cards

### Property Damage Bonuses
- **Achat** (ID: 53): Increases [★ * 2.5]% damage against Lightning property monsters
- **Bearkaras** (ID: 34): Increases [★ * 2.5]% damage against Earth property monsters
- **Ginklas** (ID: 7): Increases [★ * 2.5]% damage against Fire property monsters
- **Clymen** (ID: 67): Increases [★ * 2.5]% damage against Poison property monsters
- **Chapparition** (ID: 62): Increases [★ * 2.5]% damage against Holy property monsters
- **Merge** (ID: 28): Increases [★ * 2.5]% damage against Ice property monsters
- **Unicorn** (ID: 58): Increases [★ * 2.5]% damage against Dark property monsters

### Race/Type Damage Bonuses
- **Chafer** (ID: 63): Increases [★ * 2.5]% damage against Insect-type monsters
- **Devilglove** (ID: 11): Increases [★ * 2.5]% damage against Mutant-type monsters
- **Giant Wood Goblin** (ID: 61): Increases [★ * 2.5]% damage against Plant-type monsters
- **Moa** (ID: 27): Increases [★ * 2.5]% damage against Devil-type monsters
- **Vubbe Fighter** (ID: 36): Increases [★ * 2.5]% damage against Beast-type monsters
- **Grinender** (ID: 5): Increases [★ * 2.5]% damage against Flying-type monsters
- **Iltiswort** (ID: 59): Increases [★ * 2.5]% damage against Plant-type monsters
- **Giant Red Wood Goblin** (ID: 60): Increases [★ * 2.5]% damage against Cloth-armor type monsters
- **Corrupted** (ID: 79): Increases [★ * 2.5]% damage against Holding-type monsters

### Size Damage Bonuses
- **Mirtis** (ID: 32): Increases [★ * 1.5]% damage against Small-sized monsters
- **Moyabruka** (ID: 28): Increases [★ * 1.5]% damage against Medium-sized monsters
- **Reaverpede** (ID: 17): Increases [★ * 1.5]% damage against Medium-sized monsters
- **Moldyhorn** (ID: 29): Increases [★ * 1.5]% damage against Large-sized monsters
- **Kepa Chieftain** (ID: 65): Increases [★ * 1.5]% damage against Large-sized monsters

### Conditional Damage (Debuffed Targets)
- **Ironbaum** (ID: 49): Increases [★ * 2]% damage against targets with Sleep debuff
- **Hydra** (ID: 77): Increases [★ * 2]% damage against targets with Freeze debuff
- **Prison Cutter** (ID: 123): Increases [★ * 2]% damage against targets with Bleed debuff
- **Velnia Monkey** (ID: 124): Increases [★ * 2]% damage against targets with Poison debuff
- **Mandara** (ID: 104): Increases [★ * 2]% damage against targets with Knockdown debuff
- **Gaigalas** (ID: 1): Increases [★ * 2]% damage against targets with Stun debuff
- **Necrovanter** (ID: 9): Increases [★ * 2]% damage against targets with Blind debuff

### Weapon-Type Damage Bonuses
- **Rajatoad** (ID: 14): Increases [★ * 1]% damage with Pierce-type weapons
- **Mothstem** (ID: 26): Increases [★ * 1]% magic damage with Strike-type weapons
- **Red Vubbe Fighter** (ID: 37): Increases [★ * 1]% damage with Strike-type weapons
- **Gremlin** (ID: 98): Increases [★ * 1]% damage with Slash-type weapons
- **Sparnas** (ID: 81): Increases [★ * 1]% damage with Missile weapons (Arrow/Cannon/Gun)

### Conditional Damage (Potion-Triggered)
- **Glass Mole** (ID: 6): On SP potion use: Physical Attack +[★ * 1.5]% for 20 seconds
- **Merregina** (ID: 114): On HP potion use: Physical Attack +[★ * 1.5]% for 20 seconds
- **Honeypin** (ID: 74): On SP potion use: Magical Attack +[★ * 1.5]% for 20 seconds
- **LithoRex** (ID: 105): On HP potion use: Magical Attack +[★ * 1.5]% for 20 seconds

### Critical Attack Bonuses
- **Deadborn** (ID: 10): Critical Attack (Physical) +[★ * 1.5]%
- **Rexipher** (ID: 16): Critical Attack (Magical) +[★ * 1.5]%

### General Damage Bonuses
- **Yekub** (ID: 55): Physical and Magical Attack +[★ * 0.5]%
- **Fire Lord** (ID: 116): Physical and Magical Attack +[★ * 0.5]%
- **Archon** (ID: 50): Magic and Physical Attack +[★ * 0.3]%
- **Helgasercle** (ID: 75): Magic Attack +[★ * 0.5]%
- **Plokste** (ID: 99): Physical Attack +[★ * 0.5]%
- **Master Genie** (ID: 113): Physical Attack +[★ * 0.5]%

### Special Damage Mechanics
- **Tomb Lord** (ID: 70): Increases [★ * 1.5]% damage against Boss monsters
- **Centaurus** (ID: 107): AoE Attack Ratio +[★ * 0.2]
- **Froster Lord** (ID: 132): On attack vs Boss: [★ * 0.75]% chance to apply Frostbite for 4 seconds

---

## BLUE GROUP (DEF) - Defense Cards

### Property Damage Resistance
- **Saltistter** (ID: 41): Ice property damage taken -[★ * 2.5]%
- **Colimencia** (ID: 66): Poison property damage taken -[★ * 2.5]%
- **Denoptic** (ID: 12): Holy property damage taken -[★ * 2.5]%
- **Shnayim** (ID: 45): Lightning property damage taken -[★ * 2.5]%
- **Flammidus** (ID: 119): Fire property damage taken -[★ * 2.5]%
- **Gorgon** (ID: 3): Dark property damage taken -[★ * 2.5]%
- **Bramble** (ID: 38): Earth damage type taken -[★ * 2.5]%
- **Sparnasman** (ID: 82): Dark damage type taken -[★ * 2.5]%
- **Yonazolem** (ID: 56): Holy damage type taken -[★ * 2.5]%

### Physical/Magical Defense
- **Zaura** (ID: 120): Physical Defense +[★ * 1]%
- **Nuaele** (ID: 92): Magical Defense +[★ * 1]%
- **Gray Golem** (ID: 87): Physical Defense +[★ * 1]% 
- **Sequoia** (ID: 72): Physical Defense +[★ * 0.5]%, Magical Defense +[★ * 0.5]%

### Damage Type Resistance
- **Scorpio** (ID: 46): Pierce damage type taken -[★ * 1.5]%
- **Glackuman** (ID: 95): Slash damage type taken -[★ * 1.5]%
- **Golem** (ID: 4): Strike damage type taken -[★ * 1.5]%

### HP/Survivability
- **Woodspirit** (ID: 125): Max HP +[★ * 0.8]%

### Shield/Defensive Mechanics
- **Armaos** (ID: 126): When attacked: 10% chance to create shield ([★ * 150] value, 10s duration)
- **Progola** (ID: 109): On HP potion use: Magical Defense +[★ * 1.5]% for 20 seconds

### Healing Power
- **Varle King** (ID: 144): Healing Power +[★ * 7]

---

## GREEN GROUP (STAT) - Stat Bonus Cards

### Single Primary Stats
- **Minotaur** (ID: 31): STR +[★ * 5]
- **Blut** (ID: 94): CON +[★ * 5]
- **Kubas** (ID: 117): INT +[★ * 5]
- **Unknocker** (ID: 52): SPR +[★ * 5]
- **Netherbovine** (ID: 8): DEX +[★ * 5]

### Dual Stats
- **Abomination** (ID: 51): DEX +[★ * 3], CON +[★ * 3]
- **Mummyghast** (ID: 23): STR +[★ * 3], DEX +[★ * 3]
- **Simorph** (ID: 86): SPR +[★ * 3], INT +[★ * 3]
- **Wood Houngan** (ID: 100): INT +[★ * 3], CON +[★ * 3]
- **Templeshooter** (ID: 115): STR +[★ * 3], SPR +[★ * 3]
- **Glacia** (ID: 180): CON +[★ * 3], SPR +[★ * 3]
- **Skiaclipse** (ID: 150): STR +[★ * 3], INT +[★ * 3]
- **Tantalizer** (ID: 164): CON +[★ * 3], STR +[★ * 3]

### All Stats
- **Rafene** (ID: 129): All stats (STR/CON/INT/SPR/DEX) +[★ * 1.2]
- **Pajauta** (ID: 149): All stats (STR/CON/INT/SPR/DEX) +[★* 1.2]
- **Uska** (ID: 167): All stats (STR/CON/INT/SPR/DEX) +[★ * 1.2]
- **Neringa** (ID: 170): All stats (STR/CON/INT/SPR/DEX) +[★ * 1.2]

### Potion-Enhanced Stats
- **Stone Froster** (ID: 128): On HP potion use: CON +[★ * 10] for 30 seconds
- **Lavenzard** (ID: 130): On SP potion use: SPR +[★ * 10] for 30 seconds
- **Pyroego** (ID: 103): On HP potion use: STR +[★ * 10] for 30 seconds
- **Linkroller** (ID: 89): On SP potion use: INT +[★ * 10] for 30 seconds
- **Ellaganos** (ID: 54): On HP potion use: DEX +[★ * 10] for 30 seconds

---

## PURPLE GROUP (UTIL) - Utility Cards

### Debuff Resistances
#### High Priority Debuffs (8% per star)
- **Riteris** (ID: 91): Blind resistance +[★ * 8]%
- **Deathweaver** (ID: 118): Slow/Lethargy resistance +[★ * 8]%
- **Poata** (ID: 71): Bleed resistance +[★ * 8]%
- **Biteregina** (ID: 39): Poison resistance +[★ * 8]%
- **Salamander** (ID: 42): Burn resistance +[★ * 8]%

#### Medium Priority Debuffs
- **Bebraspion** (ID: 33): Silence resistance +[★ * 7]%
- **Capria** (ID: 84): Sleep resistance +[★ * 7]%
- **Carapace** (ID: 64): Freeze resistance +[★ * 7]%
- **Stone Whale** (ID: 44): Stun resistance +[★ * 7]%

#### Low Priority Debuffs
- **Rocktortuga** (ID: 15): Knockback/Knockdown resistance +[★ * 5]%

### Dual Debuff Resistance
- **Shadowgaler** (ID: 43): Silence and Blind Resistnace +[★ * 4]%
- **Tetraox** (ID: 96): Bleed and Poison Resistance +[★ * 4]%
- **Nuodai** (ID: 106): Silence and Sleep Resistance +[★ * 3.5]%
- **Crabil** (ID: 112): Freeze and Stun Resistance +[★ * 3.5]%

### Recovery Effects
#### HP Recovery
- **Velnewt** (ID: 101): When attacked by Small monster: [★ * 1]% chance to recover 5% HP (15s cooldown)
- **Canceril** (ID: 111): When attacked by Medium monster: [★ * 1]% chance to recover 5% HP (15s cooldown)

#### SP Recovery
- **Mallet Wyvern** (ID: 20): When attacked by Large monster: [★ * 1]% chance to recover 5% SP (5s cooldown)
- **Cerberus** (ID: 83): When attacked by Medium monster: [★ * 1]% chance to recover 5% SP (5s cooldown)
- **Magburk** (ID: 21): On kill Mutant-type: [★ * 1]% chance to recover 5% SP
- **Specter Monarch** (ID: 47): On kill Dark property: [★ * 1]% chance to recover 5% SP
- **Throneweaver** (ID: 48): On kill Poison property: [★ * 1]% chance to recover 5% SP
- **Harpeia** (ID: 73): On kill Lightning property: [★ * 1]% chance to recover 5% SP
- **Mushcaria** (ID: 24): SP Recovery +[★ * 6]

### Potion Effectiveness
- **Ravinepede** (ID: 13): Stamina potion effectiveness +[★ * 2]%
- **Rikaus** (ID: 18): SP potion effectiveness +[★ * 2]%
- **Neop** (ID: 93): HP potion effectiveness +[★ * 2]%

### Potion-Triggered Buffs
- **Manticen** (ID: 22): On Stamina potion use: Movement Speed +[★ * 0.3] for 20 seconds

### Debuff Application (On Attack)
- **Mineloader** (ID: 19): On attack: [★ * 0.5]% chance to apply Electric Shock for 3 seconds
- **Rajapearl** (ID: 78): On attack: [★ * 0.5]% chance to apply Bleed for 6 seconds
- **Marionette** (ID: 110): On attack: [★ * 0.5]% chance to apply Slow for 3 seconds

### Self Buffs
- **Velpede** (ID: 102): On attack: [★ * 0.5]% chance to gain Haste buff (+4 Movement Speed, 6s)

### Combat Stats
- **Molich** (ID: 30): Block +[★ * 2]
- **Velorchard** (ID: 35): Evasion +[★ * 2]
- **Kirmeleech** (ID: 68): Block Penetration +[★ * 2]
- **Naktis** (ID: 88): Accuracy +[★ * 2]
- **Marnox** (ID: 121): Critical Rate +[★]
- **Basilisk** (ID: 97): Critical Resistance +[★]

### Aggro/Hate Management
- **Ferret Marauder** (ID: 122): Aggro generation +[★ * 2]%
- **Succubus** (ID: 127): Aggro generation -[★ * 2]%

### Special Mechanics
- **Dullahan** (ID: 90): On death: [★ * 1]% chance to revive with 10% HP
- **Cyclops** (ID: 40): Max Stamina +[★ * 2]
- **Tutu** (ID: 69): Max Weight +[★ * 2]%
- **Lepus** (ID: 80): When one-handed weapon equipped: Attack Speed +[★ * 1]
- **Specter of Deceit** (ID: 76): Max SP +[★ * 0.8]%
- **Werewolf** (ID: 57): [★ * 1]% chance for attacks to be treated as back attacks
- **Yeti** (ID: 108): Max HP +[★ * 0.8]%
- **Nepenthes** (ID: 85): Max HP +[★ * 0.4]%, Max SP +[★ * 0.4]%
- **Gazing Golem** (ID: 2): When attacked: [★ * 1]% chance to activate Pain Barrier (15s duration)

### Misc Utility (Cards that don't fit elsewhere)
- **Moringponia** (ID: 160): Max HP +[★ * 100]
- **Misrus** (ID: 162): Max SP +[★ * 60]
- **Cyrenia** (ID: 168): Max Stamina +[★ * 1], Max HP +[★ * 60]
- **Gorkas** (ID: 131): Looting Chance +[★ * 7]

---
