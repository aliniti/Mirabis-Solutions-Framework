using System;
using System.Windows.Media;
using Styx.Common;
using Styx.WoWInternals;

namespace Miracle_Business_Solutions_Framework.Extensions
{
    /// <summary>
    /// Logging output class
    /// </summary>
    internal static class Logger
    {
        #region Tidy : Base Method
        /// <summary>
        /// Logging capacityquee
        /// </summary>
        private static readonly CapacityQueue<string> LogQueue = new CapacityQueue<string>(5);

        /// <summary>
        /// Output override function
        /// </summary>
        /// <param name="level"></param>
        /// <param name="color"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        private static void Output(LogLevel level, Color color, string format, params object[] args)
        {
            if (LogQueue.Contains(string.Format(format, args))) return;
            LogQueue.Enqueue(string.Format(format, args));

            Logging.Write(level, color, string.Format("[{0}]: {1}", DateTime.Now.ToString("ss:fff"), format), args);
        }

        #endregion

        #region Tidy : Usage Functions
        /// <summary>
        /// Normal initialization output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void InitLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.SeaGreen, message, args);
        }

        /// <summary>
        /// Normal item output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void ItemLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.DodgerBlue, message, args);
        }

        /// <summary>
        /// Normal failure output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void FailLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.DarkOrange, message, args);
        }

        /// <summary>
        /// Normal casting output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void CombatLog(string message, params object[] args)
        {
            Logging.Write(LogLevel.Normal, Colors.MediumSeaGreen,
                string.Format("[{0}]: {1}", DateTime.Now.ToString("ss:fff"), message), args);
        }

        /// <summary>
        /// Normal developer output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void DevLog(string message, params object[] args)
        {
            //TODO: Add settings for Developer Mode
            if (true)
                Output(LogLevel.Normal, Colors.MediumOrchid, message, args);
        }

        /// <summary>
        /// Normal interrupt output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void InterruptLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.Yellow, message, args);
        }

     /// <summary>
     /// Pet Logging
     /// </summary>
     /// <param name="message"></param>
     /// <param name="args"></param>
        internal static void PetLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.Tomato, message, args);
        }

        /// <summary>
        /// Normal cancelling output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void CancelLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.OrangeRed, message, args);
        }

        /// <summary>
        /// Normal dispel output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void Dispelog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.DeepPink, message, args);
        }

        /// <summary>
        /// Verbose target output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void TargetLog(string message, params object[] args)
        {
            Output(LogLevel.Verbose, Colors.CornflowerBlue, message, args);
        }

        /// <summary>
        /// Diagnostic debug output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void DebugLog(string message, params object[] args)
        {
            Output(LogLevel.Diagnostic, Colors.Firebrick, message, args);
        }

        /// <summary>
        /// Normal Performance output
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void PerfLog(string message, params object[] args)
        {
            Output(LogLevel.Normal, Colors.GreenYellow, message, args);
        }

        /// <summary>
        /// Prints on honorbuddy log and the wow chat frame if wanted
        /// </summary>
        /// <param name="template"></param>
        /// <param name="args"></param>
        internal static void TellUser(string template, params object[] args)
        {
            string msg = string.Format(template, args);
            ItemLog(template, args);
            //TODO: Add Settings to enable in-game printing of text
            if (true)
                Lua.DoString(string.Format("print('{0}!')", msg));

            ItemLog(template, args);
        }
        #endregion
    }
}