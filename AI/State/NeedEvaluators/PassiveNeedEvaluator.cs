using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [CreateAssetMenu(menuName = "Character Engine/AI/Passive Need Evaluator", order = 1004, fileName = "Passive" +
    "Evaluator")]
    public class PassiveNeedEvaluator : AbstractNeedEvaluator {

        public override float GetDesirability(ActivityChoice choice, Need need, float unused = 0f) {
            SetDesirability(choice, need);
            return 0.0f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetDesirability(ActivityChoice choice, Need need, float unused = 0f) {
            choice.desirability = 0.0f;
        }

    }

}
