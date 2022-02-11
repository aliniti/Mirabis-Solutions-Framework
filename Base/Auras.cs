using System;
using System.Collections.Generic;
using System.Linq;
using Miracle_Business_Solutions_Framework.Extensions;
using Miracle_Business_Solutions_Framework.Managers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Miracle_Business_Solutions_Framework.Base
{
    internal static class Auras
    {
        /// <summary>
        ///     Indicates if the selected unit has our debuff or not.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraId">The debuff id to check for</param>
        /// <param name="msLeft"></param>
        /// <returns>Returns true, if the the checked aura is present</returns>
        internal static bool HasMyAura(this WoWUnit unit, int auraId, double msLeft = 0)
        {

            IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

            bool any = false;
            foreach (WoWAura a in auralist)
            {
                if (a.SpellId == auraId && a.TimeLeft.TotalMilliseconds > msLeft)
                {
                    any = true;
                    break;
                }
            }
            return (any);


        }
        
        /// <summary>
        ///     Checks for cached auras
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        /// <param name="stacks"></param>
        /// <param name="msLeft"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasCachedAura(this WoWUnit unit, int aura, int stacks = 0, double msLeft = 0, int expiry = 250)
        {
            using (new Performance.Block("HasCachedAura", LogCategory.Auras))
            {
                try
                {

                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                    return auralist.Any(a => a.SpellId == aura && a.StackCount >= stacks && a.TimeLeft.TotalMilliseconds > msLeft);
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAura: {0}", e);
                    return false;
                }

            }
        }

        /// <summary>
        ///     Checks for cached passives
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        /// <param name="stacks"></param>
        /// <param name="msLeft"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasCachedPassive(this WoWUnit unit, int aura, int stacks = 0, double msLeft = 0, int expiry = 1000)
        {
            try
            {
                bool any = false;

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                foreach (WoWAura a in auralist)
                {
                    if (a.IsPassive && a.SpellId == aura && a.StackCount >= stacks && a.TimeLeft.TotalMilliseconds > msLeft)
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasCachedPassive: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Checks for cached auras
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        /// <param name="stacks"></param>
        /// <param name="msLeft"></param>
        /// <param name="active"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasCachedAura(this WoWUnit unit, string aura, int stacks = 0, double msLeft = 0, bool active = true, int expiry = 250)
        {
            using (new Performance.Block("HasCachedAura", LogCategory.Auras))
            {
                try
                {

                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if ((!active || a.IsActive) && a.Spell.Name == aura && a.StackCount >= stacks &&
                            a.TimeLeft.TotalMilliseconds > msLeft)
                        {
                            any = true;
                            break;
                        }
                    }
                    return (any);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown at Auras.HasCachedAura: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for my cached auras
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        /// <param name="stacks"></param>
        /// <param name="msLeft"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasMyCachedAura(this WoWUnit unit, int aura, int stacks = 0, double msLeft = 0, int expiry = 100)
        {
            try
            {
                if (unit == null)
                    return false;

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (a.SpellId == aura && a.CreatorGuid == Root.MyGuid && a.TimeLeft.TotalMilliseconds > msLeft && a.StackCount >= stacks)
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Auras.HasMyCachedAura: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Checks for any cached aura
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <param name="msleft"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasAnyCachedAura(this WoWUnit unit, HashSet<int> auraIDs, int msleft, int expiry = 250)
        {
            try
            {
                if (unit == null)
                    return false;

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (auraIDs.Contains(a.SpellId) && a.TimeLeft.TotalMilliseconds > msleft)
                    {
                        any = true;
                        break;
                    }
                }
                return
                    (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyCachedAura: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Checks for any cached aura
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <returns></returns>
        internal static bool HasAnyCachedAura(this WoWUnit unit, HashSet<int> auraIDs)
        {
            try
            {

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (auraIDs.Contains(a.SpellId))
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyCachedAura: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Non Cached version
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <param name="msleft"></param>
        /// <returns></returns>
        internal static bool HasAnyAura(this WoWUnit unit, HashSet<int> auraIDs, double msleft = 0)
        {
            try
            {
                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);
                foreach (WoWAura a in auralist)
                {
                    if (auraIDs.Contains(a.SpellId) && a.TimeLeft.TotalMilliseconds > msleft) return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyAura: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Non-Cached version
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auranames"></param>
        /// <returns></returns>
        internal static bool HasAnyAura(this WoWUnit unit, IEnumerable<string> auranames)
        {
            try
            {

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);
                var hashes = new HashSet<string>(auranames);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (hashes.Contains(a.Name))
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyAura: {0}", e);
                return false;
            }
        }
        
        /// <summary>
        ///     Checks for cached passives
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasAnyCachedPassive(this WoWUnit unit, HashSet<int> auraIDs, int expiry = 1000)
        {
            try
            {

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (a.IsPassive && auraIDs.Contains(a.SpellId))
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyCachedPassive: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Checks for cached passives
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static bool HasAnyCachedPassive(this WoWUnit unit, HashSet<string> auranames, int expiry = 1000)
        {
            try
            {

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                bool any = false;
                foreach (WoWAura a in auralist)
                {
                    if (a.IsPassive && auranames.Contains(a.Name))
                    {
                        any = true;
                        break;
                    }
                }
                return (any);
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasAnyCachedPassive: {0}", e);
                return false;
            }
        }

        /// <summary>
        ///     Checks wheter the unit has any cached aura
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <param name="auranames">Auras</param>
        /// <param name="expiry">Cache expiry time</param>
        /// <returns>True/False</returns>
        internal static bool HasAnyCachedAura(this WoWUnit unit, IEnumerable<string> auranames, int expiry = 250)
        {
            using (new Performance.Block("HasAnyCachedAura", LogCategory.Auras))
            {
                try
                {
                    //Save them in order to iterate through them
                    var hashes = new HashSet<string>(auranames);
                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);

                    foreach (WoWAura a in auralist)
                    {
                        if (hashes.Contains(a.Name)) return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasAnyCachedAura: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for the aura stackcount
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        /// <param name="fromMyAura"></param>
        /// <param name="expiry"></param>
        /// <returns>True/False</returns>
        internal static uint CachedStackCount(this WoWUnit unit, int aura, bool fromMyAura = false, int expiry = 250)
        {
            try
            {
                //Save them in order to iterate through them
                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit, expiry);
                WoWAura cachedResult = null;
                foreach (WoWAura a in auralist)
                {
                    if (a.SpellId == aura && (!fromMyAura || a.CreatorGuid == Root.MyGuid))
                    {
                        cachedResult = a;
                        break;
                    }
                }

                return cachedResult != null ? cachedResult.StackCount : 0;
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.CachedStackCount: {0}", e);
                return 0;
            }
        }


        /// <summary>
        ///     Checks for aura mechanics
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="mechanics"></param>
        /// <returns></returns>
        internal static bool HasCachedAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            using (new Performance.Block("HasCachedAuraWithMechanic", LogCategory.Auras))
            {
                try
                {
                    var hashes = new HashSet<WoWSpellMechanic>(mechanics);
                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if (hashes.Contains(a.Spell.Mechanic))
                        {
                            any = true;
                            break;
                        }
                    }
                    return (any);
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAuraWithEffect: {0}", e);
                    return false;
                }

            }
        }

        /// <summary>
        ///     Checks for aura mechanics
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="msleft"></param>
        /// <param name="mechanics"></param>
        /// <returns></returns>
        internal static bool HasCachedAuraWithMechanic(this WoWUnit unit, double msleft, params WoWSpellMechanic[] mechanics)
        {
            using (new Performance.Block("HasCachedAuraWithMechanic", LogCategory.Auras))
            {
                try
                {
                    var hashes = new HashSet<WoWSpellMechanic>(mechanics);
                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if (hashes.Contains(a.Spell.Mechanic) && a.TimeLeft.TotalMilliseconds > msleft)
                        {
                            any = true;
                            break;
                        }
                    }
                    return (any);

                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAuraWithEffect: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for auras with effects
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraType"></param>
        /// <param name="miscValue"></param>
        /// <param name="basePointsMin"></param>
        /// <param name="basePointsMax"></param>
        /// <param name="msleft"></param>
        /// <returns></returns>
        internal static bool HasCachedAuraWithEffect(this WoWUnit unit, WoWApplyAuraType auraType, int miscValue,
            int basePointsMin, int basePointsMax, int msleft)
        {
            using (new Performance.Block("HasCachedAuraWithEffect", LogCategory.Auras))
            {
                try
                {
                    //Save them in order to iterate through them
                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);
                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if (a.TimeLeft.TotalMilliseconds > msleft)
                        {
                            bool any1 = false;
                            foreach (SpellEffect e in a.Spell.GetSpellEffects())
                            {
                                if (e != null && e.AuraType == auraType && (miscValue == -1 || e.MiscValueA == miscValue) &&
                                    e.BasePoints >= basePointsMin && e.BasePoints <= basePointsMax)
                                {
                                    any1 = true;
                                    break;
                                }
                            }
                            if (any1)
                            {
                                any = true;
                                break;
                            }
                        }
                    }
                    return (any);
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAuraWithEffect: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for auras with effect
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="applyType"></param>
        /// <returns></returns>
        internal static bool HasCachedAuraWithEffect(this WoWUnit unit, WoWApplyAuraType applyType)
        {
            using (new Performance.Block("HasCachedAuraWithEffect", LogCategory.Auras))
            {
                try
                {

                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if (a.Spell != null && a.Spell.SpellEffects.Select(at => at.AuraType).Contains(applyType))
                        {
                            any = true;
                            break;
                        }
                    }
                    return (any);
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAuraWithEffect: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for auras with certain effect
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="applyType"></param>
        /// <returns></returns>
        internal static bool HasCachedAuraWithEffect(this WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            using (new Performance.Block("HasCachedAuraWithEffect", LogCategory.Auras))
            {
                try
                {
                    var hashes = new HashSet<WoWApplyAuraType>(applyType);
                    IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                    bool any = false;
                    foreach (WoWAura a in auralist)
                    {
                        if (a.Spell != null && a.Spell.SpellEffects.Any(se => hashes.Contains(se.AuraType)))
                        {
                            any = true;
                            break;
                        }
                    }
                    return (any);
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Auras.HasCachedAuraWithEffect: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Checks for the aura time left
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraID"></param>
        /// <param name="fromMyAura"></param>
        /// <returns>Auratimeleft</returns>
        internal static TimeSpan HasCachedAuraTimeLeft(this WoWUnit unit, int auraID, bool fromMyAura)
        {
            try
            {
                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                WoWAura wantedAura = null;
                foreach (WoWAura a in auralist)
                {
                    if (a.SpellId == auraID && a.TimeLeft > TimeSpan.Zero && (!fromMyAura || a.CreatorGuid == Root.MyGuid))
                    {
                        wantedAura = a;
                        break;
                    }
                }

                return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasCachedAuraTimeLeft: {0}", e);
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        ///     Returns aura timeleft
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="auraIDs"></param>
        /// <param name="fromMyAura"></param>
        /// <returns>Auratimeleft</returns>
        internal static TimeSpan HasAnyCachedAuraTimeLeft(this WoWUnit unit, HashSet<int> auraIDs, bool fromMyAura)
        {
            try
            {

                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                WoWAura wantedAura = null;
                foreach (WoWAura a in auralist)
                {
                    if (a.TimeLeft > TimeSpan.Zero && auraIDs.Contains(a.SpellId) && (!fromMyAura || a.CreatorGuid == Root.MyGuid))
                    {
                        wantedAura = a;
                        break;
                    }
                }

                return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;

            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasCachedAuraTimeLeft: {0}", e);
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        ///     Returns aura timeleft
        /// </summary>
        /// <param name="auranames"></param>
        /// <param name="fromMyAura"></param>
        /// <param name="unit"></param>
        /// <returns>Auratimeleft</returns>
        internal static TimeSpan HasAnyCachedAuraTimeLeft(this WoWUnit unit, IEnumerable<string> auranames, bool fromMyAura)
        {
            try
            {
                IEnumerable<WoWAura> auralist = GetAllCachedAuras(unit);

                WoWAura wantedAura = null;
                foreach (WoWAura a in auralist)
                {
                    if (a.TimeLeft > TimeSpan.Zero && auranames.Contains(a.Name) && (!fromMyAura || a.CreatorGuid == Root.MyGuid))
                    {
                        wantedAura = a;
                        break;
                    }
                }

                return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;

            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown at Auras.HasCachedAuraTimeLeft: {0}", e);
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        ///     Gets all cached auras on a unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        internal static IEnumerable<WoWAura> GetAllCachedAuras(this WoWUnit unit, int expiry = 100)
        {
            using (new Performance.Block("GetAllCachedAuras", LogCategory.Auras))
            {
                try
                {
                    //Catch Nulls
                    if (unit == null)
                        return new WoWAuraCollection();

                    string cacheKey = "GA" + unit.Guid;

                    // Check the cache
                    var getAllAuras = CacheManager.Get<WoWAuraCollection>(cacheKey);

                    //If not cached yet, fill cache
                    if (getAllAuras == null)
                    {
                        // Go and retrieve the data from the objectManager
                        getAllAuras = unit.GetAllAuras();

                        // Then add it to the cache so we
                        // can retrieve it from there next time
                        // set the object to expire
                        CacheManager.Add(getAllAuras, cacheKey, expiry);
                        //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
                    }
                    return getAllAuras;
                }
                catch
                {
                    //Eat it
                    return new WoWAuraCollection();
                }
            }

        }
        
        /// <summary>
        ///     Checks for spell effects
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        private static IEnumerable<SpellEffect> GetSpellEffects(this WoWSpell spell)
        {
            var effects = new SpellEffect[3];
            effects[0] = spell.GetSpellEffect(0);
            effects[1] = spell.GetSpellEffect(1);
            effects[2] = spell.GetSpellEffect(2);
            return effects;
        }
    }
}