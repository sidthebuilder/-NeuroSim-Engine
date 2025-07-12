using System.Runtime.CompilerServices;
using UnityEngine;


namespace CharacterModel {

    public abstract class AbstractNeedEvaluator : ScriptableObject {

        public abstract float GetDesirability(ActivityChoice choice, Need need, float extraData = 0f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void SetDesirability(ActivityChoice choice, Need need, float extraData = 0f);
    }

}