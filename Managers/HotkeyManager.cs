using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using Miracle_Business_Solutions_Framework.Extensions;
using Styx.Common;
using Styx.TreeSharp;
using Styx.WoWInternals;

namespace Miracle_Business_Solutions_Framework.Managers
{
    internal static class HotkeyManager
    {
        #region Tidy : Properties

        internal static bool Initialized;
        private static bool _registered;
        private static bool _hotkey1Enabled;
        private static bool _hotkey2Enabled;
        private static bool _hotkey4Enabled;
        private static bool _hotkey3Enabled;
        private static bool _hotkey5Enabled;
        private static bool _hotkey6Enabled;
        private static bool _lastHotkey6Enabled;
        private static bool _lastHotkey2Enabled;
        private static bool _lastHotkey1Enabled;
        private static bool _lastHotkey4Enabled;
        private static bool _lastHotkey3Enabled;
        private static bool _lastHotkey5Enabled;

        #endregion

        #region Tidy : Register/Unregister Keys

        /// <summary>
        ///     Registers the Keys
        /// </summary>
        internal static void RegisterKeys()
        {
            if (_registered) return;

            HotkeysManager.Register("Hotkey1", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            HotkeysManager.Register("Hotkey2", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            HotkeysManager.Register("Hotkey3", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            HotkeysManager.Register("Hotkey4", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            HotkeysManager.Register("Hotkey5", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            HotkeysManager.Register("Hotkey6", Keys.A, ModifierKeys.Control, ret =>
            {
                // Do something
            });

            //At End
            _registered = true;
            //TODO: Create Settings wheter to print in-wow chat or not
            if (true)
                Lua.DoString(@"print('[ROUTINENAME][Hotkeys] \124cFF15E61C registered!')");
            Logging.Write(LogLevel.Normal, Colors.ForestGreen, "Hotkeys Registered!");
        }

        /// <summary>
        /// Removes all the hooked keys
        /// </summary>
        internal static void RemoveKeys()
        {
            if (!_registered) return;
            HotkeysManager.Unregister("Hotkey1");
            HotkeysManager.Unregister("Hotkey2");
            HotkeysManager.Unregister("Hotkey3");
            HotkeysManager.Unregister("Hotkey4");
            HotkeysManager.Unregister("Hotkey5");
            HotkeysManager.Unregister("Hotkey6");
            Lua.DoString(@"print('[ROUTINENAME][Hotkeys] \124cFFE61515 removed! Bools defaulted to false...')");
            Logger.InitLog("Hotkeys Removed!");
        }

        /// <summary>
        /// Registers our AsyncHotkeys
        /// </summary>
        internal static void RegisterAsyncHotkeys()
        {
            /*   TODO: Create Settings and Link them
             * if (GST.DamKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey1", GST.DamKey, hk => Hotkey1Toggle());

              if (GST.TankKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey2", GST.TankKey, hk => Hotkey2Toggle());
                          
              if (GST.ERKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey3", GST.ERKey, hk => Hotkey3Toggle());

              if (GST.CycKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey4", GST.CycKey, hk => Hotkey4Toggle());

              if (GST.ERKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey3", GST.ERKey, hk => Hotkey3Toggle());

              if (GST.HiberKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey5", GST.HiberKey, hk => Hotkey5Toggle());

              if (GST.MushKey != Keys.None)
                  RegisterHotkeyAssignment("Hotkey6", GST.MushKey, hk => Hotkey6Toggle()); */
        }

        /// <summary>
        /// Registers the key + Modifier keys
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        private static void RegisterHotkeyAssignment(string name, Keys key, Action<Hotkey> callback)
        {
            Keys keyCode = key & Keys.KeyCode;
            ModifierKeys mods = 0;

            if ((key & Keys.Shift) != 0)
                mods |= ModifierKeys.Shift;
            if ((key & Keys.Alt) != 0)
                mods |= ModifierKeys.Alt;
            if ((key & Keys.Control) != 0)
                mods |= ModifierKeys.Control;

            if (mods != 0)
            {
                Logger.ItemLog("Hotkey: {0} registered as [{1}]", name, key.ToFormattedString());
                HotkeysManager.Register(name, keyCode, mods, callback);
            }

            else
            {
                Logger.InitLog(
                    "warning: {0} cannot be a hotkey for {1}!  HonorBuddy now requires you to add a Shift, Alt, or Control modifier key to work.  For example, change your config to use Shift+{0}",
                    key.ToFormattedString(),
                    name);
            }
        }

        #endregion

        #region Tidy : HotkeyHandlers

        internal static void Hotkey1Handler()
        {
            if (_hotkey1Enabled != _lastHotkey1Enabled)
            {
                /*
                _lastHotkey1Enabled = _hotkey1Enabled;
                if (_lastHotkey1Enabled)
                    Logger.TellUser("Damage rotation is now activated");
                else
                    Logger.TellUser("Damage rotation is now disabled... press {0} to enable again", GST.DamKey.ToFormattedString());
                 */
            }
        }


        internal static void Hotkey2Handler()
        {
            if (_hotkey2Enabled != _lastHotkey2Enabled)
            {
                /*
                _lastHotkey2Enabled = _hotkey2Enabled;
                if (_lastHotkey2Enabled)
                    Logger.TellUser("Tank rotation is now activated");
                else
                    Logger.TellUser("Tank rotation is now disabled... press {0} to enable again", GST.TankKey.ToFormattedString());
                 * */
            }
        }


        internal static void Hotkey6Handler()
        {
            if (_hotkey6Enabled != _lastHotkey6Enabled)
            {
                /*
                _lastHotkey6Enabled = _hotkey6Enabled;
                if (_lastHotkey6Enabled)
                    Logger.TellUser("Mouseover Mushroom is now activated");
                else
                    Logger.TellUser("Mouseover Mushroom is now disabled... press {0} to enable again",
                        GST.MushKey.ToFormattedString());
                 * */
            }
        }


        internal static void Hotkey4Handler()
        {
            if (_hotkey4Enabled != _lastHotkey4Enabled)
            {
                /*
                _lastHotkey4Enabled = _hotkey4Enabled;
                if (_lastHotkey4Enabled)
                    Logger.TellUser("Cyclone is now activated");
                else
                    Logger.TellUser("Cyclone is now disabled... press {0} to enable again",
                        GST.CycKey.ToFormattedString());
                 * */
            }
        }

        internal static void Hotkey3Handler()
        {
            if (_hotkey3Enabled != _lastHotkey3Enabled)
            {
                /*
                _lastHotkey3Enabled = _hotkey3Enabled;
                if (_lastHotkey3Enabled)
                    Logger.TellUser("Entangling Roots is now activated");
                else
                    Logger.TellUser("Entangling Roots is now disabled... press {0} to enable again",
                        GST.ERKey.ToFormattedString());
                 * */
            }
        }

        internal static void Hotkey5Handler()
        {

            if (_hotkey5Enabled != _lastHotkey5Enabled)
            {
                /*
                _lastHotkey5Enabled = _hotkey5Enabled;
                if (_lastHotkey5Enabled)
                    Logger.TellUser("Hibernate is now activated");
                else
                    Logger.TellUser("Hibernate is now disabled... press {0} to enable again",
                        GST.HiberKey.ToFormattedString());
                 * */
            }
        }

        #endregion

        #region Tidy : State Toggles

        /// <summary>
        /// Toggles the damage hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey1Toggle()
        {
            _hotkey1Enabled = !_hotkey1Enabled;

            return (_hotkey1Enabled);
        }

        /// <summary>
        /// Toggles the Tank hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey2Toggle()
        {
            _hotkey2Enabled = !_hotkey2Enabled;

            return (_hotkey2Enabled);
        }

        /// <summary>
        /// Toggles the cyclone hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey4Toggle()
        {
            _hotkey4Enabled = !_hotkey4Enabled;

            return (_hotkey4Enabled);
        }

        /// <summary>
        /// Toggles the hibernate hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey5Toggle()
        {
            _hotkey5Enabled = !_hotkey5Enabled;

            return (_hotkey5Enabled);
        }

        /// <summary>
        /// Toggles the Mushroom hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey6Toggle()
        {
            _hotkey6Enabled = !_hotkey6Enabled;

            return (_hotkey6Enabled);
        }

        /// <summary>
        /// Toggles the Entangling Roots hotkey
        /// </summary>
        /// <returns></returns>
        private static bool Hotkey3Toggle()
        {
            _hotkey3Enabled = !_hotkey3Enabled;

            return (_hotkey3Enabled);
        }

        #endregion

        #region Tidy : EasyToggles

        internal static bool IsHotkey1Enabled
        {
            get { return _hotkey1Enabled; }
        }

        internal static bool IsHotkey2Enabled
        {
            get { return _hotkey2Enabled; }
        }

        internal static bool IsHotkey3Enabled
        {
            get { return _hotkey3Enabled; }
        }

        internal static bool IsHotkey4Enabled
        {
            get { return _hotkey4Enabled; }
        }

        internal static bool IsHotkey5Enabled
        {
            get { return _hotkey5Enabled; }
        }

        internal static bool IsHotkey6Enabled
        {
            get { return _hotkey6Enabled; }
        }

        /// <summary>
        /// Returns true when any hotkey is enabled
        /// </summary>
        public static bool AnyHotkey
        {
            get
            {
                return IsHotkey6Enabled || IsHotkey4Enabled || IsHotkey2Enabled || IsHotkey1Enabled || IsHotkey5Enabled ||
                       IsHotkey3Enabled;
            }
        }

        #endregion

        #region Tidy : Initialize

        /// <summary>
        /// Initializes the HotkeyManager
        /// </summary>
        internal static void Initialize()
        {
            //  HealingMode = 2; //Hybrid Healing on default
            //dummyhotkey = true;
            RegisterAsyncHotkeys();
            RegisterKeys();
            //    RegisterModifier();
            _hotkey1Enabled = false;
            _hotkey4Enabled = false;
            _hotkey3Enabled = false;
            _hotkey5Enabled = false;
            _hotkey6Enabled = false;
            _lastHotkey6Enabled = false;
            _lastHotkey1Enabled = false;
            _lastHotkey4Enabled = false;
            _lastHotkey3Enabled = false;
            _lastHotkey5Enabled = false;
            _lastHotkey2Enabled = false;

            _registered = true;
            Initialized = true;
        }

        #endregion

        #region Tidy : Pulse

        /// <summary>
        /// Pulses our hotkeys
        /// </summary>
        internal static void Pulse()
        {
            //Check if registered
            if (!_registered)
            {
                RegisterKeys();
                _registered = true;
            }


            //GetValues
            Hotkey1Handler();
            Hotkey2Handler();
            Hotkey3Handler();
            Hotkey4Handler();
            Hotkey5Handler();
            Hotkey6Handler();
        }

        #endregion

        #region Tidy : ExecuteHotkeys

        /// <summary>
        /// Executes the hotkeys when they are enabled
        /// </summary>
        internal static Composite ExecuteHotkeys()
        {
            return new PrioritySelector();
        }

        #endregion

        #region Tidy : KeyBoardPolling
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetAsyncKeyState(int vkey);

        internal static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState((int)key) & 0x8000) != 0;
        }

        /// <summary>
        /// New.. supposed to be faster
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool IsAnyKeyDown(IEnumerable<Keys> key)
        {
            return key.Any(i => ((GetAsyncKeyState((int)i)) & 0x8000) != 0);
        }
        #endregion
    }
}