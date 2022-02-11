using System;
using System.Collections.Generic;
using System.Linq;
using CommonBehaviors.Actions;
using Miracle_Business_Solutions_Framework.Extensions;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace Miracle_Business_Solutions_Framework.Managers
{
    /// <summary>
    /// PetManager Class
    /// </summary>
    internal static class PetManager
    {
        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        private static readonly WaitTimer PetTimer = new WaitTimer(TimeSpan.FromSeconds(2));
        private static readonly WaitTimer CallPetTimer = WaitTimer.OneSecond;
        private static readonly List<WoWPetSpell> PetSpells = new List<WoWPetSpell>();

        /// <summary>
        /// 
        /// </summary>
        private static WoWUnit MyPet
        {
            get { return StyxWoW.Me.Pet; }
        }
        
        /// <summary>Returns the Pet spell cooldown using Timespan (00:00:00.0000000)
        /// gtfo if the Pet dosn't have the spell.</summary>
        /// <param name="name">the name of the spell to check for</param>
        /// <returns>The spell cooldown.</returns>
        internal static TimeSpan PetSpellCooldown(string name)
        {
            WoWPetSpell petAction = PetSpells.FirstOrDefault(p => p.ToString() == name);
            if (petAction == null || petAction.Spell == null)
            {
                return TimeSpan.Zero;
            }

            //Logger.DebugLog(" [PetSpellCooldown] {0} : {1}", name, petAction.Spell.CooldownTimeLeft);
            return petAction.Spell.CooldownTimeLeft;
        }

        /// <summary>
        ///   Calls a pet by name, if applicable.
        /// </summary>
        /// <remarks>
        ///   Created 2/7/2011.
        /// </remarks>
        /// <param name = "petNumber">Number of the pet. This parameter is ignored for mages. Warlocks should pass only the name of the pet. Hunters should pass which pet (1, 2, etc)</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        internal static bool CallPet(string petNumber)
        {
            if (!CallPetTimer.IsFinished)
            {
                return false;
            }

            if (SpellManager.CanCast("Call Pet " + petNumber))
            {
                if (!StyxWoW.Me.GotAlivePet)
                {
                    Logger.InitLog("[Pet] Calling out pet #{0}", petNumber);
                    bool result = SpellManager.Cast("Call Pet " + petNumber);
                    return result;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static Composite CreateHunterCallPetBehavior()
        {
            return new Decorator(
                ret => !Me.GotAlivePet
                       && PetTimer.IsFinished
                       && !Me.Mounted && !Me.OnTaxi,
                new PrioritySelector(
                    new Decorator(ret => (MyPet == null || MyPet.IsDead) /*&& PetSettingsMM.Instance.RevivePet*/,
                        new Sequence(
                            new Action(
                                ret =>
                                    Logger.DebugLog("CallPet: attempting Revive Pet - cancast={0}",
                                        SpellManager.CanCast("Revive Pet"))),
                            CastManager.BuffSelf("Revive Pet"),
                            CastManager.CreateWaitForLagDuration(or => Me.IsCasting),
                            new Wait(TimeSpan.FromMilliseconds(750), ret => Me.IsCasting, new ActionAlwaysSucceed())
                            )
                        ),
                    new Decorator(ret => MyPet == null /* PetSettingsMM.Instance.CallPet != Misc.CallPet.None*/,
                        new Sequence(
                            //new Action(ret => Logger.DebugLog("CallPet: attempting Call Pet {0} - canbuff={1}", PetSettingsMM.Instance.CallPet, SpellManager.CanCast("Call Pet " + Helpers.CallPetNumber.ToString(CultureInfo.InvariantCulture)))),
                            // new Action(ret => CallPet(Helpers.CallPetNumber.ToString(CultureInfo.InvariantCulture))),
                            CastManager.CreateWaitForLagDuration(or => Me.GotAlivePet),
                            new WaitContinue(1, ret => Me.GotAlivePet, new ActionAlwaysSucceed())
                            )
                        )
                    )
                );
        }

        #region Tidy : CastAction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        internal static bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }

            return !petAction.Spell.Cooldown;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        internal static void CastPetAction(string action)
        {
            WoWPetSpell spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.InitLog("[Pet] Casting {0}", action);
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="on"></param>
        internal static void CastPetAction(string action, WoWUnit on)
        {
            // target is currenttarget, then use simplified version (to avoid setfocus/setfocus
            if (on == StyxWoW.Me.CurrentTarget)
            {
                CastPetAction(action);
                return;
            }

            WoWPetSpell spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.CombatLog("[Pet] Casting {0} on {1}", action, on.SafeName);
            WoWUnit save = StyxWoW.Me.FocusedUnit;
            StyxWoW.Me.SetFocus(on);
            Lua.DoString("CastPetAction({0}, 'focus')", spell.ActionBarIndex + 1);
            StyxWoW.Me.SetFocus(save == null ? 0 : save.Guid);
        }

        /// <summary>
        /// behavior form of CastPetAction().  note that this Composite will return RunStatus.Success
        /// if it appears the ability was cast.  this is to trip the Throttle wrapping it internally
        /// -and- to allow cascaded sequences of Pet Abilities.  Note: Pet Abilities are not on the
        /// GCD, so you can safely allow execution to continue even on Success
        /// </summary>
        /// <param name="action">pet ability</param>
        /// <param name="onUnit">unit deleg to cast on (null if current target)</param>
        /// <returns></returns>
        internal static Composite CastAction(string action, Root.UnitSelectionDelegate onUnit = null)
        {
            return new Performance.Throttle(TimeSpan.FromMilliseconds(750),
                new Action(ret =>
                {
                    if (!CanCastPetAction(action))
                        return RunStatus.Failure;

                    WoWUnit target;
                    if (onUnit == null)
                        target = StyxWoW.Me.CurrentTarget;
                    else
                        target = onUnit(ret);

                    if (target == null)
                        return RunStatus.Failure;

                    if (target.Guid == StyxWoW.Me.CurrentTargetGuid)
                        CastPetAction(action);
                    else
                        CastPetAction(action, target);

                    return RunStatus.Success;
                }));
        }
        #endregion
    }
}