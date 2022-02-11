using System;
using System.Collections.Generic;
using System.Linq;
using Miracle_Business_Solutions_Framework.Extensions;
using Miracle_Business_Solutions_Framework.Managers;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Miracle_Business_Solutions_Framework.Base
{
    internal static class Unit
    {
        #region Tidy : AuraMechanics
       

        /// <summary>
        ///     Returns wheter the unit is stunned or not by checking Mechanic, if that fails checks auralist
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsStunned(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Stunned, WoWSpellMechanic.Incapacitated);
        }
        
        /// <summary>
        ///     Returns wheter the unit is Rooted or slowed not by checking Mechanic, if that fails checks auralist
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsRootorSlowed(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Rooted, WoWSpellMechanic.Shackled, WoWSpellMechanic.Slowed, WoWSpellMechanic.Snared);
        }

        /// <summary>
        ///     Returns wheter the unit is Rooted or not by checking Mechanic, if that fails checks auralist
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsRooted(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Rooted, WoWSpellMechanic.Shackled);
        }

        /// <summary>
        ///     Returns wheter the unit is Silenced or not by checking Mechanic, if that fails checks auralist
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsSilenced(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Interrupted, WoWSpellMechanic.Silenced);
        }

        /// <summary>
        ///     Slow is a general term for any effect (debuff) that lowers the target's attack or movement speed.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsSlowed(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Slowed, WoWSpellMechanic.Snared);
        }

        /// <summary>
        ///     Snare effects are a form of crowd control that reduce a victim's movement speed, preventing him from effectively maneuvering in combat.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool IsSnared(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Slowed, WoWSpellMechanic.Snared, WoWSpellMechanic.Dazed);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsInvulnerable(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Invulnerable, WoWSpellMechanic.Invulnerable2);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsFeared(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Horrified, WoWSpellMechanic.Fleeing);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsCrowdControlled(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Charmed, WoWSpellMechanic.Banished, WoWSpellMechanic.Asleep, WoWSpellMechanic.Polymorphed);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsSleeped(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Charmed, WoWSpellMechanic.Asleep, WoWSpellMechanic.Sapped);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsEnraged(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Enraged);
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>True/False if Spell Immune</returns>
        internal static bool IsPolymorphed(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Polymorphed);
        }

        /// <summary>
        ///   Wheter we should pause the routine on certain things
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool UnControllable(this WoWUnit unit)
        {
            return unit.HasCachedAuraWithMechanic(WoWSpellMechanic.Stunned, WoWSpellMechanic.Incapacitated, WoWSpellMechanic.Horrified, WoWSpellMechanic.Fleeing, WoWSpellMechanic.Charmed, WoWSpellMechanic.Asleep, WoWSpellMechanic.Sapped,
                WoWSpellMechanic.Charmed, WoWSpellMechanic.Banished, WoWSpellMechanic.Asleep, WoWSpellMechanic.Polymorphed, WoWSpellMechanic.Interrupted, WoWSpellMechanic.Silenced);
        }

        #endregion

        #region Tidy : Abilities

        /// <summary>
        /// Returns wheter the unit can Stealth
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool CanStealth(this WoWUnit unit)
        {
            if (unit == null)
                return false;

            //Night elfs can always stealth!
            if (unit.Race == WoWRace.NightElf) return true;
            if (!unit.IsPlayer) return false;
            switch (unit.Class)
            {
                case WoWClass.Rogue:
                    return true;
                case WoWClass.Priest:
                    return true;
                case WoWClass.Shaman:
                    return unit.IsHealer() && unit.HasCachedAura("Symbiosis");
                case WoWClass.Druid:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Wheter we can hibernate this unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static bool CanHibernate(this WoWUnit unit)
        {
            if (unit == null )
                return false;

            //Night elfs can always stealth!
            if (unit.Race == WoWRace.NightElf) return true;

            switch (unit.Class)
            {
                case WoWClass.Shaman:
                    return unit.Shapeshift == ShapeshiftForm.GhostWolf;
                case WoWClass.Druid:
                    return unit.Shapeshift != ShapeshiftForm.Normal && unit.Shapeshift != ShapeshiftForm.Moonkin;
                default:
                    return unit.IsBeast;
            }
        }


        #endregion

        #region Tidy : ValidUnit

        /// <summary>
        /// Filters that 299 shit
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool Error299(this WoWUnit u)
        {
            try
            {

                return u == null || !u.IsValid || u.IsDisabled;

            }
            catch //(Exception e)
            {
                // Logger.FailLog("Exception thrown at Error299: {0}", e);
                return true;
            }
        }

        /// <summary>
        /// Returns wheter this unit is valid or not 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool ValidUnit(this WoWUnit u)
        {
            return u != null && u.IsValid && /*u.Attackable */ !u.IsDisabled && u.IsAlive && u.CanSelect && !u.IsNonCombatPet;
        }

        /// <summary>
        /// Lite version of the Valid check, contains less entries
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool LiteValidUnit(this WoWUnit u)
        {
            return u != null && u.IsValid && u.IsAlive && !u.IsDisabled;
        }

        #endregion ValidUnit

        #region Tidy : SpecType

        internal static ClassType SpecType(this WoWUnit target)
        {
            if (target.Error299())
            {
                return ClassType.Empty;
            }

            switch (target.Class)
            {
                case WoWClass.Monk:
                    if (target.MaxMana >= 290000)
                        return ClassType.Healer;
                    break;
                case WoWClass.DeathKnight:
                    return ClassType.Melee;
                case WoWClass.Hunter:
                    return ClassType.PRanged;
                case WoWClass.Mage:
                    return ClassType.MRanged;
                case WoWClass.Paladin:
                    return target.MaxMana >= 80000 ? ClassType.Healer : ClassType.Melee;
                case WoWClass.Priest:
                    return target.Shapeshift == ShapeshiftForm.Shadow ? ClassType.MRanged : ClassType.Healer;
                case WoWClass.Rogue:
                    return ClassType.Melee;
                case WoWClass.Warlock:
                    return ClassType.MRanged;
                case WoWClass.Warrior:
                    return ClassType.Melee;
                case WoWClass.Shaman:
                    if (target.MaxMana < 40000)
                        return ClassType.Melee;
                    if (target.HasCachedAura("Elemental Oath") && target.Buffs["Elemental Oath"].CreatorGuid == target.Guid)
                        return ClassType.MRanged;
                    return ClassType.Healer;
                case WoWClass.Druid:
                    if (target.Shapeshift == ShapeshiftForm.Moonkin)
                        return ClassType.MRanged;
                    if ((target.Buffs.ContainsKey("Leader of the Pack") && target.Buffs["Leader of the Pack"].CreatorGuid == target.Guid) || target.MaxMana < 40000)
                        return ClassType.Melee;
                    return ClassType.Healer;
            }
            return ClassType.Empty;
        }

        #endregion SpecType

        #region Tidy : IsInGroup

        /// <summary>
        /// Determines wheter I'm in group or not
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        internal static bool IsInGroup(this LocalPlayer me)
        {
            return me.GroupInfo.IsInParty || me.GroupInfo.IsInRaid;
        }

        #endregion 
       
        #region Tidy : Clustered Units

        internal static IEnumerable<WoWUnit> GetRadiusCluster(this WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            if (target != null)
            {
                var targetLoc = target.Location;
                return otherUnits.Where(u => u.Location.DistanceSqr(targetLoc) <= radius * radius);
            }
            return null;
        }

        internal static WoWUnit GetBestUnitForCluster(IEnumerable<WoWUnit> units, float clusterRange)
        {
            IEnumerable<WoWUnit> wUnits = units as WoWUnit[] ?? units.ToArray();
            if (units != null && wUnits.Any())
            {
                var firstOrDefault = (from u in wUnits where !u.Error299() select new { Count = GetRadiusClusterCount(u, wUnits, clusterRange), Unit = u }).OrderByDescending(a => a.Count).FirstOrDefault();
                if (firstOrDefault != null)
                    return firstOrDefault.Unit;
            }

            return null;
        }

        internal static int GetRadiusClusterCount(this WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            var rdx = radius * radius;
            var targetLoc = target.Location;
            return otherUnits.Count(u => u.Location.DistanceSqr(targetLoc) <= rdx);
        }

        #endregion Clustered Units

        #region Tidy : IsTargetingUs

        /// <summary>
        ///     checks if unit is targeting you, your minions, a group member, or group pets
        /// </summary>
        /// <param name="u">unit</param>
        /// <returns>true if targeting your guys, false if not</returns>
        internal static bool IsTargetingUs(this WoWUnit u)
        {
            return u.IsTargetingMeOrPet || u.IsTargetingAnyMinion || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember;
        }

        #endregion IsTargetingUs

        #region Cache : IsEnemy

        /// <summary>
        /// Determines wheter a unit is a enemy or not
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static bool _IsEnemy(this WoWUnit target)
        {

           //Get Map
            var map = StyxWoW.Me.CurrentMap;
            //All our Raid Members
            var check = StyxWoW.Me.RaidMembers.Union(StyxWoW.Me.PartyMembers).Distinct().ToList();
            //Check like this in arena/battleground
            if (map.IsBattleground || map.IsArena)
            {
                if (target.IsPet)
                {
                    //Has a owner
                    if (target.OwnedByUnit != null) //Owner in party or not
                        return !check.Contains(target.OwnedByUnit);
                    //Unknown pet -> depend on hb
                    return !target.IsFriendly;
                }
                if (target.IsPlayer)
                {
                    return !check.Contains(target);
                }
                return !target.IsFriendly;
            }
            return !target.IsFriendly;

        }

        /// <summary>
        /// Cached version of isEnemy
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool IsEnemy(this WoWUnit u)
        {
            try
            {
                return GetEnemyState(u);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.IsEnemy: {0}", e);
                return !u.IsFriendly;
            }
        }

        /// <summary>
        /// Gets the cached Enemy State
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetEnemyState(this WoWUnit unit, int expiry = 5000)
        {
            if (unit.Error299()) return true;

            string cacheKey = "EN" + unit.Guid;

            // Check the cache
            var enemyState = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (enemyState == null)
            {
                // Go and retrieve the data from the objectManager
                enemyState = new Root.MathCache { Bool = unit._IsEnemy() };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(enemyState, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return enemyState.Bool;
        }
        #endregion IsEnemy

        #region Cache : Roles

        /// <summary>
        ///  Checks if it's not a healer or tank -> then its a dps
        /// </summary>
        /// <param name="u"></param>
        /// <returns>1 = True, 0 = False</returns>
        internal static bool IsDps(this WoWUnit u)
        {
            try
            {
                return GetDps(u, 5000);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns wheter a unit is a healer or not
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool IsHealer(this WoWUnit u)
        {
            try
            {
                return GetHealer(u, 5000);

            }
            catch
            {
                return false;
            }
        }

       /// <summary>
        /// Returns the role of the unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="role"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetHealer(this WoWUnit unit, int expiry = 2000)
        {
            if (unit.Error299()) return false;

            string cacheKey = "IsHealer" + unit.Guid;

            // Check the cache
            var canHealStatus = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (canHealStatus == null)
            {
                canHealStatus = new Root.MathCache { Bool = TargetManager.FriendlyHealers.Any(a=>  a != null && a.Guid == unit.Guid ) };
            }
            // Then add it to the cache so we
            // can retrieve it from there next time
            // set the object to expire
            CacheManager.Add(canHealStatus, cacheKey, expiry);
            //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            return canHealStatus.Bool;
        }

        /// <summary>
        /// Returns the role of the unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="role"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetDps(this WoWUnit unit, int expiry = 2000)
        {
            if (unit.Error299()) return false;

            

            string cacheKey = "IsDps" + unit.Guid;

            // Check the cache
            var canHealStatus = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (canHealStatus == null)
            {
                canHealStatus = new Root.MathCache { Bool = TargetManager.FriendlyMelees.Union(TargetManager.FriendlyCasters).Any(a => a != null && a.Guid == unit.Guid) };
            }
            // Then add it to the cache so we
            // can retrieve it from there next time
            // set the object to expire
            CacheManager.Add(canHealStatus, cacheKey, expiry);
            //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            return canHealStatus.Bool;
        }



        #endregion

        #region Cache : InLineofSpellSight

        /// <summary>
        /// Cached version of inlineofSpellsight
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool CachedLoS(this WoWUnit u)
        {
            try
            {
                return GetCachedInLineOfSight(u);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.CachedLoS: {0}", e);
                return u != null && u.InLineOfSpellSight;
            }
        }

        /// <summary>
        /// Gets the cached Los Check
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetCachedInLineOfSight(this WoWUnit unit, int expiry = 250)
        {

            if (unit == null) return false;

            string cacheKey = "LoS" + unit.Guid;

            // Check the cache
            var getLosStatus = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (getLosStatus == null)
            {
                // Go and retrieve the data from the objectManager
                getLosStatus = new Root.MathCache { Bool = unit.InLineOfSpellSight };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(getLosStatus, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return getLosStatus.Bool;
        }
        #endregion

        #region Cache : Range/Distance/Sqr

        /// <summary>
        /// To work with the cache manager
        /// </summary>
        private class CRange
        {
            public bool InRange;
            public double DistanceSqr;
            public double Distance;
        }

        /// <summary>
        /// Checks if the unit is within CachedInRange
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static bool CachedInRange(this WoWUnit u)
        {
            try
            {
                return GetCachedInRange(u);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.CachedInRange: {0}", e);
                return u != null && u.DistanceSqr <= 40 * 40;
            }
        }

        /// <summary>
        /// Returns the true range value as sqr
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static double CachedDistanceSqr(this WoWUnit u)
        {
            try
            {
                return GetCachedDistance(u, true);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.CachedDistanceSqr: {0}", e);
                return u.DistanceSqr;
            }
        }


        /// <summary>
        /// Returns the true range value as sqr
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static double CachedDistance(this WoWUnit u)
        {
            try
            {
                return GetCachedDistance(u, false);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.CachedDistance: {0}", e);
                return u.Distance;
            }
        }


        /// <summary>
        /// Cached version of Distance
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetCachedInRange(this WoWUnit unit, int expiry = 250)
        {

            if (unit == null) return false;

            string cacheKey = "GCDis" + unit.Guid;

            // Check the cache
            var getDistanceSqr = CacheManager.Get<CRange>(cacheKey);

            //If not cached yet, fill cache
            if (getDistanceSqr == null)
            {
                // Go and retrieve the data from the objectManager
                getDistanceSqr = new CRange { DistanceSqr = unit.DistanceSqr, Distance = unit.Distance, InRange = unit.DistanceSqr <= 1600 };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(getDistanceSqr, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return getDistanceSqr.InRange;
        }


        /// <summary>
        /// Gets the cached value of Sqr
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sqr"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static double GetCachedDistance(this WoWUnit unit, bool sqr, int expiry = 250)
        {

            if (unit == null) return 105;

            string cacheKey = "GCDis" + unit.Guid;

            // Check the cache
            var getDistanceSqr = CacheManager.Get<CRange>(cacheKey);

            //If not cached yet, fill cache
            if (getDistanceSqr == null)
            {
                // Go and retrieve the data from the objectManager
                getDistanceSqr = new CRange { DistanceSqr = unit.DistanceSqr, Distance = unit.Distance, InRange = unit.DistanceSqr <= 1600 };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(getDistanceSqr, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return sqr ? getDistanceSqr.DistanceSqr : getDistanceSqr.Distance;
        }

        #endregion
    }
}
