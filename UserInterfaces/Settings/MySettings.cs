using System;
using Styx;
using Styx.Common;

namespace Miracle_Business_Solutions_Framework.UserInterfaces.Settings
{
    /// <summary>
    ///  Combat Routin Settings
    /// </summary>
    internal class MySettings : Styx.Helpers.Settings
    {
        #region Tidy : Styx.Helpers.Settings
        private static string SettingsPath(string e)
        {
            return String.Format("{0}\\Routines\\ROUTINENAME\\Settings\\{1}_{2}", Utilities.AssemblyDirectory, StyxWoW.Me.Name, e);
        }

         private static MySettings _instance;

            internal static MySettings Instance
            {
                get { return _instance ?? (_instance = new MySettings()); }
            }

            private MySettings()
                : base(SettingsPath("Global") + ".config")
            {
            }
        #endregion

        #region Tidy : Category1


        #endregion

        #region Tidy : Category2


        #endregion

        #region Tidy : Category3


        #endregion

        #region Tidy : Category4


        #endregion

        #region Tidy : Category5


        #endregion

        #region Tidy : Category6


        #endregion
    }
}
