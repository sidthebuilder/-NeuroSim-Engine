using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [CreateAssetMenu(menuName = "Character Engine/AI/Vital Need Evaluator", order = 1003, fileName = "VitalEvaluator")]
    public class VitalNeedTracker : AbstractNeedEvaluator {

        public override float GetDesirability(ActivityChoice choice, Need need, float situation) {
            SetDesirability(choice, need, situation);
            return choice.desirability;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetDesirability(ActivityChoice choice, Need need, float situation) {
            if(choice.activity.available || choice.activity.shareable) {
                choice.desirability = (choice.activity.satisfaction - situation) * need.GetDrive() * Need.TIME_SCALE;
                choice.desirability *= choice.desirability;
            } else {
                choice.desirability = 0.0f;
            }
        }


        public float GetDrive(ActivityChoice choice, Need need) {
            return (Mathf.Max((need.DriveOrigin - need.Value), 0f)
                    / Mathf.Clamp(need.Value - 0.2f, 0.01f, 0.4f) * need.Importance);
        }

    }

}