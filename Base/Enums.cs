using System;

namespace Miracle_Business_Solutions_Framework.Base
{
    #region Tidy : LogCategory
    /// <summary>
    /// Performance logger flag
    /// </summary>
    [Flags]
    public enum LogCategory
    {
        None = 0,
        Healing,
        Units,
        Auras,
        CastManager,
        TargetManager

    }
    #endregion

    public enum Role
    {
        Healer,
        Tank,
        Dps,
        None
    }

    public enum Priority
    {
        High,
        Medium,
        Low,
        None
    }

    public enum UnitCount
    {
        FriendlyPlayers,
        EnemyPlayers
    }
    
    public enum ClassType
    {
        Melee,
        MRanged,
        PRanged,
        Healer,
        Tank,
        Empty
    }

    public enum NSFavor
    {
        Automatic,
        HealingTouch,
        Regrowth
    }

    public enum TalentNames
    {
        Talent1,
        Talent2,
        Talent3
    }


    public enum TrinketUsage
    {
        Never,
        OnCooldown,
        LowHealth,
        LowMana,
        Stunned,
        StunnedandLowHealth
    }


    public enum GloveUsage
    {
        Never,
        Smart,
        OnCooldownInCombat,
    }

    public enum LifebloodUsage
    {
        Never,
        Smart,
        OnCooldownInCombat
    }

    public enum HPRoleCHeck
    {
        Spell1,
        Spell2,
        Spell3,
        Spell4,
        Spell5
    }
    public enum PotionUsage
    {
        Never,
        LowHealth,
        LowMana
    }

    public enum Interrupt
    {
        PvEBlacklist,
        PvPWhitelist
    }

}
