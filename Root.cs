using System;
using System.Windows.Forms;
using CommonBehaviors.Actions;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Miracle_Business_Solutions_Framework.Managers;
using Miracle_Business_Solutions_Framework.Rotations;
using Miracle_Business_Solutions_Framework.UserInterfaces;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Miracle_Business_Solutions_Framework
{
    /// <summary>
    /// The root/base of the CombatRoutine
    /// </summary>
    public class Root : CombatRoutine
    {
        #region Tidy : Properties
        /// <summary>
        /// Saves our GUID so we don't have to get it over and over
        /// </summary>
        internal static ulong MyGuid { get; private set; }
        private bool _initialized;

        /// <summary>
        ///     Represents the Combat Routine external version.
        /// </summary>
        private static readonly Version Version = new Version(1, 0, 0, 5);
        /// <summary>
        /// Combat Routine name bundled with the version
        /// </summary>
        private static readonly string _name = String.Format("ROUTINENAME - {0}", Version);
        private Form _tempUi;
        #endregion

        #region Tidy :  Overrides of CombatRoutine

        /// <summary>
        ///     The name of this CombatRoutine
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     The <see cref="T:Styx.WoWClass" /> to be used with this routine
        /// </summary>
        /// <value>
        ///     The class.
        /// </value>
        public override WoWClass Class
        {
            get { return WoWClass.Druid; }
        }

        /// <summary>
        ///     Whether this CC want the button on the form to be enabled.
        /// </summary>
        public override bool WantButton
        {
            get { return _initialized; }
        }

        /// <summary>
        ///     Called upon start
        /// </summary>
        public override void Initialize()
        {
            try
            {
                try
                {
                    Logger.ItemLog("[ROUTINENAME] -- Initialization Started");
                    PauseTimer = new WaitTimer(TimeSpan.Zero);
                    MyGuid = StyxWoW.Me.Guid;
                    CastManager.UpdateLatency();
                    Logger.ItemLog("[ROUTINENAME] -- Latency set to {0}, Use Latency : {1}", CastManager.MyLatency, CastManager.ShouldUseLatency.ToYN());
                    TreeHooks.Instance.ClearAll();
                    CombatLogHandler.Initialize();
                    HotkeyManager.Initialize();
                    TalentManager.Update();
                    Logger.ItemLog("[ROUTINENAME] -- Initialization completed!");


                }
                catch (Exception e)
                {
                    Logger.FailLog("Error thrown at Root.Initialize() : {0}", e);
                    _initialized = false;
                }
                Logger.FailLog("Press [F9] to print Tree Performance");
                Logger.FailLog("Press [F10] to print PerformanceLogger.Values");
                Logger.FailLog("Press [F11] to print FrameLock.Values");
                _initialized = true;

            }
            catch (Exception e)
            {
                //Print that damn Error if it occures
                Logger.FailLog(e.ToString());
            }
        }

        /// <summary>
        ///     Called when the button for this CC is pressed.
        /// </summary>
        public override void OnButtonPress()
        {
            //Settings.Instance.Load();
            if (_tempUi == null || _tempUi.IsDisposed || _tempUi.Disposing) _tempUi = new Interface();
            if (_tempUi != null || _tempUi.IsDisposed) _tempUi.ShowDialog();
        }

        public override void Pulse()
        {
            // No pulsing if we're loading or out of the game.
            if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                return;

            // intense if does work, so return if true
            if (TalentManager.Pulse())
                return;

            //Pulse derrived ones
            CastManager.UpdateLatency();
            HotkeyManager.Pulse();

        }

        /// <summary>
        /// Do shit when routine shuts down
        /// </summary>
        public override void ShutDown()
        {
            if (HotkeyManager.Initialized)
            {
                //  HotkeyManager.RemoveModifier();
                HotkeyManager.RemoveKeys();
            }
            CombatLogHandler.Shutdown();

        }

        #endregion Overrides of CombatRoutine

        #region Tidy :  Behavior Tree

        /// <summary>
        ///     Behavior used in combat.
        /// </summary>
        public override Composite CombatBehavior
        {
            get { return CombatRotation(); }
        }

        /// <summary>
        ///     Behavior used for buffing, regular buffs like 'Power Word: Fortitude', 'MotW' etc..
        /// </summary>
        public override Composite PreCombatBuffBehavior
        {
            get { return PreCombatRotation(); }
        }

        /// <summary>
        /// Pause -> Hotkeys -> Stopcasting if casting -> Heal ( Throttled )
        /// </summary>
        /// <returns></returns>
        private static Composite CombatRotation()
        {
            return new PrioritySelector(
                Performance.Tree("CombatRotation"),
                HotkeyManager.ExecuteHotkeys(),
                new Decorator(req => IsPauseNeeded(), new PrioritySelector(new ActionAlwaysSucceed())),
                   new Switch<WoWSpec>(ctx => StyxWoW.Me.Specialization,
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightBlood, Specialization1.Rotation),
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightFrost, Specialization2.Rotation),
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightUnholy, Specialization3.Rotation))
               );
        }

        /// <summary>
        /// Pause -> Hotkeys -> Stopcasting if casting -> Heal ( Throttled )
        /// </summary>
        /// <returns></returns>
        private static Composite PreCombatRotation()
        {
            return new PrioritySelector(
                Performance.Tree("PreCombatRotation"),
                HotkeyManager.ExecuteHotkeys(),
                new Decorator(req => IsPauseNeeded(), new PrioritySelector(new ActionAlwaysSucceed()))

                  /* new Switch<WoWSpec>(ctx => StyxWoW.Me.Specialization, BUG: Want specifiz precombats use the switch, else just priorityselector
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightBlood, Specialization1.Rotation),
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightFrost, Specialization2.Rotation),
                       new SwitchArgument<WoWSpec>(WoWSpec.DeathKnightUnholy, Specialization3.Rotation)) */
               );
        }

        #endregion Behavior Tree

        #region Tidy :  Delegates
        internal delegate WoWUnit UnitSelectionDelegate(object context);

        internal delegate WoWPoint LocationRetriever(object context);

        public delegate T Selection<out T>(object context);

        internal delegate bool SimpleBooleanDelegate(object context);

        #endregion Delegates

        #region Tidy : PauseFunction
        private static readonly WaitTimer Checkpause = new WaitTimer(TimeSpan.FromMilliseconds(100));
        private static readonly WaitTimer Refreshpause = new WaitTimer(TimeSpan.FromMilliseconds(250));
        internal static bool _isPauseNeeded;

        /// <summary>
        /// True: if Damage spells are allowed, False: healing
        /// </summary>
        internal static bool IsPauseNeeded()
        {
            //Already Paused, check 250ms
            if (_isPauseNeeded)
            {

                if (Refreshpause.IsFinished)
                {
                    //Reset Tree Roots
                    _isPauseNeeded = NeedPause;
                    Refreshpause.Reset();
                }
                return _isPauseNeeded;
            }
            //else 
            if (Checkpause.IsFinished)
            {
                _isPauseNeeded = NeedPause;
                Checkpause.Reset();
            }
            return _isPauseNeeded;


        }

        internal static WaitTimer PauseTimer;
        /// <summary>
        /// DO NOT C
        /// </summary>
        private static bool NeedPause
        {
            get
            {
                try
                {
                    #region Override

                    if (!PauseTimer.IsFinished)
                    {
                        Logger.FailLog("Pause: Manual casting detected");
                        return true;
                    }
                    #endregion

                    #region Common
                    if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld || Battlegrounds.Finished || !StyxWoW.Me.IsValid || !StyxWoW.Me.IsAlive)
                    {
                        Logger.FailLog("Pause: I'm  Not a valid unit");
                        return true;
                    }
                    if (StyxWoW.Me.CastingSpell != null && StyxWoW.Me.CastingSpell.Mechanic == WoWSpellMechanic.Mounted)
                    {
                        Logger.FailLog("Pause: I'm mounting up");
                        return true;
                    }
                    //Prowl , Potion
                    //76092,5215
                    if (StyxWoW.Me.HasAnyAura(new[] { "Food", "Drink", "Potion of Focus" }))
                    {
                        Logger.FailLog("Pause: User interaction detected");
                        return true;
                    }
                    #endregion

                    #region PvP
                    //TODO: Make requirements
                    #endregion

                    #region PvE
                    //TODO: Make requirements
                    #endregion


                    //Nothing found, continue
                    return false;
                }
                catch (Exception e)
                {
                    Logger.FailLog("Exception thrown at Root.NeedPause : {0}", e);
                    return false;
                }
            }
        }
        #endregion

        #region Tidy : MultiPurpose Classes
        /// <summary>
        /// Workarounds for LinQ return values
        /// </summary>
        internal struct IntUnit
        {
            public WoWUnit Target;
            public WoWSpell Spell;
            public WoWAura Aura;
            public string AuraName;
        }

        /// <summary>
        /// Workarounds for the CacheManager return values
        /// </summary>
        internal class MathCache
        {
            public bool Bool;
            public bool Bool2;
            public float Float;
            public float Float2;
            public int Int;
            public int Int2;
            public double Double;
            public double Double2;
        }
        #endregion
    }
}
