using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Miracle_Business_Solutions_Framework.Extensions
{
    /// <summary>
    /// Miscellaneous convertions 
    /// </summary>
    internal static class TypeConverters
    {
        /// <summary>
        /// Converts Keys to formatted strings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string ToFormattedString(this Keys key)
        {
            string txt = "";

            if ((key & Keys.Shift) != 0)
                txt += "Shift+";
            if ((key & Keys.Alt) != 0)
                txt += "Alt+";
            if ((key & Keys.Control) != 0)
                txt += "Ctrl+";
            txt += (key & Keys.KeyCode).ToString();
            return txt;
        }

        /// <summary>
        /// converts bool to Y or N string
        /// </summary>
        /// <param name="b">bool to convert</param>
        /// <returns></returns>
        internal static string ToYN(this bool b)
        {
            return b ? "Y" : "N";
        }

        /// <summary>
        /// Value between double min and max
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        internal static bool Between(this double distance, double min, double max)
        {
            return distance >= min && distance <= max;
        }

        /// <summary>
        /// Value between floating point min and max
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        internal static bool Between(this float distance, float min, float max)
        {
            return distance >= min && distance <= max;
        }

        /// <summary>
        /// Value between int min and max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        internal static bool Between(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        ///   A string extension method that turns a Camel-case string into a spaced string. (Example: SomeCamelString -> Some Camel String)
        /// </summary>
        /// <remarks>
        ///   Created 2/7/2011.
        /// </remarks>
        /// <param name = "str">The string to act on.</param>
        /// <returns>.</returns>
        internal static string CamelToSpaced(this string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    sb.Append(' ');
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        internal static string RealLuaEscape(string luastring)
        {
            //luastring = Lua.LocalizeSpellName(luastring);
            var bytes = Encoding.UTF8.GetBytes(luastring);
            return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }
    }
}
