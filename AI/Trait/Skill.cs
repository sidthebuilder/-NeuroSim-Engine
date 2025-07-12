using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [System.Serializable]
    public class Skill {
        // Constant representing base XP for skill levels
        // Values may change with development and testing
        public const double ZERO    = 0;
        public const double ONE     = 100;
        public const double TWO     = 100  * 1.58489319246;
        public const double THREE   = 100  * 2.51188643151;
        public const double FOUR    = 100  * 3.98107170554;
        public const double FIVE    = 100  * 6.3095734448;
        public const double SIX     = 1000;
        public const double SEVEN   = 1000 * 1.58489319246;
        public const double EIGHT   = 1000 * 2.51188643151;
        public const double NINE    = 1000 * 3.98107170554;
        public const double TEN     = 1000 * 6.3095734448;
        public static readonly double[] XP_FOR_LEVELS
                = { ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN };
        // The maximum value achievable;
        public const double MAX     = 10000;

        public const double BASE_DECAY_FACTOR = 0.912010839356; // 1 over the 25th root of 10, or 1/5 of a skill level
        public const double BASE_DECAY_FIXED = 100;


        // DATA
        [SerializeField][Range(0,10000)] double xp;
        [SerializeField][Range(0,1000)] float minXp;
        [SerializeField][Range(0,10)] public int level;
        [SerializeField][Range(0,10)] int highestReached;
        [SerializeField][Range(1.0f / 2.51188643151f, 2.51188643151f)] float bonus = 1;
        [SerializeField] double lastUsed;

        public double XP => xp;
        public int Level => level;


        /// <summary>
        /// Handles increases in skill XP for the skill, and raises the skill if needed.
        /// </summary>
        /// <param name="amount">How much the skill has increased (skill XP)</param>
        /// <returns>Any general character XP gain from leveling the skill;
        /// 0 if the skill level did not change</returns>
        public int Increase(float amount) {
            BeUsed();
            int output = 0;
            xp += (amount * bonus);
            if(xp > MAX) xp = MAX;
            if(xp > XP_FOR_LEVELS[level]) {
                level++;
                if(level > highestReached) {
                    highestReached = level;
                    // This is equal to the sum of all numbers from 1 to level
                    output =  (level * (level + 1)) / 2;
                }
            }
            // Decay will not reduce the level to less than 1/2 the highest obtained.
            float newMinimum = Mathf.Sqrt((float)xp);
            if(newMinimum > minXp) minXp = newMinimum;
            // If the character has gained at least one level never loose the skill.
            if((level >= 1) && (minXp < ONE)) minXp = (float)ONE;
            return output;
        }


        [System.Obsolete("Abandoned early mechanic prototype; use RelatativeDecay() as this is the intended " +
        "mechanic.  This may, however, have special usaes?")]
        public int Decay(float amount) {
            xp -= amount;
            if(xp < minXp) xp = minXp;
            if(xp < XP_FOR_LEVELS[level]) level--;
            return level;
        }


        /// <summary>
        /// Removes 1/5 of a skill level worth of XP, to a minimum of the square root of the of the highest XP reached.
        ///
        /// If a skill is not used, practiced, or studied for an extended period of time skill will be lost.
        ///
        /// After a one day grace period, each day one fifth of skill level is lost, or one level every five days.
        /// Thus, this would firest be called after a skill is neglected for two days and every day afterward until it
        /// is used, resulting in lost of one skill level on the sixth day and every fifth day thereacters (until used).
        ///
        /// However, it will never reduce the skill to less than helf the the highest level obtatined, or to less than
        /// one (as long as level 1 was reach) so that a skill is never completely lost once obtained.
        /// </summary>
        /// <param name="amount">The multiplier (should be less than one) for the XP; defaults to the 25th room of 10
        /// (i.e., 1/5 of a level, as a level is based on the 5th root of 10)</param>
        /// <returns>The level of the skill after the reduction</returns>
        public int RelativeDecay(double amount = BASE_DECAY_FACTOR) {
            xp *= amount;
            if(xp < minXp) xp = minXp;
            if(xp < XP_FOR_LEVELS[level]) level--;
            return level;
        }


        // Mot sure if this "worst of both worlds" approach is to harsh of if the other option are too soft in some cases.
        // Note that this might actually be kinder to new players, who would more likely to see the result at at lower
        // skill level, and thus lose less when first hit by the decay mechanic.
        [System.Obsolete("This may be switched to in the future, but at this time RelativeDecay() is the " +
        "intended mechanic.")]
        public int Decay(double factor = BASE_DECAY_FACTOR, double subtraction = BASE_DECAY_FIXED) {
            xp = System.Math.Min(xp * factor, xp - subtraction);
            if(xp < minXp) xp = minXp;
            if(xp < XP_FOR_LEVELS[level]) level--;
            return level;

        }


        /// <summary>
        /// This should be called after character creation or after certain age-ups
        /// </summary>
        /// <param name="attribute"></param> The AbilityScore, or similar, from which the bonus applies
        public void SetBonus(float attribute) {
            bonus = Mathf.Pow(1.58489319246f, (attribute / 5) - 2);
        }


        /// <summary>
        /// This updates the lastUsed variable, and is called when the skill is used or learning occurs;
        /// note that working a job that requires the skill does count as use, as we assume it will be used
        /// in some aspect on the job.
        /// </summary>
        public void BeUsed() {
            // FIXME??? Get from manager?
            lastUsed = WorldTime.Instance.Days;
        }


        /// <summary>
        /// To be called once per day (probably at midnight or 6:00 am / 06:00, mosty to handle skill
        /// decay from non-use.
        /// </summary>
        /// <returns>True if a skill level was lost</returns>
        public bool DailyUpdate() {
            int currentLevel = level;
            // FIXME??? Get from manager?
            if((lastUsed - 1.5) > WorldTime.GetWorldTime().Days) {
                RelativeDecay();
                return level < currentLevel;
            }
            return false;
        }


        public void SetLevel(int newLevel) {
            if(newLevel > level ) {
                level = newLevel;
                xp = XPForLevelED(level);
                float newMin = Mathf.Sqrt((float)xp);
                minXp = Mathf.Max(minXp, newMin);
            } else if (newLevel < level) {
                level = newLevel;
                xp = XPForLevelED(level);
                float newMin = Mathf.Sqrt((float)xp);
                minXp = Mathf.Min(Mathf.Max(minXp, newMin), (float)XP_FOR_LEVELS[level]);
            }
        }


        //TODO: Code for actually using the skill (success?  Speed?)





        #region Editor Helper
        public static int LevelForXPEd(double xp)
                => Mathf.Max(0, Mathf.FloorToInt(Mathf.Log((float)xp, 1.58489319246f) - 9));
        public static double XPForLevelED(int level) {
            if(level < 1) return 0.0;
            else return Mathf.Pow(1.58489319246f, level - 1) * 100;
        }
        #endregion

    }

}
