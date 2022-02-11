using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Styx;
using Styx.Common.Helpers;
using Styx.WoWInternals;

namespace Miracle_Business_Solutions_Framework.Managers
{
    internal static class TalentManager
    {
        static TalentManager()
        {
            Talents = new List<Talent>();
            TalentId = new int[6];
            Glyphs = new HashSet<string>();
            GlyphId = new int[6];
            Lua.Events.AttachEvent("PLAYER_LEVEL_UP", UpdateTalentManager);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("GLYPH_UPDATED", UpdateTalentManager);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("PLAYER_SPECIALIZATION_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("LEARNED_SPELL_IN_TAB", UpdateTalentManager);
        }


        internal static WoWSpec CurrentSpec { get; private set; }

        internal static List<Talent> Talents { get; private set; }

        private static int[] TalentId { get; set; }

        internal static HashSet<string> Glyphs { get; private set; }

        private static int[] GlyphId { get; set; }

        internal static bool IsSelected(int index)
        {
            //return Talents.FirstOrDefault(t => t.Index == index).Selected;
            int tier = (index - 1) / 3;
            if (tier.Between(0, 5))
                return TalentId[tier] == index;
            return false;
        }

        public static bool HasTalent(TalentNames tal) { return IsSelected((int)tal); }

        /// <summary>
        ///   Checks if we have a glyph or not
        /// </summary>
        /// <param name = "glyphName">Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form")</param>
        /// <returns></returns>
        internal static bool HasGlyph(string glyphName)
        {
            return Glyphs.Any() && Glyphs.Contains(glyphName);
        }

        /// <summary>
        /// Called when our talents change
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private static void UpdateTalentManager(object o, LuaEventArgs args)
        {
            var oldSpec = CurrentSpec;
            int[] oldTalent = TalentId;
            int[] oldGlyph = GlyphId;

            // Logger.FailLog("{0} Event Fired!", args.EventName);

            Update();

            if (args.EventName == "PLAYER_LEVEL_UP")
            {
                RebuildNeeded = true;
                Logger.FailLog(" TalentManager: Your character has leveled up! Now level {0}", args.Args[0]);
            }

            if (CurrentSpec != oldSpec)
            {
                RebuildNeeded = true;
                Logger.FailLog(" TalentManager: Your spec has been changed.");
            }

            int i;
            for (i = 0; i < 6; i++)
            {
                if (oldTalent[i] != TalentId[i])
                {
                    RebuildNeeded = true;
                    Logger.FailLog(" TalentManager: Your talents have changed.");
                    break;
                }
            }

            for (i = 0; i < 6; i++)
            {
                if (oldGlyph[i] != GlyphId[i])
                {
                    RebuildNeeded = true;
                    Logger.FailLog(" TalentManager: Your glyphs have changed.");
                    break;
                }
            }
        }

        internal static void Update()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;

                Talents.Clear();
                TalentId = new int[6];

                // Always 18 talents. 6 rows of 3 talents.
                for (int index = 1; index <= 6 * 3; index++)
                {
                    var selected =
                        Lua.GetReturnVal<bool>(
                            string.Format(
                                "local t= select(5,GetTalentInfo({0})) if t == true then return 1 end return nil", index),
                            0);
                    var t = new Talent { Index = index, Selected = selected };
                    Talents.Add(t);

                    if (selected)
                        TalentId[(index - 1) / 3] = index;
                }

                Glyphs.Clear();
                GlyphId = new int[6];

                // 6 glyphs all the time. Plain and simple!
                for (int i = 1; i <= 6; i++)
                {
                    List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                    // add check for 4 members before access because empty sockets weren't returning 'nil' as documented
                    if (glyphInfo != null && glyphInfo.Count >= 4 && glyphInfo[3] != "nil" &&
                        !string.IsNullOrEmpty(glyphInfo[3]))
                    {
                        GlyphId[i - 1] = int.Parse(glyphInfo[3]);
                        Glyphs.Add(WoWSpell.FromId(GlyphId[i - 1]).Name.Replace("Glyph of ", ""));
                    }
                }
            }

        }


        private static readonly WaitTimer EventRebuildTimer = new WaitTimer(TimeSpan.FromSeconds(1));

        private static bool _Rebuild;

        private static bool RebuildNeeded
        {
            get
            {
                return _Rebuild;
            }
            set
            {
                _Rebuild = value;
                EventRebuildTimer.Reset();
            }
        }

        public static bool Pulse()
        {
            if (EventRebuildTimer.IsFinished && RebuildNeeded)
            {
                RebuildNeeded = false;
                Logger.FailLog(" TalentManager: Rebuilding behaviors due to changes detected.");
                Update();
                return true;
            }

            return false;
        }





        #region Nested type: Talent

        public struct Talent
        {
            public bool Selected;
            public int Index;
        }

        #endregion


        #region Debug Output
        /// <summary>
        /// Dumps all our Talents
        /// </summary>
        /// <returns></returns>
        internal static string DumpTalents()
        {
            return Talents.Where(talent => talent.Selected).Select(ta => (TalentNames)ta.Index).Aggregate("", (current, talent) => current + (talent + ", "));
        }

        /// <summary>
        /// Dumps all our Glyphs
        /// </summary>
        /// <returns></returns>
        internal static string DumpGlyphs()
        {
            return Glyphs.Aggregate("", (current, glyph) => current + (glyph.ToString(CultureInfo.InvariantCulture) + ", "));
        }

        #endregion
    }
}