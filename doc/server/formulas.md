Formulas
=============================================================================

## STR

New Formula:
+2 ATK per 1 STR,
+5% ATK per 10 STR,
+20 Weight per 1 STR

Old Formula: 
+2 ATK per 1 STR, 
+5 ATK per 10 STR, 
+20 Weight per 1 STR
+0.5 BLK_BREAK per 1 STR,
+3 BLK_BREAK per 15 STR,

## CON

New Formula:
+0.3% MHP per 1 CON, 
+1% MHP per 10 CON, 
+2 DEF per 1 CON, 
+2 MDEF per 1 CON, 
+5% DEF per 10 CON, 
+5% MDEF per 10 CON, 
+20 Weight per 1 CON

Old Formula: 
+0.5% MHP per 1 CON, 
+1.5% MHP per 10 CON, 
+2 RHP per 1 CON, 
+3 RHP per 5 CON, 
+1 STA per 20 CON, 
+0.5 BLK per 1 CON, 
+3 BLK per 15 CON, 
+20 Weight per 1 CON

## INT

New Formula:
+2 MATK per 1 INT,
+5% MATK per 10 INT,

Old Formula:
+2 MATK per 1 INT,
+5 MATK per 10 INT,
+1.2 HEAL_PWR per 1 INT,
+10 HEAL_PWR per 34 INT,

## SPR

New Formula:
+1 HEAL_PWR per 1 SPR,
+3% HEAL_PWR per 10 SPR,

Old Formula:
+0.5% MSP per 1 SPR
+1.5% MSP per 10 SPR
+2.8 HEAL_PWR per 1 SPR,
+10 HEAL_PWR per 15 SPR,
+2 RSP per 1 SPR
+3 RSP per 5 SPR

## DEX

New Formula:
ASPD = ( DEX / 1000 ) ^ 0.5,
+2 CRTATK per 1 DEX,
+5% CRTATK per 10 DEX,

Old Formula:
ASPD = ( DEX / 750 ) ^ 0.75,
+4 CRTATK per 1 DEX,
+10 CRTATK per 10 DEX,
+0.5 Acc per 1 DEX, 
+3 Acc per 10 DEX,
+0.5 DR per 1 DEX,
+3 DR per 15 DEX,

## Defense

New Formula:
damage = (% increase factor) x attack x min {1, log10 ((attack / (defense + 1))^0.8 + 1)} + (+ attack increase) x modifiers

Old Formula:
damage = (% increase factor) x (attack + (+ attack increase) - defense) x modifiers