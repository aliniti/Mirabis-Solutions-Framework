using System.Collections.Generic;
using Miracle_Business_Solutions_Framework.Base;
using Miracle_Business_Solutions_Framework.Extensions;
using Miracle_Business_Solutions_Framework.Managers;
using Styx;
using Styx.TreeSharp;
using CM = Miracle_Business_Solutions_Framework.Managers.CastManager;
using SB = Miracle_Business_Solutions_Framework.Base.Spellbook;
using TM = Miracle_Business_Solutions_Framework.Managers.TalentManager;
using T = Miracle_Business_Solutions_Framework.Managers.TargetManager;

namespace Miracle_Business_Solutions_Framework.Rotations
{
    static class Specialization2
    {
        #region Priority : Combat Selector

        /// <summary>
        /// Picks the appropriate composite to run
        /// </summary>
        internal static Composite Rotation
        {
            get
            {
                return new Performance.FrameLock(
                     CastManager.WaitForCastOrChannel(),
                    Common.HandleItems,
                    OffGlobal(),
                    CastManager.WaitForGlobalCooldown(),
                    /* Point to rotation in diff composites or
                     * build the priority right here
                     * up to u.
                     */

                    AoEAttacks(),
                    SingleAttacks()
                    );
            }
        }

        /// <summary>
        /// Single Target
        /// </summary>
        /// <returns></returns>
        private static Composite SingleAttacks()
        {
            return new PrioritySelector(
                //Spells Here
                CM.Cast(on => T.DrinkingEnemy, SB.Example, req => StyxWoW.Me.HasAura(2) && StyxWoW.Me.HasAnyCachedAura(new HashSet<int> { 1, 2, 3, 4, 5, }), "I have 1,2,3,4,5 Aura ;",
                req => StyxWoW.Me.HasAura("BitchStop!"), "Bitch told us to stop")
                );
        }

        /// <summary>
        /// Area of Effect
        /// </summary>
        /// <returns></returns>
        private static Composite AoEAttacks()
        {
            return new Decorator(req => CHECKIFAOEISNEEDED,
                new PrioritySelector(
                //Spells Here
                 CM.CastOnGround(SB.Example, on => T.DrinkingEnemy.Location, req => T.DrinkingEnemy.CachedDistance() <= 25, false, "Boss in range")
                ));
        }


        /// <summary>
        /// Spells with no global cooldown lockouts
        /// </summary>
        /// <returns></returns>
        private static Composite OffGlobal()
        {
            return new PrioritySelector(
                //Spells Here

                );
        }

        #endregion Priority : Combat Selector

    }
}
