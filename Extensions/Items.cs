using System;
using System.Collections.Generic;
using System.Linq;
using Miracle_Business_Solutions_Framework;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Miracle_Business_Solutions_Framework.Managers;
using Styx;
using Styx.Common.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;


namespace Miracle_Business_Solutions_Framework.Misc
{
    // ReSharper disable ImplicitlyCapturedClosure
    internal static class Items
    {

        private static WoWItem FindFirstUsableItemBySpell(params string[] spellNames)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            var spellNameHashes = new HashSet<string>(spellNames);

            return (carried.Select(i => new { i, spells = i.ItemSpells }).Where(@t => @t.i.ItemInfo != null && @t.spells != null && @t.spells.Count != 0 && @t.i.Usable &&
                Math.Abs(@t.i.Cooldown - 0) < 1 && @t.i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level && @t.spells.Any(s => s.IsValid && s.ActualSpell != null && spellNameHashes.Contains(s.ActualSpell.Name))).OrderByDescending(@t => @t.i.ItemInfo.Level).Select(@t => @t.i)).FirstOrDefault();
        }

        /// <summary>
        /// Uses Mana potions
        /// </summary>
        /// <returns></returns>
        internal static Composite UseManaPotions()
        {
            return new Decorator(ret => MANASETTINGS,
                    new PrioritySelector(ctx => FindFirstUsableItemBySpell("Water Spirit", "Mana Potion"),
                        new Decorator(ctx => ctx != null,
                            new Action(ctx =>
                            {
                                Logger.ItemLog("{0} used at {1}% Mana", ((WoWItem)ctx).Name, StyxWoW.Me.ManaPercent);
                                ((WoWItem)ctx).UseContainerItem();

                            }))));
        }

        /// <summary>
        /// Uses health potions
        /// </summary>
        /// <returns></returns>
        internal static Composite UseHealthPotions()
        {
            return new Decorator(ret => HEALTHSTONESETTINGS,
                new PrioritySelector(ctx => FindFirstUsableItemBySpell("Healthstone"),
                        new Decorator(ret => ret != null,
                                                new Action(ctx =>
                                                {
                                                    Logger.ItemLog("{0} used at {1}% HP", ((WoWItem)ctx).Name, StyxWoW.Me.HealthPercent);
                                                    ((WoWItem)ctx).UseContainerItem();
                                                }))));
        }

        /// <summary>
        /// Uses healthstones
        /// </summary>
        /// <returns></returns>
        internal static Composite UseHealthstone()
        {
            return new Decorator(ret => HEALTHSETTINGS,
                new PrioritySelector(ctx => FindFirstUsableItemBySpell("Life Spirit", "Healing Potion"),
                        new Decorator(ret => ret != null,
                                                new Action(ctx =>
                                                {
                                                    Logger.ItemLog("{0} used at {1}% HP", ((WoWItem)ctx).Name, StyxWoW.Me.HealthPercent);
                                                    ((WoWItem)ctx).UseContainerItem();
                                                }))));
        }

        internal static Decorator UseEquippedItem(uint slot, Root.Selection<bool> reqs = null)
        {
            var item = StyxWoW.Me.Inventory.GetItemBySlot(slot);
            return new Performance.Throttle(new Decorator(ret => item != null && (reqs == null || reqs(ret)) && CanUseEquippedItem(item),
                new Action(ret =>
                {
                    UseItem(StyxWoW.Me.Inventory.GetItemBySlot(slot));
                    return RunStatus.Failure;
                })));
        }

        private static void UseItem(WoWItem item)
        {
            if (item != null)
            {
                item.Use();
            }
        }

        private static WoWItem Itm(uint id)
        {
            return StyxWoW.Me.CarriedItems.FirstOrDefault(item => item != null && item.Entry == id);
        }

        internal static Decorator UseItem(uint id, Root.Selection<bool> reqs = null)
        {
            return new Decorator(ret => (reqs == null || reqs(ret)) && Itm(id) != null && CanUseItem(Itm(id)),
                    new Action(ret =>
                    {
                        UseItem(Itm(id));
                        return RunStatus.Failure;
                    }));
        }

        /// <summary>
        /// Checks if we can use the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool CanUseItem(WoWItem item)
        {

            return item.CachedUsable() && !item.CachedOnCooldown();
        }

        /// <summary>
        /// Checks if we can use the item
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        internal static bool CanUseItem(uint uid)
        {
            var item = Itm(uid);
            return item.CachedUsable() && !item.CachedOnCooldown();
        }

        /// <summary>
        /// Checks if we can use the item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool CanUseEquippedItem(WoWItem item)
        {
            try
            {
                return item.CachedUsable() && !item.CachedOnCooldown();
            }
            catch
            {
                return false;
            }
        }

        internal static bool NeedTrinket(int trinket = 1)
        {
            if (StyxWoW.Me.Error299()) return false;

            if (trinket == 1)
            {
                /* switch (GST.Trinket1Choice)
                {
                    case TrinketUsage.LowHealth:
                        return StyxWoW.Me.HealthPercent <= GST.Trinket1Health;
                    case TrinketUsage.LowMana:
                        return StyxWoW.Me.ManaPercent <= GST.Trinket1Mana;
                    case TrinketUsage.Stunned:
                        return StyxWoW.Me.IsStunned();
                    case TrinketUsage.StunnedandLowHealth:
                        return StyxWoW.Me.HP() <= GST.Trinket1Health && StyxWoW.Me.IsStunned();
                    default:
                        return false;
                }*/
            }
            if (trinket == 2)
            {
               /* switch (GST.Trinket2Choice)
                {
                    case TrinketUsage.LowHealth:
                        return StyxWoW.Me.HealthPercent <= GST.Trinket2Health;
                    case TrinketUsage.LowMana:
                        return StyxWoW.Me.ManaPercent <= GST.Trinket2Mana;
                    case TrinketUsage.Stunned:
                        return StyxWoW.Me.IsStunned();
                    case TrinketUsage.StunnedandLowHealth:
                        return StyxWoW.Me.HP() <= GST.Trinket1Health && StyxWoW.Me.IsStunned();
                    default:
                        return false;
                } */
            }
            return false;
        }

        /// <summary>
        /// Decides wheter we should use engineering tinker or not
        /// </summary>
        /// <returns></returns>
        internal static bool NeedGloves()
        {
            if (StyxWoW.Me.Error299()) return false;

            /*switch (GST.GlovesChoice)
            {
                case GloveUsage.Never:
                    return false;
                case GloveUsage.Smart:
                    return StyxWoW.Me.Combat && StyxWoW.Me.HasAnyCachedAura(Lists.SmartGadgetcheck);
                case GloveUsage.OnCooldownInCombat:
                    return StyxWoW.Me.Combat;
                default:
                    return false;
            }*/
            return false;
        }

        /// <summary>
        /// Decides wheter we should use lifeblood or not
        /// </summary>
        /// <returns></returns>
        internal static bool NeedLifeblood()
        {
            if (StyxWoW.Me.Error299()) return false;
            /*
            switch (GST.LifebloodChoice)
            {
                case LifebloodUsage.Never:
                    return false;
                case LifebloodUsage.OnCooldownInCombat:
                    return StyxWoW.Me.Combat;
                case LifebloodUsage.Smart:
                    return StyxWoW.Me.Combat && StyxWoW.Me.HasAnyCachedAura(Lists.SmartGadgetcheck);
                default:
                    return false;
            }*/
            return false;
        }


        /// <summary>
        /// Decides wheter we should use berserking or not
        /// </summary>
        /// <returns></returns>
        internal static bool NeedBerserking()
        {
           /*
            switch (GST.BerserkChoice)
            {
                case BerserkingUsage.Never:
                    return false;
                case BerserkingUsage.OnCooldownInCombat:
                    return StyxWoW.Me.Combat;
                case BerserkingUsage.Smart:
                    return StyxWoW.Me.Combat && StyxWoW.Me.HasAnyCachedAura(Lists.SmartGadgetcheck);
                default:
                    return false;
            }
            * */
            return false;
        }

        #region Cache

        /// <summary>
        /// Checks wheter it's on cooldown or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static bool CachedOnCooldown(this WoWItem item)
        {
            return GetItemCooldown(item).WaitTime.TotalMilliseconds > CastManager.MyLatency;

        }

        /// <summary>
        /// Checks wheter it's usable or not
        /// </summary>
        /// <param name="item"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool CachedUsable(this WoWItem item, int expiry = 10000)
        {
            return GetUsable(item, expiry);
        }

        /// <summary>
        /// Gets all cached auras on a unit
        /// </summary>
        /// <param name="item"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static WaitTimer GetItemCooldown(this WoWItem item, int expiry = 1000)
        {
            //Catch Nulls
            if (item == null)
                return new WaitTimer(TimeSpan.MaxValue);

            string cacheKey = "GIC" + item.Entry;

            // Check the cache
            var getCooldown = CacheManager.Get<WaitTimer>(cacheKey);

            //If not cached yet, fill cache
            if (getCooldown == null)
            {
                // Go and retrieve the data from the objectManager
                getCooldown = new WaitTimer(item.CooldownTimeLeft);

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(getCooldown, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return getCooldown;
        }

        /// <summary>
        /// Gets all cached auras on a unit
        /// </summary>
        /// <param name="item"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetUsable(this WoWItem item, int expiry = 1000)
        {
            //Catch Nulls
            if (item == null)
                return false;

            string cacheKey = "Usb" + item.Entry;

            // Check the cache
            var usable = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (usable == null)
            {
                // Go and retrieve the data from the objectManager
                usable = new Root.MathCache { Bool = item.Usable };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(usable, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return usable.Bool;//Bool
        }


        #endregion
    }


    // ReSharper restore ImplicitlyCapturedClosure
}