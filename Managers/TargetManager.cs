using System;
using System.Collections.Generic;
using System.Linq;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Styx;
using Styx.Common.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Miracle_Business_Solutions_Framework.Managers
{
    class TargetManager
    {
        #region Tidy : Target(s)
        //Targets
        public static WoWUnit HealTarget;

        public static WoWUnit DrinkingEnemy;
        public static WoWUnit DrinkingFriendly;
        public static WoWUnit PsyFiend;
        public static DispelManager.DTarget DispelTarget;
        public static InterruptManager.ITarget InterruptTarget;

        /// <summary>
        /// All Friendly units
        /// </summary>
        internal static readonly List<WoWUnit> AllFriendlies = new List<WoWUnit>();
        /// <summary>
        /// UnitPower < 100000
        /// </summary>
        internal static readonly List<WoWPlayer> FriendlyMelees = new List<WoWPlayer>();
        /// <summary>
        /// UnitPower > 290000
        /// </summary>
        internal static readonly List<WoWPlayer> FriendlyCasters = new List<WoWPlayer>();
        /// <summary>
        /// Only pets, no totems
        /// </summary>
        internal static readonly List<WoWUnit> FriendlyPets = new List<WoWUnit>();
        /// <summary>
        /// Friendly Healers, by role
        /// </summary>
        internal static readonly List<WoWPlayer> FriendlyHealers = new List<WoWPlayer>();
        /// <summary>
        ///Friendly players consuming drinks 
        /// </summary>
        internal static readonly List<WoWPlayer> FriendlyDrinkingPlayers = new List<WoWPlayer>();
        /// <summary>
        /// Enemy players consuming drinks
        /// </summary>
        internal static readonly List<WoWPlayer> EnemyDrinkingPlayers = new List<WoWPlayer>();
        /// <summary>
        /// Totems
        /// </summary>
        internal static readonly List<WoWObject> Totems = new List<WoWObject>();
        /// <summary>
        /// Friendly players carrying a flag
        /// </summary>
        internal static readonly List<WoWPlayer> FriendlyFlagCarriers = new List<WoWPlayer>();
        /// <summary>
        /// Enemy players carrying a flag
        /// </summary>
        internal static readonly List<WoWPlayer> EnemyFlagCarriers = new List<WoWPlayer>();
        /// <summary>
        /// UnitPower < 100000
        /// </summary>
        internal static readonly List<WoWPlayer> EnemyMelees = new List<WoWPlayer>();
        /// <summary>
        /// UnitPower > 290000
        /// </summary>
        internal static readonly List<WoWPlayer> EnemyCasters = new List<WoWPlayer>();
        /// <summary>
        /// Only pets, no totems
        /// </summary>
        internal static readonly List<WoWUnit> EnemyPets = new List<WoWUnit>();
        /// <summary>
        /// Friendly Healers, by role
        /// </summary>
        internal static readonly List<WoWPlayer> EnemyHealers = new List<WoWPlayer>();
        /// <summary>
        /// All enemy units
        /// </summary>
        internal static readonly List<WoWUnit> AllEnemies = new List<WoWUnit>();
        #endregion

        #region Tidy : AcquireTargets

        /// <summary>
        /// Acquires all our Targets
        /// </summary>
        private static bool AcquireTargets()
        {
            using (new Performance.Block("AcquireTargets", LogCategory.TargetManager))
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    try
                    {   //Core
                        RefreshLists();
                        SetHealtarget();
                        SetDispelTarget();
                        SetInterruptTarget();
                        SetDrinkingPlayers();
                        SetPsyFiend();
                        //Class Specific


                        return true;
                    }
                    catch (Exception e)
                    {
                        Logger.FailLog("Exception thrown in HealEngine.PulsePvPTargets: {0}", e);

                        return false;
                    }
                }
            }
        }

        #endregion

        #region Tidy : SetTargets

        /// <summary>
        /// Populates the 
        /// </summary>
        private static void SetHealtarget()
        {
            //TODO: Add ur requirements
            HealTarget = AllFriendlies.Where(a=> a != null).OrderBy(a=> a.GetPredictedHealthPercent()).FirstOrDefault();
        }

        /// <summary>
        /// Populates the 
        /// </summary>
        private static void SetInterruptTarget()
        {
            InterruptTarget = InterruptManager.MyTarget();
        }

        /// <summary>
        /// Populates the 
        /// </summary>
        private static void SetDrinkingPlayers()
        {
            //TODO: ADD extra requirements if needed
            DrinkingEnemy = EnemyDrinkingPlayers.Where(a => a != null).OrderBy(a => a.CachedDistanceSqr()).FirstOrDefault();

            DrinkingFriendly = FriendlyDrinkingPlayers.Where(a => a != null).OrderBy(a => a.CachedDistanceSqr()).FirstOrDefault();
        }

        /// <summary>
        /// Gets PsyFiend
        /// </summary>
        private static void SetPsyFiend()
        {
            //BUG: Test if it's a totem, pet or just a unit
            PsyFiend = Totems.Where(a => a.Entry == 108921).OrderBy(a => a.DistanceSqr).FirstOrDefault() as WoWUnit;
        }
        /// <summary>
        /// Populates the 
        /// </summary>
        private static void SetDispelTarget()
        {
            DispelTarget = DispelManager.MyTarget();
        }


        #endregion

        #region Tidy : RefreshLists
        /// <summary>
        /// Refreshes the lists
        /// </summary>
        private static bool RefreshLists()
        {
            using (new Performance.Block("RefreshLists", LogCategory.TargetManager))
            {
                try
                {

                    #region Clear all Lists
                    EnemyHealers.Clear();
                    EnemyCasters.Clear();
                    EnemyMelees.Clear();
                    EnemyPets.Clear();
                    AllEnemies.Clear();
                    AllFriendlies.Clear();
                    FriendlyCasters.Clear();
                    FriendlyHealers.Clear();
                    FriendlyMelees.Clear();
                    FriendlyPets.Clear();
                    Totems.Clear();
                    FriendlyDrinkingPlayers.Clear();
                    EnemyDrinkingPlayers.Clear();
                    FriendlyFlagCarriers.Clear();
                    EnemyFlagCarriers.Clear();
                    
                    #endregion

                    #region Tidy : Healables

                    var partyguids = StyxWoW.Me.GroupInfo.RaidMemberGuids.ToList();

                    #region Tidy : Including Pets
                    List<WoWUnit> obList = ObjectManager.GetObjectsOfTypeFast<WoWUnit>();
                        IEnumerable<WoWPlayer> temp = obList.Where(a => a != null && a.IsPlayer).Select(a => a.ToPlayer());
                        var list = new List<ulong>();
                        HashSet<ulong> set = new HashSet<ulong>();
                        foreach (WoWPlayer p in temp)
                        {
                            if (!p.Error299() && p.GotAlivePet && p.IsInMyPartyOrRaid)
                            {
                                var pet = p.Pet;
                                ulong @ulong = pet.Guid;
                                if (set.Add(@ulong)) 
                                    list.Add(@ulong);
                                //Add it ?
                                FriendlyPets.Add(pet);
                            }
                        }
                        partyguids.AddRange(list);

                    #endregion

                    #endregion

                    #region Tidy : UpdateRoles

                    IEnumerable<WoWPartyMember> mygroup = StyxWoW.Me.GroupInfo.RaidMembers;

                    foreach (WoWPartyMember u in mygroup)
                    {
                        WoWPlayer p = u.ToPlayer();

                        if (u.HasRole(WoWPartyMember.GroupRole.Tank))
                            FriendlyMelees.Add(p);
                        else if (u.HasRole(WoWPartyMember.GroupRole.Healer))
                            FriendlyHealers.Add(p);
                        else if (u.HasRole(WoWPartyMember.GroupRole.Damage))
                        {
                            //UnitPower > 290000
                            if (p.MaxPower > 290000)
                                FriendlyCasters.Add(p);
                            else FriendlyMelees.Add(p);
                        }
                    }

                    #endregion

                    #region Tidy : Objectmanager


                    //For each unit in the objectmanager list, within 60 yards radius
                    foreach (WoWUnit unit in obList)
                    {
                        if (partyguids.Contains(unit.Guid))
                        {
                            //Healers, and DPS have been populated in UpdateRoles

                            //Drinking
                            if (unit.IsPlayer && unit.HasCachedAura("Drink"))
                                FriendlyDrinkingPlayers.Add(unit as WoWPlayer);
                            //Pets
                            if (unit.IsPet)
                                FriendlyPets.Add(unit);
                            //Flag Carriers
                            if (unit.HasAnyCachedAura(new HashSet<int> { 23335, 23333 }))
                                FriendlyFlagCarriers.Add(unit as WoWPlayer);
                        }
                        else //Everything else
                        {
                            //All units
                            AllEnemies.Add(unit);
                            
                            //Drinking
                            if (unit.IsPlayer && unit.HasCachedAura("Drink"))
                                EnemyDrinkingPlayers.Add(unit as WoWPlayer);

                            //Flag Carriers
                            if (unit.HasAnyCachedAura(new HashSet<int> { 23335, 23333 }))
                                EnemyFlagCarriers.Add(unit as WoWPlayer);

                            //Save those for usage below
                            var unitPower = unit.MaxPower;
                            var wowclass = unit.Class;
                            //Casters
                            if (unitPower > 2900000)
                            {
                                EnemyCasters.Add(unit as WoWPlayer);
                                //Healers
                                if ((wowclass == WoWClass.Druid || wowclass == WoWClass.Monk || wowclass == WoWClass.Paladin || wowclass == WoWClass.Shaman ) 
                                    && !unit.HasAnyCachedAura(new HashSet<int> { 24858,15473,324}))
                                    EnemyHealers.Add(unit as WoWPlayer);
                            }
                            else if (unitPower < 100000)
                            {   //Players
                                if (unit.IsPlayer)
                                    EnemyMelees.Add(unit as WoWPlayer);
                                //Pets
                                else if (unit.IsPet)
                                    EnemyPets.Add(unit);
                                //Totems
                                else if (unit.IsTotem)
                                    Totems.Add(unit);
                            }
                        }
                    }

                    #endregion
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.FailLog("Exception thrown at HealEngine.TidyOB : {0}", ex);
                    return false;
                }
            }
        }
        #endregion

        #region Tidy : Pulse
        /// <summary>
        /// Delay continuees scanning
        /// </summary>
        private static WaitTimer OBDelay = new WaitTimer(TimeSpan.FromMilliseconds(250));

        /// <summary>
        /// Updates all the targets
        /// </summary>
        internal static void Pulse()
        {
            try
            {
                //When our timer ended, we can safely repopulate
                if (OBDelay.IsFinished && CastManager.GlobalCooldownLeft.TotalMilliseconds <= 40)
                {
                    if (AcquireTargets())
                        {
                            OBDelay.Reset();
                        }
                }
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown in HealEngine.Pulse: {0}", e);
            }
        }

        #endregion
    }

    /// <summary>
    /// Gives an InterruptTarget
    /// </summary>
    class InterruptManager
    {
        /// <summary>
        /// Target with priority
        /// </summary>
        public static ITarget MyTarget()
        {
            //Validate Unit
            if (_MyTarget.Unit == null || !_MyTarget.Unit.IsAlive)
                return new ITarget();
            //TODO: Change this to ur likeness
            if (TargetManager.AllFriendlies.Any(a => a != null && a.HealthPercent < 40))
                _MyTarget.Prio = Priority.High;
            else if (_MyTarget.Unit.CurrentTarget != null && _MyTarget.Unit.CurrentTarget.HealthPercent < 40)
                _MyTarget.Prio = Priority.Medium;
            else _MyTarget.Prio = Priority.Low;

            return _MyTarget;

        }
        /// <summary>
        /// Target without priority
        /// </summary>
        private static ITarget _MyTarget;

        /// <summary>
        /// Struct for our Target
        /// </summary>
        public struct ITarget
        {
            public Priority Prio;
            public WoWUnit Unit;
            public WoWSpell Spell;
        }

        /// <summary>
        /// Interrupts on a random tme
        /// </summary>
        /// <returns></returns>
        private static bool InterruptRandom(WoWUnit unit, bool channel = false)
        {
            //start interrupt @ random number between (10-90)%
            var mininterrupt = RandomNumber(10, 90);
            //dont interrupt above 95 %
            const int maxinterrupt = 95;
            //start interrupt @ random number between (10-36)%
            var channelInterruptmin = RandomNumber(15, 40);
            //dont interrupt above random number between (50,80) %
            var channelInterruptmax = RandomNumber(50, 80);
            double castpercent;

            if (channel) // 54 + 3 = 56 - 54 = 3s cast / n
                castpercent = (unit.CurrentChannelTimeLeft.TotalMilliseconds / (unit.CurrentChannelEndTime - unit.CurrentChannelStartTime).TotalMilliseconds) * 100;
            else
                castpercent = (unit.CurrentCastTimeLeft.TotalMilliseconds / (unit.CurrentCastEndTime - unit.CurrentCastStartTime).TotalMilliseconds) * 100;

            if (channel && castpercent.Between(channelInterruptmin, channelInterruptmax))
            {
                Logger.DevLog("Interrupt target found ({0}) - Cast % : {1} - Spell : {2}", unit.Name, castpercent, unit.ChanneledSpell.Name);
                return true;
            }
            if (castpercent.Between(mininterrupt, maxinterrupt))
            {
                Logger.DevLog("Interrupt target found ({0}) - Cast % : {1} - Spell : {2}", unit.Name, castpercent, unit.CastingSpell.Name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Little helper
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
    }

    /// <summary>
    /// Gives an DispelManager
    /// </summary>
    class DispelManager
    {
        /// <summary>
        /// Target with priority
        /// </summary>
        public static DTarget MyTarget()
        { 
            //Validate Unit
            if (_MyTarget.Unit == null || !_MyTarget.Unit.IsAlive)
                return new DTarget();

            //Add Priority
            var checkaura = _MyTarget.Aura.SpellId;
            //TODO: Change the Priorities..to whatever u want
            if (TargetManager.AllFriendlies.Any(a=> a != null && a.HealthPercent < 40))
                _MyTarget.Prio = Priority.High;
            else if (Lists.PvPAttentionBuffs.Contains(checkaura))
                _MyTarget.Prio = Priority.Medium;
            else _MyTarget.Prio = Priority.Low;

            return _MyTarget;

        }
        /// <summary>
        /// Target without priority
        /// </summary>
        private static DTarget _MyTarget;

        internal struct DTarget
        {
            public Priority Prio;
            public WoWUnit Unit;
            public WoWAura Aura;
            public string Dispelling;
        }

        /// <summary>Checks the aura to see if it can be dispeled by the name on specified unit</summary>
        /// <param name="unit">The unit to check auras for. </param>
        /// <param name="disease">The disease.</param>
        /// <param name="magic">The magic.</param>
        /// <param name="poison">The poison.</param>
        /// <param name="curse">The curse.</param>
        /// <returns>true if the target has an aura to dispel</returns>
        private static bool PvPAuraToDispel(WoWUnit unit, bool disease, bool magic, bool poison, bool curse)
        {
            foreach (WoWAura aura in unit.GetAllCachedAuras()) //.Where(AuraName => !Lists.DoNotDispel.Contains(aura.SpellId)))
            {
                if (curse && Lists.PvPDispelCurse.Contains(aura.SpellId))
                {
                    _MyTarget = new DTarget { Aura = aura, Dispelling = aura.Name, Unit = unit };
                    return true;
                }
                if (magic && Lists.PvPDispelMagic.Contains(aura.SpellId))
                {
                    _MyTarget = new DTarget { Aura = aura, Dispelling = aura.Name, Unit = unit };
                    return true;
                }
                if (poison && Lists.PvPDispelPoison.Contains(aura.SpellId))
                {
                    _MyTarget = new DTarget { Aura = aura, Dispelling = aura.Name, Unit = unit };
                    return true;
                }
                if (disease && aura.Spell.DispelType == WoWDispelType.Disease)
                {
                    _MyTarget = new DTarget { Aura = aura, Dispelling = aura.Name, Unit = unit };
                    return true;
                }
            }

            return false;
        }
    }
}
