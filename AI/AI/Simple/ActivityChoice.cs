using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterModel {


    [System.Serializable]
    public class ActivityChoice : IComparer<ActivityChoice>, System.IComparable<ActivityChoice> {
        [SerializeField] public Activity activity;
        //[SerializeField] public AbstractNeedEvaluator evaluator;
        [SerializeField] public EActivityCategory category; // New category field
        public float desirability = 0; // This is to be calculated during decision making, not preset as data
        [SerializeField] AbstractNeedEvaluator  evaluator;


        // Getting and setting desirability
        public float GetDesirability(CoreNeeds needs, float situation, Preferences prefs = null) {
             float baseScore = evaluator.GetDesirability(this, needs.GetNeed(activity.need), situation);
             if (prefs != null) {
                 baseScore *= prefs.GetModifier(category);
             }
             return baseScore;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetDesirability(CoreNeeds needs, float situation, Preferences prefs = null) {
            float baseScore = evaluator.GetDesirability(this, needs.GetNeed(activity.need), situation);
             if (prefs != null) {
                 baseScore *= prefs.GetModifier(category);
             }
            desirability = baseScore;
        }


        // Comparisons -- desirability is the basis, since the more desirable choices willl be preferred
        // Compare() and CompareTo() are set up so that sort functions will place higher desirability on top
        public static bool operator >(ActivityChoice a, ActivityChoice b) => a.desirability > b.desirability;
        public static bool operator <(ActivityChoice a, ActivityChoice b) => a.desirability < b.desirability;
        public static bool operator >=(ActivityChoice a, ActivityChoice b) => a.desirability >= b.desirability;
        public static bool operator <=(ActivityChoice a, ActivityChoice b) => a.desirability <= b.desirability;
        public int Compare(ActivityChoice a, ActivityChoice b) {
            return -a.desirability.CompareTo(b.desirability); // Should I do this, or use arithmetic?
        }
        public int CompareTo(ActivityChoice other) {
            return -desirability.CompareTo(other.desirability); // Should I do this, or use arithmetic?
        }


            ActivityChoice output = new ActivityChoice();
            output.activity = activity;
            output.desirability = desirability;
            output.evaluator = evaluator;
            output.category = category;
            return output;
        }


    }


}