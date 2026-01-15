using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [System.Serializable]
    public class PersonalityDevelopment {
        // Constant representing base XP for skill levels
        // Values may change with development and testing
        public const double ZERO    = -100 * 6.3095734448;
        public const double ONE     = -100 * 3.98107170554;
        public const double TWO     = -100 * 2.51188643151;
        public const double THREE   = -100 * 1.58489319246;
        public const double FOUR    = -10  * 7.9244659623;
        public const double FIVE    = 0;
        public const double SIX     = 10   * 7.9244659623;
        public const double SEVEN   = 100  * 1.58489319246;
        public const double EIGHT   = 100  * 2.51188643151;
        public const double NINE    = 100  * 3.98107170554;
        public const double TEN     = 100  * 6.3095734448;
        public static readonly double[] XP_FOR_LEVELS
                = { ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN };
        // The maximum value achievable;
        public const double MIN     = -10000;
        public const double MAX     =  10000;


        // DATA
        [SerializeField][Range(-1000,1000)] double xp = 0;
        [SerializeField][Range(0,10)] public int level = 5;

        public double XP => xp;
        public int Level => level;


        /// <summary>
        /// Handles increases in skill XP for the skill, and raises the skill if needed.
        /// </summary>
        /// <param name="amount">How much the skill has increased (skill XP)</param>
        /// <returns>Any general character XP gain from leveling the skill;
        /// 0 if the skill level did not change</returns>
        public int Change(float amount) {
            int output = 0;
            xp += amount;
            // FIXME: Is there a better way to do this, give the numbers available?
            if(xp > 0) {
                if(xp < SIX) level = 5;
                else if(xp < SEVEN) level = 6;
                else if(xp < EIGHT) level = 7;
                else if(xp < NINE) level = 8;
                else if(xp < TEN) level = 9;
                else level = 10;
            } else {
                if(xp > FOUR) level = 5;
                else if(xp > THREE) level = 4;
                else if(xp > TWO) level = 3;
                else if(xp > ONE) level = 2;
                else if(xp > ZERO) level = 1;
                else level = 0;
            }
            System.Math.Clamp(xp, MIN, MAX);
            return output;
        }


        public void SetLevel(int newLevel) {
            level = newLevel;
            xp = XP_FOR_LEVELS[level];
        }


        //TODO: Code for actually using the skill (success?  Speed?)





#region Editor Helper
        public static int LevelForXPEd(double xp) {
            int level;
            if(xp > 0) {
                if(xp < SIX) level = 5;
                else if(xp < SEVEN) level = 6;
                else if(xp < EIGHT) level = 7;
                else if(xp < NINE) level = 8;
                else if(xp < TEN) level = 9;
                else level = 10;
            } else {
                if(xp > FOUR) level = 5;
                else if(xp > THREE) level = 4;
                else if(xp > TWO) level = 3;
                else if(xp > ONE) level = 2;
                else if(xp > ZERO) level = 1;
                else level = 0;
            }
            return level;
        }
        public static double XPForLevelED(int level) {
            if(level < 1) return 0.0;
            return XP_FOR_LEVELS[level];
        }
#endregion

    }

}
