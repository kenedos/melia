# Quest Reward Tables

This document contains the authoritative reward tables for Laima quests. These values are balanced against monster grinding rates (8 seconds per kill, 15-50 kills per quest, with 1.20× bonus for quest effort).

---

## EXP Card Reference

| Card Level | Item ID | Base EXP | Job EXP |
|------------|---------|----------|---------|
| Lv1 | 640080 | 500 | 333 |
| Lv2 | 640081 | 2,686 | 1,791 |
| Lv3 | 640082 | 8,442 | 5,628 |
| Lv4 | 640084 | 22,860 | 15,240 |
| Lv5 | 640085 | 24,571 | 16,381 |
| Lv6 | 640086 | 60,312 | 40,208 |

---

## Quest Rewards by Level Threshold

### LEVEL 1-10 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 200/135 | 2× Lv1 (640080) | 2,500 |
| Moderate | 350/235 | 3× Lv1 (640080) | 3,500 |
| Complex | 475/320 | 4× Lv1 (640080) | 4,500 |

### LEVEL 10-20 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 500/340 | 1× Lv2 (640081) | 5,000 |
| Moderate | 1,200/800 | 2× Lv2 (640081) | 6,500 |
| Complex | 1,600/1,100 | 3× Lv2 (640081) | 8,000 |

### LEVEL 20-30 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 1,000/700 | 2× Lv2 (640081) | 9,000 |
| Moderate | 1,550/1,090 | 1× Lv3 (640082) | 11,500 |
| Complex | 3,100/2,200 | 2× Lv3 (640082) | 15,000 |

### LEVEL 30-40 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 1,900/1,430 | 1× Lv3 (640082) | 13,000 |
| Moderate | 3,800/2,700 | 2× Lv3 (640082) | 16,000 |
| Complex | 4,200/3,200 | 3× Lv3 (640082) | 24,000 |

### LEVEL 40-50 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 3,900/2,700 | 1× Lv4 (640084) | 21,000 |
| Moderate | 6,100/4,200 | 2× Lv4 (640084) | 29,000 |
| Complex | 8,700/6,000 | 3× Lv4 (640084) | 36,000 |

### LEVEL 50-60 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 15,600/10,800 | 1× Lv5 (640085) | 32,000 |
| Moderate | 11,000/7,500 | 2× Lv5 (640085) | 45,000 |
| Complex | 15,600/10,800 | 3× Lv5 (640085) | 57,000 |

### LEVEL 60-70 QUESTS

| Complexity | Direct EXP (base/job) | EXP Cards | Silver |
|------------|----------------------|-----------|--------|
| Simple | 11,900/8,100 | 1× Lv6 (640086) | 60,000 |
| Moderate | 23,800/16,200 | 2× Lv6 (640086) | 68,000 |
| Complex | 26,400/18,000 | 2× Lv6 (640086) | 75,000 |

---

## Potion Rewards by Level

### LEVEL 1-14 QUESTS (Small Potions)

| Level Range | HP Potion | SP Potion | Stamina Potion | Amount |
|-------------|-----------|-----------|----------------|--------|
| 1-14 | 640002 | 640005 | 640008 | 8-12 |

### LEVEL 15-40 QUESTS (Normal Potions)

| Level Range | HP Potion | SP Potion | Stamina/Recovery Potion | Amount |
|-------------|-----------|-----------|------------------------|--------|
| 15-29 | 640003 | 640006 | 640009 | 8-10 |
| 30-40 | 640003 | 640006 | 640011 (Recovery) | 10-15 |

### LEVEL 41+ QUESTS (Large Potions)

| Level Range | HP Potion | SP Potion | Recovery Potion | Amount |
|-------------|-----------|-----------|----------------|--------|
| 41-45 | 640004 | 640007 | 640012 | 8-10 |
| 46-60 | 640004 | 640007 | 640012 | 10-13 |
| 61+ | 640004 | 640007 | 640013 | 13-15 |

---

## Potion Distribution Rules

1. **ALWAYS award HP and SP potions** in every quest
2. **Stamina/Recovery potions**: Award occasionally (not every quest, roughly 50-70% of quests)
3. **Level 30+ quests**: Award Recovery potions in ~30% of quests, Stamina potions in ~40% of quests (remaining ~30% award neither)
4. **Amount resets at tier changes**: When potion tier increases (14→15, 40→41), start at lower amounts (8-10)
5. **Amount increases gradually**: As you approach the next tier, increase to 12-15

---

## City-Specific Potion Distribution

- **Klaipeda**: Even HP/SP distribution (equal amounts)
- **Orsha**: Slightly more HP potions (+2 HP, -2 SP from baseline)
- **Fedimian**: Slightly more SP potions (+2 SP, -2 HP from baseline)

---

## Example Reward Code

### Level 10 Quest (Klaipeda - even distribution)
```csharp
AddReward(new ExpReward(200, 135));
AddReward(new SilverReward(3500));
AddReward(new ItemReward(640080, 2)); // Lv1 EXP Cards
AddReward(new ItemReward(640002, 10)); // Small HP potion
AddReward(new ItemReward(640005, 10)); // Small SP potion
AddReward(new ItemReward(640008, 3));  // Small Stamina (occasional)
```

### Level 20 Quest (Orsha - more HP)
```csharp
AddReward(new ExpReward(500, 340));
AddReward(new SilverReward(9000));
AddReward(new ItemReward(640081, 1)); // Lv2 EXP Cards
AddReward(new ItemReward(640003, 10)); // Normal HP potion
AddReward(new ItemReward(640006, 8));  // Normal SP potion
```

### Level 35 Quest (Fedimian - more SP, with Stamina - 40% of quests)
```csharp
AddReward(new ExpReward(1550, 1090));
AddReward(new SilverReward(23000));
AddReward(new ItemReward(640082, 1)); // Lv3 EXP Cards
AddReward(new ItemReward(640003, 10)); // Normal HP potion
AddReward(new ItemReward(640006, 12)); // Normal SP potion
AddReward(new ItemReward(640009, 3));  // Stamina potion (40% of lvl 30+ quests)
```

### Level 35 Quest (Fedimian - more SP, with Recovery - 30% of quests)
```csharp
AddReward(new ExpReward(1550, 1090));
AddReward(new SilverReward(23000));
AddReward(new ItemReward(640082, 1)); // Lv3 EXP Cards
AddReward(new ItemReward(640003, 10)); // Normal HP potion
AddReward(new ItemReward(640006, 12)); // Normal SP potion
AddReward(new ItemReward(640011, 5));  // Recovery potion (30% of lvl 30+ quests)
```

### Level 50 Quest (Klaipeda - even distribution, with Recovery)
```csharp
AddReward(new ExpReward(6100, 4200));
AddReward(new SilverReward(58000));
AddReward(new ItemReward(640082, 3)); // Lv3 EXP Cards
AddReward(new ItemReward(640004, 12)); // Large HP potion
AddReward(new ItemReward(640007, 12)); // Large SP potion
AddReward(new ItemReward(640012, 4));  // Recovery potion (30% of quests)
```

---

## Complexity Guidelines

**IMPORTANT**: Quest complexity is NOT determined solely by kill count or objective count.

**Quick Reference:**
- **Simple**: Monsters near NPC, high spawn density, single map, 1 objective, straightforward dialog
- **Moderate**: Moderate distances, medium spawn density, 2 objectives, OR simple quest with complicating factors (scattered spawns, long travel)
- **Complex**: Long distances or multi-map, low spawn density, 3+ objectives, quest prerequisites, OR moderate quest with multiple complicating factors

**Complexity can increase** due to:
- Monster spawn distance from quest giver
- Low monster spawn density requiring extensive searching
- Collection points spread across large areas
- Multi-map travel or city-to-city deliveries
- Quest prerequisites creating narrative dependencies
- Multiple dialog branches or complex player choices
