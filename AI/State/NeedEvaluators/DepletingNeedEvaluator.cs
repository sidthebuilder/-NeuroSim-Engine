using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [CreateAssetMenu(menuName = "Character Engine/AI/Depleting Need Evaluator", order = 1001, fileName = "DepletingEvaluator")]
    public class DepletingNeedEvaluator : AbstractNeedEvaluator {

        public override float GetDesirability(ActivityChoice choice, Need need, float unused = 0f) {
            SetDesirability(choice, need);
            return choice.desirability;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetDesirability(ActivityChoice choice, Need need, float unused = 0f) {
            if(choice.activity.available || choice.activity.shareable) {
                choice.desirability = (choice.activity.satisfaction
                    + (choice.activity.satisfaction / (choice.activity.timeToDo + 5)))
                    * need.GetDrive() * Need.TIME_SCALE;
            } else {
                choice.desirability = 0.0f;
            }
        }

    }

}
