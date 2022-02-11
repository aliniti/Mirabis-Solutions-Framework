using Leaves.Misc;
using Styx.TreeSharp;

namespace Miracle_Business_Solutions_Framework.Rotations
{
    /// <summary>
    /// Rotational info multiple specializations share
    /// </summary>
    class Common
    {

        /// <summary>
        /// Handles all Item usages
        /// </summary>
        internal static Composite HandleItems
        {
            get
            {
                return new PrioritySelector(
                    Items.UseEquippedItem(9, ret => Items.NeedGloves()),
                    Items.UseEquippedItem(12, ret => Items.NeedTrinket()),
                    Items.UseEquippedItem(13, ret => Items.NeedTrinket(2)),
                    Items.UseHealthPotions(),
                    Items.UseManaPotions(),
                    Items.UseHealthstone()

                    );
            }
        }
    }
}
