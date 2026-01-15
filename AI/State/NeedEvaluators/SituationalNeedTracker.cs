using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [CreateAssetMenu(menuName = "Character Engine/AI/Situational Need Evaluator", order = 1002, fileName = "SituationalEvaluator")]
    public class SituationalNeedTracker : AbstractNeedEvaluator {

        public override float GetDesirability(ActivityChoice choice, Need need, float situation) {
            SetDesirability(choice, need, situation);
            return choice.desirability;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetDesirability(ActivityChoice choice, Need need, float situation) {
            if(choice.activity.available || choice.activity.shareable) {
                choice.desirability = (choice.activity.satisfaction - situation) * need.GetDrive() * Need.TIME_SCALE;
            } else {
                choice.desirability = 0.0f;
            }
        }

    }

}