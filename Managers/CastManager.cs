using System;
using System.Collections.Generic;
using System.Linq;
using CommonBehaviors.Actions;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace Miracle_Business_Solutions_Framework.Managers
{
    // ReSharper disable ImplicitlyCapturedClosure
    internal static class CastManager
    {
        //For StopCasting
        internal static WoWUnit LastTarget;
        internal static WoWSpell LastCast;
        private static WoWSpell _gcdSpell;

        #region Casting : Buff

        /// <summary>
        /// Buff urself 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reqs"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite BuffSelf(WoWSpell spell, Root.Selection<bool> reqs = null, string reason = null)
        {
            return Cast(on => StyxWoW.Me, spell, reqs, reason);
        }

        /// <summary>
        /// Buffself from string
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reqs"></param>
        /// <param name="reason"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static Composite BuffSelf(string spell, Root.Selection<bool> reqs = null, string reason = null, params object[] args)
        {
            return
                new Decorator(
                    ret => SpellManager.CanBuff(spell, StyxWoW.Me) &&
                        (reqs == null || reqs(ret)), new Action(delegate
                        {
                            if (SpellManager.Cast(spell, StyxWoW.Me))
                            {
                                Logger.CombatLog("Buffing [{0}] [On: Me] [Reason: {1}]", spell, reason, args);
                                LastTarget = StyxWoW.Me;
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        }));
        }

        /// <summary>
        /// Buffself version without requirements
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite BuffSelf(WoWSpell spell, string reason = null)
        {
            return Cast(on => StyxWoW.Me, spell, ret => true, reason);
        }


        #endregion Casting : Buff

        #region Casting : Symbiosis

        internal static Composite SymbCast(WoWSpell spell, Root.Selection<WoWUnit> on, Root.Selection<bool> reqs = null, string reason = null, params object[] args)
        {
            return new Decorator(
                ret => on(ret) != null && StyxWoW.Me.HasCachedAura(110309) && (reqs == null || reqs(ret)),
                new Action(ret =>
                {
                    if (spell == null || !spell.CachedHasSpell())
                        return RunStatus.Failure;

                    if (!Woosh(spell, on(ret), true, true))
                        return RunStatus.Failure;

                    if (SpellManager.Cast(spell, on(ret)))
                    {
                        Logger.Dispelog("[{0}][On: {1}] [HP:{2:F1}%] [Reason: {3}]", spell.Name, on(ret).SafeName,on(ret).HealthPercent, reason, args);
                        LastTarget = on(ret);

                        LastCast = spell;
                        return RunStatus.Success;
                    }
                    return RunStatus.Failure;
                }));
        }
        #endregion

        #region Casting : Cast
        
        /// <summary>
        /// cached result of onUnit delegate for Spell.Cast.  for expensive queries (such as Cluster.GetBestUnitForCluster()) we want to avoid
        /// performing them multiple times.  in some cases we were caching that locally in the context parameter of a wrapping PrioritySelector
        /// but doing it here enforces for all calls, so will reduce list scans and cycles required even for targets selected by auras present/absent
        /// </summary>
        private static WoWUnit _castOnUnit;


        /// <summary>
        /// Test
        /// </summary>
        /// <param name="onUnit"></param>
        /// <param name="spell"></param>
        /// <param name="reqs"></param>
        /// <param name="reason"></param>
        /// <param name="cancel"></param>
        /// <param name="cancelreason"></param>
        /// <returns></returns>
        internal static Composite Cast(Root.Selection<WoWUnit> onUnit, WoWSpell spell, Root.Selection<bool> reqs, string reason, Root.Selection<bool> cancel = null, string cancelreason = null)
        {
            using (new Performance.Block("Cast", LogCategory.CastManager))
            {   //This checks null on the unit, and then populates the damn _castonUnit
                return new Decorator(ret => onUnit(ret) != null && (reqs == null || reqs(ret)),
                    new PrioritySelector(
                        new Sequence(
                            new Action(ret =>
                            {
                                //
                                _castOnUnit = onUnit(ret);
                                //Cast state is not determined yet
                                var status = RunStatus.Failure;
                                //Determines wheter we can cast on this unit or not
                                if (Woosh(spell, _castOnUnit, true, true))
                                {
                                    //Check if requirements meet?

                                    // .. (since spell lookup, move while casting check, and cancast take time)
                                    var health = onUnit(ret).HealthPercent;

                                    //Casts the spell, and the client will return true/false if it succeeded
                                    if (SpellManager.Cast(spell, _castOnUnit))
                                    {
                                        Logger.CombatLog("[{0}][On: {1}] [HP:{2:F1}%] [Reason: {3}]", spell.Name, _castOnUnit.SafeName, health, reason);
                                        //Cast is successful
                                        LastTarget = _castOnUnit;
                                        LastCast = spell;
                                        status = RunStatus.Success;
                                    }
                                }
                                //Return the cast status
                                return status;

                            }),
                    //Wait for the Cast to register BUG: Only needed if it's too fast
                           new WaitContinue(TimeSpan.FromMilliseconds(350), ret =>
                               (spell.CastTime == 0 && spell.CooldownTimeLeft.TotalMilliseconds > 750)
                               || StyxWoW.Me.CurrentCastTimeLeft.TotalMilliseconds > 750
                               || StyxWoW.Me.CurrentChannelTimeLeft.TotalMilliseconds > 750, new ActionAlwaysSucceed()),
                    //CancelCasting Implementation
                            new PrioritySelector(
                    //Have not specified a cancelcast, or did but are no longer casting,return back to top of tree to calculate next spell
                                new Decorator(req => cancel == null || !IsCastingorChanneling(), new ActionAlwaysSucceed()),
                    //The Wait returns RunStatus.Failure, if the associated predicate evaluates to 'false' when the internal timer expires.
                                new WaitContinue(GetSpellCastTime(spell).Subtract(TimeSpan.FromMilliseconds(MyLatency)), ret => cancel(ret), CancelCast(cancelreason)))), // If Cancelled go back up, else go down
                    //If for any reason the sequence failed, we did not cast or could not cast, check the next spell on priority
                                new ActionAlwaysFail()));

            }
        }

        /// <summary>
        /// Cast without any requirements
        /// </summary>
        /// <param name="onUnit"></param>
        /// <param name="spell"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite Cast(Root.Selection<WoWUnit> onUnit, WoWSpell spell, string reason)
        {
            return Cast(onUnit, spell, req => true, reason);
        }
        /// <summary>
        /// Picks ur Current Target, when no target specified
        /// </summary>
        /// <param name="onUnit"></param>
        /// <param name="spell"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite Cast(WoWSpell spell, string reason)
        {
            return Cast(on=> StyxWoW.Me.CurrentTarget, spell, req => true, reason);
        }

        /// <summary>
        /// Picks ur Current Target, when no target specified + requirements
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite Cast(WoWSpell spell,Root.Selection<bool> reqs, string reason)
        {
            return Cast(on => StyxWoW.Me.CurrentTarget, spell, reqs, reason);
        }


        #endregion Casting : Cast

        #region Casting : Heal

        internal static Composite Heal(WoWSpell spell, Root.Selection<bool> reqs, string reason, Root.Selection<bool> cancel, string cancelreason)
        {
            return Cast(on => TargetManager.HealTarget, spell, reqs, reason, cancel, cancelreason);
        }

        /// <summary>
        /// Cast(On=> HealEngine Healtarget, with requirements
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reqs"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite Heal(WoWSpell spell, Root.Selection<bool> reqs = null, string reason = null)
        {
            return Cast(on => TargetManager.HealTarget, spell, reqs, reason);
        }

        /// <summary>
        /// Cast(On=> HealEngine Healtarget, with no requirements
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        internal static Composite Heal(WoWSpell spell, string reason = null)
        {
            return Cast(on => TargetManager.HealTarget, spell, ret => true, reason);
        }

        #endregion Casting : Cast

        #region Casting : Interrupt

        /// <summary>
        /// Shorter version of saing Cast(onUnit=> InterruptTarget
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="reqs"></param>
        /// <returns></returns>
        internal static Composite Interrupt(WoWSpell spell, Root.Selection<bool> reqs = null)
        {
            return new Action(ret =>
            {

                var target = TargetManager.InterruptTarget.Unit;
                var prio = TargetManager.InterruptTarget.Prio.ToString();
                var iSpell = TargetManager.InterruptTarget.Spell.Name;

                //Can't cast
                if (!Woosh(spell, target, true, false))
                    return RunStatus.Failure;
                //Requirements
                if (!(reqs == null || reqs(ret)))
                    return RunStatus.Failure;

                //Cast
                if (SpellManager.Cast(spell, target))
                {
                    Logger.InterruptLog("[{0}][Priority: {1}] Interrupting [{2}]'s {3} ", spell.Name, target, iSpell);
                    LastTarget = target;
                    LastCast = spell;
                    return RunStatus.Success;
                } //Fail
                return RunStatus.Failure;
            });
        }

        #endregion Casting : Interrupt

        #region Casting : Dispel

        /// <summary>
        /// Shorter version of saying Cast(onUnit=> DispelTarget
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="symbiosis"></param>
        /// <param name="reqs"></param>
        /// <returns></returns>
        internal static Composite Dispel(WoWSpell spell, bool symbiosis = false, Root.Selection<bool> reqs = null)
        {
            //WoWUnit target = onUnit.Target;
            return new Action(ret =>
            {
                var target = TargetManager.DispelTarget.Unit;
                var auraname = TargetManager.DispelTarget.Dispelling;

                //Can't cast
                if (!Woosh(spell, target, true, false))
                    return RunStatus.Failure;
                //Requirements
                if (!(reqs == null || reqs(ret)))
                    return RunStatus.Failure;
                //Cast
                if (SpellManager.Cast(spell, target))
                {
                    Logger.InterruptLog("[{0}][Dispelling {1}'s [2} ", spell.Name, target.SafeName, auraname);
                    LastTarget = target;
                    LastCast = spell;
                    return RunStatus.Success;
                }
                return RunStatus.Failure;
            }
                    );
        }

        #endregion Casting : dispel

        #region Casting : Shapeshift

        internal static Composite Shapeshift(WoWSpell spell, bool powershift, Root.Selection<bool> reqs = null, string reason = null, params object[] args)
        {
            return //new Throttle(
                new Decorator(ret => (reqs == null || reqs(ret)) && Woosh(spell, StyxWoW.Me, false, false), new Action(delegate
                {
                    switch (powershift)
                    {
                        case true:
                            if (SpellManager.Buff(spell))
                            {
                                Logger.TargetLog("[ShapeShift] [Form: {0}] [Reason: {1}]", spell.Name, reason, args);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        case false:
                            if (StyxWoW.Me.HasAura(spell.Id)) //Check if we are in this shape   
                                return RunStatus.Failure;

                            if (SpellManager.Buff(spell)) //Enter Shape
                            {
                                Logger.TargetLog("[ShapeShift] [Form: {0}] [Reason: {1}]", spell.Name, reason, args);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                    }
                    return RunStatus.Failure;
                }));
        }

        #endregion Casting : Shapeshift

        #region Casting : Cancel

        internal static Composite CancelCast(CanRunDecoratorDelegate cond, string reason = null, params object[] args)
        {
            return new Decorator(a => IsCastingorChanneling() && (cond == null || cond(a)),
                new Action(ret =>
                {
                    Logger.CancelLog("Cancelling {0} - [{1}]", StyxWoW.Me.CastingSpell.Name, reason, args);
                    SpellManager.StopCasting();

                    return RunStatus.Success;
                }));
        }

        /// <summary>
        /// To be used with our cast method
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Composite CancelCast(string reason = null, params object[] args)
        {
            return new Action(ret =>
            {
                if (!String.IsNullOrEmpty(reason))
                    Logger.CancelLog("Cancelled {0} : {1}", StyxWoW.Me.CastingSpell.Name, reason, args);
                SpellManager.StopCasting();

                return RunStatus.Success;
            });
        }

        internal static Composite CancelCast(WoWSpell spell, CanRunDecoratorDelegate cond, RunStatus status, string reason = null, params object[] args)
        {
            return new Decorator(ret => IsCastingorChanneling() && StyxWoW.Me.CastingSpellId == spell.Id && (cond == null || cond(ret)), new Action(ret =>
            {
                Logger.CancelLog("Cancelling {0} - [{1}]", StyxWoW.Me.CastingSpell.Name, reason, args);
                SpellManager.StopCasting();

                return status;
            }));
        }

        #endregion Casting : Cancel

        #region Casting : CastOnGround

        internal static Composite CastOnGround(WoWSpell spell, Root.LocationRetriever onLocation, Root.Selection<bool> reqs = null, bool waitForSpell = false, string reason = null)
        {
            return new Decorator(ret => onLocation != null && (reqs == null || reqs(ret)) && StyxWoW.Me.Location.Distance(onLocation(ret)) < spell.MaxRange && GameWorld.IsInLineOfSpellSight(StyxWoW.Me.GetTraceLinePos(), onLocation(ret)),
                new Sequence(
                    new Action(ret => SpellManager.Cast(spell)),
                    new DecoratorContinue(ctx => waitForSpell,
                        new WaitContinue(1, ret => StyxWoW.Me.CurrentPendingCursorSpell != null && StyxWoW.Me.CurrentPendingCursorSpell.Id == spell.Id, new ActionAlwaysSucceed())),
                        new Action(ret => Logger.CombatLog("[{0}][On: Ground] [Reason: {1}]", spell.Name, reason)),
                        new Action(ret => SpellManager.ClickRemoteLocation(onLocation(ret)))));
        }

        #endregion Casting : CastOnGround

        #region Extension : Initialize

        /// <summary>
        /// Initializes the CastManager
        /// </summary>
        internal static void Initialize()
        {
            try
            {
                //Combatlog
                CombatLogHandler.Register("SPELL_CAST_FAILED", UserCast);
                //GlobalCOoldown Tweak
                _gcdSpell = WoWSpell.FromId(1); //TODO: Change this
            }
            catch (Exception e)
            {
                Logger.FailLog("Exception thrown in CastManager.Initialize: {0}", e);
            }
        }
        #endregion

        #region Extension : CooldownTracker

        /// <summary>
        /// Checks if spell is on cooldown
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public static bool IsOnCooldown(this WoWSpell spell)
        {
            using (new Performance.Block("IsOnCooldown()", LogCategory.CastManager))
            {
                try
                {
                    if (spell == null)
                        return true;

                    if (Lists.NoCooldowns.Contains(spell.Id))
                        return false;


                    return spell.GetSpellCooldown().WaitTime.TotalMilliseconds > MyLatency;
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at CooldownTracker.IsOnCooldown: {0}", e);
                    return true;
                }
            }
        }

        /// <summary>
        /// Checks for cooldowntimeleft on a spell
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public static TimeSpan CooldownTimeLeft(this WoWSpell spell)
        {
            using (new Performance.Block("CooldownTimeLeft()", LogCategory.CastManager))
            {
                try
                {
                    if (spell == null)
                        return TimeSpan.MaxValue;

                   // if (Lists.NoCooldown.Contains(spell.Id))
                   //     return TimeSpan.FromSeconds(0.0);

                    return spell.GetSpellCooldown().WaitTime;
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at CooldownTracker.IsOnCooldown()TimeLeft: {0}", e);
                    return spell != null ? spell.CooldownTimeLeft : TimeSpan.MaxValue;
                }
            }
        }

        /// <summary>
        /// Tweaked Globalcooldown
        /// </summary>
        /// <returns></returns>
        internal static bool IsGlobalCooldown()
        {
            using (new Performance.Block("GlobalCooldown", LogCategory.CastManager))
            {
                try
                {
                    var latency = ShouldUseLatency ? MyLatency : 0;
                    var gcdTimeLeft = GlobalCooldownLeft.TotalMilliseconds; //TODO: TEst
                    return gcdTimeLeft > latency;
                }

                catch
                {
                    return SpellManager.GlobalCooldown;
                }
            }
        }
        /// <summary>
        /// Modified Globalcooldown left
        /// </summary>
        internal static TimeSpan GlobalCooldownLeft
        {
            get
            {
                using (new Performance.Block("GlobalCooldownLeft", LogCategory.CastManager))
                {
                    try
                    {
                        return _gcdSpell.CooldownTimeLeft;
                    }
                    catch // (Exception) all
                    {
                        return SpellManager.GlobalCooldownLeft;
                    }
                }
            }
        }

        #region Process Cast
        /// <summary>
        /// Returns a cached cooldown waittime
        /// </summary>
        /// <param name="item"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static WaitTimer GetSpellCooldown(this WoWSpell item, int expiry = 1000)
        {
            //Catch Nulls
            if (item == null)
                return new WaitTimer(TimeSpan.MaxValue);

            //return new WaitTimer(item.CooldownTimeLeft);
            string cacheKey = "GSC" + item.Id;

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

        #endregion

        #endregion

        #region Extension : Woosh
        /// <summary>
        /// My precious!
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="onUnit"></param>
        /// <param name="rangeCheck"></param>
        /// <param name="checkMovement"></param>
        /// <returns></returns>
        private static bool Woosh(WoWSpell spell, WoWUnit onUnit, bool rangeCheck, bool checkMovement)
        {
            try
            {
                #region Checks: ManualCast Handler

                if (!Root.PauseTimer.IsFinished)
                    return false;
                #endregion

                #region Checks:  Valid Unit & Valid spell

                //Check for errors
                if (!spell.CachedHasSpell())
                    return false;

                #endregion
                
                #region Checks : Mana/Energy/Rage

                if (StyxWoW.Me.CurrentMana < spell.PowerCost)
                {
                    //  Logger.DevLog("Woosh.Mana({0},{1}) == FALSE", spell.Name, spell.PowerCost);
                    return false;
                }

                #endregion

                #region Checks : Spell distance

                //Range check
                if (rangeCheck && spell.HasRange && !onUnit.IsMe)
                {
                    var distance = onUnit.CachedDistance();
                    if (distance > spell.ActualMaxRange(onUnit))
                        return false;
                    if (distance < spell.ActualMinRange(onUnit))
                        return false;
                }

                #endregion

                #region Checks : Cooldown

                switch (ShouldUseLatency)
                {
                    case true:
                        if (CooldownTimeLeft(spell).TotalMilliseconds > MyLatency)
                        {
                            //  Logger.DevLog("Woosh.IsOnCooldown()({0},{1}) == TRUE", spell.Name, spell.CooldownTimeLeft());
                            return false;
                        }
                        break;
                    case false:
                        if (IsOnCooldown(spell))
                        {
                            // Logger.DevLog("Woosh.IsOnCooldown()({0},{1}) == TRUE", spell.Name, spell.CooldownTimeLeft());
                            return false;
                        }
                        break;
                    default:
                        if (spell.IsOnCooldown()) return false;
                        break;
                }

                #endregion

                #region Checks : LineofSight / Range / Auras / Alive

                //LineofSight + Distance 
             //   if (!onUnit.CachedLoS()) TODO: Enable if neccesary, bind to override if wanted
                //    return false;

                #endregion

                #region Checks : Movement

                if (checkMovement)
                {
                    var value = spell.CastTime <= 0 || !StyxWoW.Me.IsMoving() || StyxWoW.Me.HasCachedAura(110806) ||
                                StyxWoW.Me.Shapeshift == ShapeshiftForm.TreeOfLife &&
                                new[] { 8936, 339 }.Contains(spell.Id);
                    //   if (value)
                    // Logger.DevLog("Woosh.CanCast({0}) == true", spell.Name);
                    return value;
                }

                #endregion

                return true;
            }
            catch //Eat allexceptions
            {
                //Logger.FailLog("Exception thrown at Castmanager.Woosh: {0}", e);
                return !onUnit.Error299() && SpellManager.CanCast(spell, onUnit, rangeCheck, checkMovement, ShouldUseLatency);
            }
        }

        #endregion Extension : Woosh

        #region Extension : Combatlog

        #region Tidy : UserCastList
        private static readonly HashSet<int> UserCastsSet = new HashSet<int>
        { 
            33786,//Cyclone
            339, //Entangling roots
            2637, // Hibernate
            20484, // rebirth
            50769, // revive
            110309, // symbiosis
            740, // tranquility
            5176, // wrath
            99, //Disorienting Roar
            102793,// ursols vortex
            5211, //Mighty Bash
            132469, // Typhoon
            102359, // Mass Entanglement
            5420, // Incarnation
            33891, //Incarnation
            106731, //Incarnation
            132158,//Nature's Swiftness
            88423, // Nature's Cure
             1850,//Dash
             102280,//Displacer Beast
             16689,//Nature's Grasp
             108294,//Hearth of the Wild
             102401,//Wild Charge
             16979,// Wild Charge
             49376,//Wild Charge
             5211,//Mighty Bash
             102793//Ursols Vortex
            

        };
        #endregion

        #region Tidy : CombatLogHandlers
        /// <summary>
        /// Casts spell based on users last spell
        /// </summary>
        /// <param name="e"></param>
        private static void UserCast(CombatLogEventArgs e)
        {
            try
            {
                if (e.SourceGuid != Root.MyGuid) return;

                switch (e.FailedType)
                {
                    case "Not yet recovered":
                        if (LastCast.Id != e.SpellId && LastTarget.Guid != e.DestGuid && UserCastsSet.Contains(e.SpellId))
                        {
                            Logger.InterruptLog("CastManager.HandleCasts : Manual Cast Detected ({0}) -- Pausing for 1.1s", e.SpellName);
                            Root.PauseTimer = new WaitTimer(TimeSpan.FromSeconds(1.5));
                            Root.PauseTimer.Reset();

                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                Logger.DebugLog("HealEngine.UserCast threw a error  : {0}", ex);
            }
        }

        #endregion

        #endregion

        #region Extension : IsMoving

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit, bool keys = false)
        {
            if (keys)
            {
             /*  TODO: Add Settings for ur Hotkey overrides
              * var movKeys = new[] { MySettings.Global.Instance.MKeyForward, MySettings.Global.Instance.MKeyBackward, MySettings.Global.Instance.MKeyLeft, MySettings.Global.Instance.MKeyRight };
                if (HotkeyManager.IsAnyKeyDown(movKeys))
                    return true;*/
            }
            //Else I'm moving normally ?
            return unit.MovementInfo.MovingBackward || unit.MovementInfo.MovingForward ||
                   unit.MovementInfo.MovingStrafeLeft || unit.MovementInfo.MovingStrafeRight;
        }

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot, and allows specifying how long you've been moving for before accepting it as actually moving.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="movingDuration">Duration in MS how long the unit has been moving before accepting it as a moving unit</param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit, int movingDuration)
        {
            return unit.IsMoving() && unit.MovementInfo.TimeMoved >= movingDuration;
        }
        #endregion

        #region Extension : Min/Max Range
        /// <summary>
        ///  Returns maximum spell range based on hitbox of unit.
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="unit"></param>
        /// <returns>Maximum spell range</returns>
        internal static float ActualMaxRange(this WoWSpell spell, WoWUnit unit)
        {
            if (spell.MaxRange.Equals(0))
                return 0;
            // 0.3 margin for error
            return unit != null ? spell.MaxRange + unit.CombatReach + 1f : spell.MaxRange;
        }

        /// <summary>
        /// Returns minimum spell range based on hitbox of unit. 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="unit"></param>
        /// <returns>Minimum spell range</returns>

        public static float ActualMinRange(this WoWSpell spell, WoWUnit unit)
        {
            if (Math.Abs(spell.MinRange) < 0)
                return 0;

            // some code was using 1.66666675f instead of Me.CombatReach ?
            return unit != null ? spell.MinRange + unit.CombatReach + StyxWoW.Me.CombatReach + 0.1f : spell.MinRange;
        }
        #endregion

        #region Extension : HasSpell

        /// <summary>
        /// Cached version of inlineofSpellsight
        /// </summary>
        /// <returns></returns>
        internal static bool CachedHasSpell(this WoWSpell spell)
        {
            try
            {
                return GetCachedHasSpell(spell);
            }
            catch (Exception e)
            {
                Logger.DebugLog("Exception thrown at Unit.CachedHasSpell: {0}", e);
                return SpellManager.HasSpell(spell);
            }
        }

        /// <summary>
        /// Gets the cached Los Check
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        private static bool GetCachedHasSpell(this WoWSpell spell, int expiry = 10000)
        {

            if (spell == null || !spell.IsValid) return false;

            string cacheKey = "HS" + spell.Id;

            // Check the cache
            var spellStatus = CacheManager.Get<Root.MathCache>(cacheKey);

            //If not cached yet, fill cache
            if (spellStatus == null)
            {
                // Go and retrieve the data from the objectManager
                spellStatus = new Root.MathCache { Bool = SpellManager.HasSpell(spell) };

                // Then add it to the cache so we
                // can retrieve it from there next time
                // set the object to expire
                CacheManager.Add(spellStatus, cacheKey, expiry);
                //  Logger.DebugLog("[Re-Build] cachedObject({0}) at : {1}", cacheKey, DateTime.Now);
            }
            return spellStatus.Bool;
        }

        #endregion

        #region Extension : Wait

        /// <summary>
        /// Allows waiting for SleepForLagDuration() but ending sooner if condition is met
        /// </summary>
        /// <param name="orUntil">if true will stop waiting sooner than lag maximum</param>
        /// <returns></returns>
        internal static Composite CreateWaitForLagDuration(CanRunDecoratorDelegate orUntil)
        {
            return new WaitContinue(TimeSpan.FromMilliseconds(150), orUntil, new ActionAlwaysSucceed());
        }


        /// <summary>
        /// Allows waiting for a custom duration
        /// </summary>
        /// <param name="waittime"></param>
        /// <param name="orUntil">if true will stop waiting sooner than lag maximum</param>
        /// <returns></returns>
        internal static Composite CreateWaitForDuration(TimeSpan waittime, CanRunDecoratorDelegate orUntil)
        {
            return new WaitContinue(waittime, orUntil, new ActionAlwaysSucceed());
        }

        /// <summary>
        /// Wait for cast , gcd, or channel
        /// </summary>
        /// <returns></returns>
        internal static Composite WaitForGcdOrCastOrChannel()
        {
            return new PrioritySelector(WaitForGlobalCooldown(), WaitForCast(), WaitForChannel());
        }

        /// <summary>
        /// Wait for cast or channel
        /// </summary>
        /// <returns></returns>
        internal static Composite WaitForCastOrChannel()
        {
            return new PrioritySelector(WaitForCast(), WaitForChannel());
        }

        public static Composite WaitForGlobalCooldown()
        {
            return new PrioritySelector(new Action(ret => IsGlobalCooldown() ? RunStatus.Success : RunStatus.Failure));
        }

        /// <summary>
        ///     Wait for Cast # Mirabis
        /// </summary>
        /// <returns> Runstatus</returns>
        private static Composite WaitForCast()
        {
            return new Action(ret => IsCasting() ? RunStatus.Success : RunStatus.Failure);
        }

        /// <summary>
        ///     Wait for Channel # Mirabis
        /// </summary>
        /// <returns></returns>
        private static Composite WaitForChannel()
        {
            return new Action(ret => IsChannelling() ? RunStatus.Success : RunStatus.Failure);
        }

        /// <summary>
        ///     True/false if questing
        /// </summary>
        /// <returns></returns>
        private static bool IsCasting()
        {
            try
            {
                if (!StyxWoW.Me.IsCasting)
                    return false;

                if (StyxWoW.Me.ChannelObjectGuid > 0)
                    return false;

                if (ShouldUseLatency && StyxWoW.Me.CurrentCastTimeLeft.TotalMilliseconds < MyLatency)
                    return false;

                return true;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        ///     True/false if Casting
        /// </summary>
        /// <returns></returns>
        private static bool IsChannelling()
        {
            if (!StyxWoW.Me.IsChanneling)
                return false;

            if (StyxWoW.Me.ChannelObjectGuid > 0)
                return false;

            if (ShouldUseLatency && StyxWoW.Me.CurrentChannelTimeLeft.TotalMilliseconds < MyLatency)
                return false;

            return true;
        }

        /// <summary>
        ///     True/False if GCD is Active
        /// </summary>
        /// <returns></returns>
        internal static bool IsCastingorChanneling()
        {
            return IsChannelling() || IsCasting();
        }
        #endregion Extension : Wait

        #region Extension : Pulse

        /// <summary>
        /// Updates our Latency
        /// </summary>
        internal static void UpdateLatency()
        {
            MyLatency = Math.Min(((int)StyxWoW.WoWClient.Latency << 1), 401);
            ShouldUseLatency = MyLatency <= 120;
        }
        /// <summary>
        /// Contains our latency
        /// </summary>
        public static int MyLatency { get; private set; }

        /// <summary>
        /// Wheter we should use latency
        /// </summary>
        public static bool ShouldUseLatency { get; private set; }
        #endregion

        #region Extension : CastTime

        internal static TimeSpan GetSpellCastTime(WoWSpell s)
        {
            return new TimeSpan(s.CastTime); //TimeSpan.FromMilliseconds(s.CastTime);
        }

        #endregion Extension : CastTime

        #region Extension : CancelAura

        /// <summary>
        ///     Cancel/Remove a aura on x unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="aura"></param>
        internal static void CancelAura(this WoWUnit unit, string aura)
        {
            WoWAura a = unit.GetAuraByName(aura);
            if (a != null && a.Cancellable)
                a.TryCancelAura();
        }

        #endregion Extension : CancelAura
    }

    // ReSharper restore ImplicitlyCapturedClosure
}
