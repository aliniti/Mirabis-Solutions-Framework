using System.Collections.Generic;

namespace Miracle_Business_Solutions_Framework.Base
{
    internal class Lists
    {
        ////////////PVP/////////////
        
        #region Category : PvP Migigate Stun

        internal static readonly HashSet<int> MigateStun = new HashSet<int>
        {
            	110698,		// Hammer of Justice (Paladin)
					1330,		// Garrote - Silence
					108194,		// Asphyxiate
					22570,		// Maim
					5211,		// Mighty Bash
					9005,		// Pounce
					102546,		// Pounce (Incarnation)
					91800,		// Gnaw
					91797,		// Monstrous Blow (Dark Transformation)
					44572,		// Deep Freeze
					119381,		// Leg Sweep
					105593,		// Fist of Justice
					853,		// Hammer of Justice
					1833,		// Cheap Shot
					408,		// Kidney Shot
					30283,		// Shadowfury
					89766,		// Axe Toss (Felguard/Wrathguard)
					7922,		// Charge Stun
					132168,		// Shockwave
					105771		// Warbringer
        };
        #endregion

        #region Category : PvP Juking

        internal static readonly HashSet<int> PvPJukes = new HashSet<int>
        {
            102060, //Disrupting Shout
            106839, //Skull Bash
            80964, //Skull Bash
            115781, //Optical Blast
            116705, //Spear Hand Strike
            1766, //Kick
            19647, //Spell Lock
            2139, //Counterspell
            47476, //Strangulate
            47528, //Mind Freeze
            57994, //Wind Shear
            6552, //Pummel
            96231, //Rebuke
            147362 //Counter shot coming in 5.4
        };
        #endregion

        #region Category : PvP AllSrs

        /// <summary>
        ///
        /// </summary>
        internal static readonly HashSet<int> PvPAllSerious = new HashSet<int> {
            96294, // Chains of Ice (Chilblains)
            45524, // Chains of Ice
            50435, // Chilblains
            115001, // Remorseless Winter
            115000, // Remorseless Winter
            91800, // Gnaw
            91797, // Monstrous Blow (Dark Transformation)
            91807, // Shambling Rush (Dark Transformation)
            102795, // Bear Hug
            22570, // Maim
            5211, // Mighty Bash
            9005, // Pounce
            102546, // Pounce (Incarnation)
            339, // Entangling Roots
            19975, // Entangling Roots (Nature's Grasp)
            45334, // Immobilized (Wild Charge - Bear)
            102359, // Mass Entanglement
            50259, // Dazed (Wild Charge - Cat)
            58180, // Infected Wounds
            61391, // Typhoon
            127797, // Ursol's Vortex
            110698, // Hammer of Justice (Paladin)
            110693, // Frost Nova (Mage)
            110610, // Ice Trap (Hunter)
            117526, // Binding Shot
            19185, // Entrapment
            128405, // Narrow Escape
            35101, // Concussive Barrage
            5116, // Concussive Shot
            61394, // Frozen Wake (Glyph of Freezing Trap)
            13810, // Ice Trap
            50245, // Pin (Crab)
            54706, // Venom Web Spray (Silithid)
            4167, // Web (Spider)
            50433, // Ankle Crack (Crocolisk)
            54644, // Frost Breath (Chimaera)
            118271, // Combustion Impact
            44572, // Deep Freeze
            122, // Frost Nova
            111340, // Ice Ward
            11113, // Blast Wave - gone?
            121288, // Chilled (Frost Armor)
            120, // Cone of Cold
            116, // Frostbolt
            44614, // Frostfire Bolt
            113092, // Frost Bomb
            31589, // Slow
            33395, // Freeze
            126451, // Clash
            122242, // Clash (not sure which one is right)
            119392, // Charging Ox Wave
            119381, // Leg Sweep
            116706, // Disable
            113275, // Entangling Roots (Symbiosis)
            123407, // Spinning Fire Blossom
            116095, // Disable
            118585, // Leer of the Ox
            123727, // Dizzying Haze
            123586, // Flying Serpent Kick
            105593, // Fist of Justice
            853, // Hammer of Justice
            110300, // Burden of Guilt
            63529, // Dazed - Avenger's Shield
            20170, // Seal of Justice
            113275, // Entangling Roots (Symbiosis)
            87194, // Glyph of Mind Blast
            114404, // Void Tendril's Grasp
            15407, // Mind Flay
            1833, // Cheap Shot
            408, // Kidney Shot
            115197, // Partial Paralysis
            3409, // Crippling Poison
            26679, // Deadly Throw
            119696, // Debilitation
            77505, // Earthquake
            118905, // Static Charge (Capacitor Totem)
            64695, // Earthgrab (Earthgrab Totem)
            63685, // Freeze (Frozen Power)
            3600, // Earthbind (Earthbind Totem)
            77478, // Earthquake (Glyph of Unstable Earth)
            8034, // Frostbrand Attack
            8056, // Frost Shock
            51490, // Thunderstorm
            30283, // Shadowfury
            18223, // Curse of Exhaustion
            47960, // Shadowflame
            89766, // Axe Toss (Felguard/Wrathguard)
            7922, // Charge Stun
            118895, // Dragon Roar
            132168, // Shockwave
            105771, // Warbringer
            107566, // Staggering Shout
            1715, // Hamstring
            12323 // Piercing Howl
        };

        #endregion Category: PvP AllSrs

        #region Category : PvP AttentionBuffs Buffs

        /// <summary>
        ///
        /// </summary>
        internal static readonly HashSet<int> PvPAttentionBuffs = new HashSet<int> {
            107574, //Avatar
            123904, // Invoke Xuen, the White Tiger
            51713, // Shadow Dance
           // 1719, // Recklessness we can soothe
            51271, // Pillar of Frost
            //49016, // Unholy Frenzy we can soothe
            114049, // Ascendance
            31884, // Avenging Wrath
            12472, // Icy Veins
            12042, // Arcane Power
            113860, // Dark Soul: Misery
            113861, // Dark Soul: Knowledge
            113858, // Dark Soul: Instability
            102560, // Incarnation: Chosen of Elune
            102543, // Incarnation: King of the Jungle
            33891, // Incarnation: Tree of Life
            34692 // The Beast Within
        };

        #endregion Category : PvP AttentionBuffs Buffs

        #region Category : PvP ccthatcanbreak

        /// <summary>
        /// Credits to cokx
        /// </summary>
        internal static readonly HashSet<int> PvPCcBreakables = new HashSet<int>
        {
            28272, // Pig Poly  (cast)
            118, // Sheep Poly  (cast)
            61305, // Cat Poly  (cast)
            61721, // Rabbit Poly  (cast)
            61780, // Turkey Poly  (cast)
            28271, // Turtle Poly  (cast)
            20066, //Repentance  (cast)
            115078, // Paralysis
            104045, // Sleep (Metamorphosis)
            115268, // Mesmerize (Shivarra)
            82691, // Ring of Frost
            6358, // Seduction (Succubus)
            2094, // Blind
            115750, // Blinding Light
            6770, // Sap
            2637, // Hibernate
            113056, // Intimidating Roar
            3355, // Freezing Trap
            19503, // Scatter Shot
            19386, // Wyvern Sting
            126246, // Lullaby
            90337, // Bad Manner
            24394, // Intimidation
            126355, // Paralyzing Quill
            126423, // Petrifying Gaze
            50519, // Sonic Blast
            56626, // Sting
            96201, // Web Wrap
            82691, // Ring of Frost
            9484, // Shackle Undead
            88625, // Holy Word: Chastise
            115268, // Mesmerize
            6358, // Seduction
            20511 // Intimidating Shout
        };

        #endregion Category : Breakables

        #region Category : PvP Disarm Damage

        internal static readonly HashSet<int> PvPDisarmDamage = new HashSet<int> {
            51271, // Pillar of Frost
            49016, // Unholy Frenzy
            13750, // Adrenaline Rush
            79140, // Vendetta
            51713, // Shadow Dance
            34692, // Beast Within
            121471, // Shadow Blades
            31884, //  Avenging Wrath
            107574, //  Avatar
            12292, // Bloodbath
            1719, // Recklessness
            114049 // Ascendance
        };

        #endregion Category : PvP Disarm Damage

        #region Category : PvP Fears

        internal static readonly HashSet<int> PvPFears = new HashSet<int> {
            5782, //fear
            8122, //psychic scream
            5484, //Howl of terror
            5246, //Intimidating shout
            64044, //Psychic horror
            111397, //Blood fear
            113792, //Pcychic terror
            16508 //Intimidating roar
        };

        #endregion Category : PvP Fears

        #region Category : PvP KillableTotems

        internal static readonly HashSet<int> PvPKillableTotems = new HashSet<int> {
            53006, //
            60561, //
            2630, //
            59717, //
            10467, //
            59764, //
            61245 //
        };

        #endregion Category : PvP KillableTotems

        #region Category : PvP Capturing

        internal static readonly HashSet<int> PvPCapturing = new HashSet<int> { 98322, 98323, 98324 };

        #endregion Category : PvP Capturing

        #region Category : PvP Stuns

        /// <summary>
        ///
        /// </summary>
        internal static readonly HashSet<int> PvPStuns = new HashSet<int> {
            110698, // Hammer of Justice (Paladin)
            1330, // Garrote - Silence
            108194, // Asphyxiate
            22570, // Maim
            5211, // Mighty Bash
            9005, // Pounce
            102546, // Pounce (Incarnation)
            91800, // Gnaw
            91797, // Monstrous Blow (Dark Transformation)
            44572, // Deep Freeze
            119381, // Leg Sweep
            105593, // Fist of Justice
            853, // Hammer of Justice
            1833, // Cheap Shot
            408, // Kidney Shot
            30283, // Shadowfury
            89766, // Axe Toss (Felguard/Wrathguard)
            7922, // Charge Stun
            132168, // Shockwave
            105771 // Warbringer
        };

        #endregion Category : PvP Stuns

        #region Category : PvP ImmuneDmg

        internal static readonly HashSet<int> PvPImuneDmg = new HashSet<int> {
            33786, // Cyclone
            113506, // Cyclone
            45438, // Ice Block
            110700, // Divine Shield (Paladin)
            110696, // Ice Block (Mage)
            19263, // Deterrence
            45438, // Ice Block
            122464, // Dematerialize
            //122470, // touch of karma
            642 // Divine Shield
        };

        #endregion Category : PvP ImmuneDmg

        #region Category : PvP ImmuneStun

        #endregion Category : PvP ImmuneStun

        #region Category : PvP ImmuneDispel

        internal static readonly HashSet<int> PvPImmuneDispel = new HashSet<int> {
            45438, // Ice Block
            110700, // Divine Shield (Paladin)
            110696, // Ice Block (Mage)
            45438, // Ice Block
            1022, //Hand of Protection
            642 // Divine Shield
        };

        #endregion Category : PvP ImmuneDispel

        #region Category : PvP ImmuneSlow

        internal static readonly HashSet<int> PvPImmuneSlow = new HashSet<int> {
            1044, //Hand of Freedom
            47585, //Dispersion
            46924, //Bladestorm
            34692, //Beast Within
            53271 //masters call
        };

        #endregion Category : PvP ImmuneSlow

        #region Category : PvP ImmuneSpell

        internal static readonly HashSet<int> PvPImmuneSpell = new HashSet<int> {//48707,	// Anti-Magic Shell
          48707,	// Anti-Magic Shell
110570,	// Anti-Magic Shell (Death Knight)
110788,	// Cloak of Shadows (Rogue)
113002,	// Spell Reflection (Warrior)
115760,	// Glyph of Ice Block
131523,	// Zen Meditation
114239,	// Phantasm
31224,	// Cloak of Shadows
8178,	// Grounding Totem Effect (Grounding Totem)
23920,	// Spell Reflection
114028	// Mass Spell Reflection
        };

        #endregion Category : PvP ImmuneSpell

        #region Category : PvP Polymorph
        /// <summary>
        ///     Credits to Cokx
        /// </summary>
        internal static readonly HashSet<int> PvPPolymorphs = new HashSet<int>
            {
                51514, // Hex
                118, // Polymorph
                61305, // Polymorph: Black Cat
                28272, // Polymorph: Pig
                61721, // Polymorph: Rabbit
                61780, // Polymorph: Turkey
                28271, // Polymorph: Turtle 
               // 605, // Dominate Mind
               // 8129 // Mana Burn

            };
        #endregion

        #region Category : PvP CC Reflect

        internal static readonly HashSet<int> PvPCCReflect = new HashSet<int>
            {
                5782, // Fear
                118699, // Fear
                118, // Polymorph
                61305, // Polymorph: Black Cat
                28272, // Polymorph: Pig
                61721, // Polymorph: Rabbit
                61780, // Polymorph: Turkey
                28271, // Polymorph: Turtle
                33786, // Cyclone
                113506, // Cyclone
                20066, // Repentance
                51514 // Hex
            };

        #endregion Category :  PvP CC Reflect

        #region Category : PvP DMG Reflect

        internal static readonly HashSet<int> PvPDMGReflect = new HashSet<int>
            {
             51505,	 	// Lava Burst
116858,		//Chaos Bolt
113092, 	//Frost Bomb
48181, 		//Haunt
78674,      // starsurge
102051 		//Frost Jaw
            };

        #endregion Category :  PvP CC Reflect

        #region Category : PVP Crowd Controlled

        internal static readonly HashSet<int> PvPCrowdControlled = new HashSet<int> {
            30217, //Adamantite Grenade
            89766, //Axe Toss (Felguard/Wrathguard)
            90337, //Bad Manner (Monkey)
            710, //Banish
            113801, //Bash (Force of Nature - Feral Treants)
            102795, //Bear Hug
            76780, //Bind Elemental
            117526, //Binding Shot
            2094, //Blind
            105421, //Blinding Light
            115752, //Blinding Light (Glyph of Blinding Light)
            123393, //Breath of Fire (Glyph of Breath of Fire)
            126451, //Clash
            122242, //Clash (not sure which one is right)
            67769, //Cobalt Frag Bomb
            118271, //Combustion Impact
            33786, //Cyclone
            113506, //Cyclone (Symbiosis)
            7922, //Charge Stun
            119392, //Charging Ox Wave
            1833, //Cheap Shot
            44572, //Deep Freeze
            54786, //Demonic Leap (Metamorphosis)
            99, //Disorienting Roar
            605, //Dominate Mind
            118895, //Dragon Roar
            31661, //Dragon's Breath
            77505, //Earthquake
            5782, //Fear
            118699, //Fear
            130616, //Fear (Glyph of Fear)
            30216, //Fel Iron Bomb
            105593, //Fist of Justice
            117418, //Fists of Fury
            3355, //Freezing Trap
            91800, //Gnaw
            1776, //Gouge
            853, //Hammer of Justice
            110698, //Hammer of Justice (Paladin)
            51514, //Hex
            2637, //Hibernate
            88625, //Holy Word: Chastise
            119072, //Holy Wrath
            5484, //Howl of Terror
            22703, //Infernal Awakening
            113056, //Intimidating Roar [Cowering in fear] (Warrior)
            113004, //Intimidating Roar [Fleeing in fear] (Warrior)
            5246, //Intimidating Shout (aoe)
            20511, //Intimidating Shout (targeted)
            24394, //Intimidation
            408, //Kidney Shot
            119381, //Leg Sweep
            126246, //Lullaby (Crane)
            22570, //Maim
            115268, //Mesmerize (Shivarra)
            5211, //Mighty Bash
            91797, //Monstrous Blow (Dark Transformation)
            6789, //Mortal Coil
            115078, //Paralysis
            113953, //Paralysis (Paralytic Poison)
            126355, //Paralyzing Quill (Porcupine)
            126423, //Petrifying Gaze (Basilisk)
            118, //Polymorph
            61305, //Polymorph: Black Cat
            28272, //Polymorph: Pig
            61721, //Polymorph: Rabbit
            61780, //Polymorph: Turkey
            28271, //Polymorph: Turtle
            9005, //Pounce
            102546, //Pounce (Incarnation)
            64044, //Psychic Horror
            8122, //Psychic Scream
            113792, //Psychic Terror (Psyfiend)
            118345, //Pulverize
            107079, //Quaking Palm
            13327, //Reckless Charge
            115001, //Remorseless Winter
            20066, //Repentance
            82691, //Ring of Frost
            6770, //Sap
            1513, //Scare Beast
            19503, //Scatter Shot
            132412, //Seduction (Grimoire of Sacrifice)
            6358, //Seduction (Succubus)
            9484, //Shackle Undead
            30283, //Shadowfury
            132168, //Shockwave
            87204, //Sin and Punishment
            104045, //Sleep (Metamorphosis)
            50519, //Sonic Blast (Bat)
            118905, //Static Charge (Capacitor Totem)
            56626, //Sting (Wasp)
            107570, //Storm Bolt
            10326, //Turn Evil
            20549, //War Stomp
            105771, //Warbringer
            19386, //Wyvern Sting
            108194, //Asphyxiate
        };

        #endregion Category : PVP Crowd Controlled

        #region Category : PvP ImmuneSlow

        internal static readonly HashSet<int> PvP = new HashSet<int> {
            1044, //Hand of Freedom
            47585, //Dispersion
            46924, //Bladestorm
            34692, //Beast Within
            53271 //masters call
        };

        #endregion Category : PvP ImmuneSlow

        #region Category : PvP DispelCurse

        internal static readonly HashSet<int> PvPDispelCurse = new HashSet<int> {
            51514 //Hex
        };

        #endregion Category : PvP DispelCurse

        #region Category : PvP DispelRoots

        /// <summary>
        ///
        /// </summary>
        internal static readonly HashSet<int> PvPDispelRoots = new HashSet<int> {
            96294, // Chains of Ice (Chilblains)
            339, // Entangling Roots
            19975, // Entangling Roots (Nature's Grasp)
            102359, // Mass Entanglement
            110693, // Frost Nova (Mage)
            19185, // Entrapment
            50245, // Pin (Crab)
            54706, // Venom Web Spray (Silithid)
            4167, // Web (Spider)
            122, // Frost Nova
            111340, // Ice Ward
            33395, // Freeze
            113275, // Entangling Roots (Symbiosis)
            123407, // Spinning Fire Blossom
            113275, // Entangling Roots (Symbiosis)
            87194, // Glyph of Mind Blast
            114404, // Void Tendril's Grasp
            115197, // Partial Paralysis
            64695, // Earthgrab (Earthgrab Totem)
            63685 // Freeze (Frozen Power)
        };

        #endregion Category : PvP DispelRoots

        #region Category : PvP DispelPoison

        internal static readonly HashSet<int> PvPDispelPoison = new HashSet<int> {
            19386, // Wyvern Sting
            113953 //// Paralysis (Paralytic Poison)
        };

        #endregion Category : PvP DispelPoison

        #region Category : PvP DispelSilence

        internal static readonly HashSet<int> PvPDispelSilence = new HashSet<int> {
            47476, // Strangulate
            114238, // Fae Silence (Glyph of Fae Silence)
            34490, // Silencing Shot
            102051, // Frostjaw (also a root)
            55021, // Silenced - Improved Counterspell
            31935, // Avenger's Shield
            15487, // Silence
            24259, // Spell Lock (Felhunter/Observer)
            25046, // Arcane Torrent (Energy)
            28730, // Arcane Torrent (Mana)
            50613, // Arcane Torrent (Runic Power)
            69179, // Arcane Torrent (Rage)
            80483, // Arcane Torrent (Focus)
            129597 // Arcane Torrent (Chi)
        };

        #endregion Category : PvP DispelSilence

        #region Category : PvP DispelMagic

        internal static readonly HashSet<int> PvPDispelMagic = new HashSet<int> {
            115001, // Remorseless Winter
            2637, // Hibernate
            110698, // Hammer of Justice (Paladin)
            117526, // Binding Shot
            3355, // Freezing Trap
            1513, // Scare Beast
            118271, // Combustion Impact
            44572, // Deep Freeze
            31661, // Dragon's Breath
            118, // Polymorph
            61305, // Polymorph: Black Cat
            28272, // Polymorph: Pig
            61721, // Polymorph: Rabbit
            61780, // Polymorph: Turkey
            28271, // Polymorph: Turtle
            82691, // Ring of Frost
            11129, // Combustion
            123393, // Breath of Fire (Glyph of Breath of Fire)
            115078, // Paralysis
            105421, // Blinding Light
            115752, // Blinding Light (Glyph of Blinding Light)
            105593, // Fist of Justice
            853, // Hammer of Justice
            119072, // Holy Wrath
            20066, // Repentance
            10326, // Turn Evil
            64044, // Psychic Horror
            8122, // Psychic Scream
            113792, // Psychic Terror (Psyfiend)
            9484, // Shackle Undead
            118905, // Static Charge (Capacitor Totem)
            5782, // Fear
            118699, // Fear
            5484, // Howl of Terror
            6789, // Mortal Coil
            30283, // Shadowfury
            104045, // Sleep (Metamorphosis)
            115268, // Mesmerize (Shivarra)
            113092, // Frost Bomb
            6358 // Seduction (Succubus)
        };

        #endregion Category : PvP DispelMagic

        #region Category : PvP dispelableEnrages

        internal static readonly HashSet<int> PvPdispelableEnrages = new HashSet<int> {
            //Death Knight
            49016, //http://www.wowhead.com/spell=49016 //Unholy FRenzy
            93099, //http://www.wowhead.com/spell=93099 //Vengeance
            //Druid
            5229, //http://www.wowhead.com/spell=5229 //Enrage
            52610, //http://www.wowhead.com/spell=52610 //Savage Roar
          //  48393, //http://www.wowhead.com/spell=48393 //Owlkin Frenzy
            84840, //http://www.wowhead.com/spell=84840 //Vengeance
            //Paladin
            84839, //http://www.wowhead.com/spell=84839 //Vengeance
            //Warrior
            18499, //http://www.wowhead.com/spell=18499 //Berserker Rage
            56611, //http://www.wowhead.com/spell=56611 //Wrecking Crew
            13046, //http://www.wowhead.com/spell=13046 //Enrage
            12292, //http://www.wowhead.com/spell=12292 //Bloodbath
            93098 //http://www.wowhead.com/spell=93098 //Vengeance
        };

        #endregion Category : PvP dispelableEnrages

        #region Category : PvP DamageCC

        internal static readonly HashSet<int> PvPDamageCC = new HashSet<int> {
            108194, // Asphyxiate
            115001, // Remorseless Winter
            91800, // Gnaw
            91797, // Monstrous Blow (Dark Transformation)
            102795, // Bear Hug
            22570, // Maim
            5211, // Mighty Bash
            9005, // Pounce
            102546, // Pounce (Incarnation)
            110698, // Hammer of Justice (Paladin)
            113004, // Intimidating Roar [Fleeing in fear] (Warrior)
            118271, // Combustion Impact
            44572, // Deep Freeze
            126451, // Clash
            122242, // Clash (not sure which one is right)
            119392, // Charging Ox Wave
            119381, // Leg Sweep
            105593, // Fist of Justice
            853, // Hammer of Justice
            88625, // Holy Word: Chastise
            64044, // Psychic Horror
            8122, // Psychic Scream
            113792, // Psychic Terror (Psyfiend)
            1833, // Cheap Shot
            408, // Kidney Shot
            113953, // Paralysis (Paralytic Poison)
            51514, // Hex
            118905, // Static Charge (Capacitor Totem)
            54786, // Demonic Leap (Metamorphosis)
            5782, // Fear
            118699, // Fear
            5484, // Howl of Terror
            6789, // Mortal Coil
            30283, // Shadowfury
            89766, // Axe Toss (Felguard/Wrathguard)
            7922, // Charge Stun
            118895, // Dragon Roar
            5246, // Intimidating Shout (aoe)
            132168, // Shockwave
            105771 // Warbringer
        };

        #endregion Category : PvP DamageCC

        #region Category : PvP DamageSilenceCC

        internal static readonly HashSet<int> PvPDmgSilenceCC = new HashSet<int> {
            47476, // Strangulate
            114238, // Fae Silence (Glyph of Fae Silence)
            81261, // Solar Beam
            102051, // Frostjaw (also a root)
            55021, // Silenced - Improved Counterspell
            31935, // Avenger's Shield
            15487, // Silence
            1330, // Garrote - Silence
            18498 // Silenced - Gag Order
        };

        #endregion Category : PvP DamageSilenceCC

        #region Category : PvP Interrupting

        #region Casting Spells

        internal static readonly HashSet<int> PvPInterruptCasts = new HashSet<int> {
            118, //Polymorph
           // 116, Frostbolt
            61305, //Polymorph
            28271, //polymorph
            28272,  //polymorph
            61780, //Polymorph
            61721, //Polymorph
            2637,  //Hibernate
            33786, //Cyclone
           // 5185, //Healing Touch
            //8936,  Regrowth
            //50464, //Nourish
           // 19750,  //Flash of Light
           // 82326, Divine LIght 
            //2061, Flash Heal
           // 9484, Shackle Undead
            605,  //Dominate Mind
            //8129, Mana Burn
            //331, Healing Wave
            //8004,  Healing Surge
            51505, //Lava Burst
         //   403, //Lightning Bolt ( Cast ) 
           // 77472, Greater Healing Wave
            51514, //Hex
            5782, //Fear
            //1120, //Drain Soul
            48181, //Haunt
            30108, 
            33786, // Cyclone		(cast)\
            28272, // Pig Poly		(cast)
            118, // Sheep Poly		(cast)
            61305, // Cat Poly		(cast)
            61721, // Rabbit Poly		(cast)
            61780, // Turkey Poly		(cast)
            28271, // Turtle Poly		(cast)
            51514, // Hex			(cast)
            51505, // Lava Burst		(cast)
            339, // Entangling Roots	(cast)
            30451, // Arcane Blast		(cast)
            605, // Dominate Mind		(cast)
            20066, //Repentance		(cast)
            116858, //Chaos Bolt		(cast)
            113092, //Frost Bomb		(cast)
            8092, //Mind Blast		(cast)
            11366, //Pyroblast		(cast)
            48181, //Haunt			(cast)
            102051, //Frost Jaw		(cast)
          //  1064, // Chain Heal		(cast)
           // 77472, // Greater Healing Wave	(cast)
          //  8004, // Healing Surge		(cast)
           73920, // Healing Rain		(cast)
            51505, // Lava Burst		(cast)
           // 8936, // Regrowth		(cast)
          //  2061, // Flash Heal		(cast)
          //  2060, // Greater Heal		(cast)
            32375, // Mass Dispel		(cast)
            2006, // Resurrection		(cast)
         //   5185, // Healing Touch		(cast)
         //   596, // Prayer of Healing	(cast)
          //  19750, // Flash of Light	(cast)
         //   635, // Holy Light		(cast)
            7328, // Redemption		(cast)
            2008, // Ancestral Spirit	(cast)
            50769, // Revive		(cast)
            2812, // Denounce		(cast)
          //  82327, // Holy Radiance		(cast)
            10326, // Turn Evil		(cast)
            82326, // Divine Light		(cast)
            82012, // Repentance		(cast)
          //  116694, // Surging Mist		(cast)
         //   124682, // Enveloping Mist	(cast)
          //  115151, // Renewing Mist	(cast)
            115310, // Revival		(cast)
            44614, // Frostfire Bolt	(cast)
            1513, // Scare Beast		(cast)
            982, // Revive Pet		(cast)
            111771, // Demonic Gateway			(cast)
            124465, // Vampiric Touch			(cast)
            32375 // Mass Dispel				(cast)
        };

        #endregion Casting Spells

        #region Channeled Spells

        internal static readonly HashSet<int> PvPINterruptChannels = new HashSet<int> {
            1120, // Drain Soul		(channeling cast)
12051, // Evocation		(channeling cast)
115294, // Mana Tea		(channeling cast)
115175, // Soothing Mist	(channeling cast)
64843, // Divine Hymn		(channeling cast)
64901, // Hymn of Hope		(channeling cast)
115176, // Zen Meditaion
103103, //Malefic Grasp
605,		// Dominate Mind
15407, // Mind Flay
129197, // Insanity
47540 // Penance
        };

        #endregion Channeled Spells

        #endregion Category : PvP Interrupting

        #region Category : PvP StealthAuras
        internal static readonly HashSet<int> PvPStealths = new HashSet<int> 
        {
            1784, //Stealth
            5215, //Prowl
            58984 //Shadowmeld
            
        };
        #endregion

        ////////////PvE/////////////

        /// <summary>
        /// To detect enemy specializations based on their auras
        /// </summary>
        internal static readonly HashSet<int> NoCooldowns = new HashSet<int>
        {
            //TODO: Add Spells, it's class specific
        };



        ////////////Misc/////////////


        #region Category : Roles
        /// <summary>
        /// To detect enemy specializations based on their auras
        /// </summary>
        internal static readonly HashSet<int> RoleScanHealer = new HashSet<int>
        {
            88821, 76669,// Mastery: Illuminated Healing Requires Paladin (Holy)
            81662,//Evangelism	Discipline, Holy, Passive Requires Priest (Discipline, Holy)
            16213,//Purification - Requires Shaman (Restoration)
            48500,//Living Seed - Requires Druid (Restoration)
            117907//Mastery: Gift of the Serpent Requires Monk (Mistweaver)
        };


        internal static readonly HashSet<string> RoleScanTank = new HashSet<string> { "Vengeance", "Warsong Flag", "Horde Flag", "Alliance Flag", "Orb of Power" };
        #endregion
    }
}
