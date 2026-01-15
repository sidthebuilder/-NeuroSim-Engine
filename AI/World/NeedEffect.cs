using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static CharacterModel.ENeeds;
using CharacterEngine;


namespace CharacterModel {

    [Serializable]
    public class NeedEffect {
        [SerializeField] ENeeds need;
        [Tooltip ("How much it increases the need, should usually be small, well under 1.0f")]
        [SerializeField] [Range (0f, 1f)] readonly float effect;

        [SerializeField] float value;
        [SerializeField] float desireFactor = 1.0f;

        public ENeeds Need => need;
        public float Effect => effect;

        public float Value => value;
        public float DesireFactor => desireFactor = 1.0f;
        public float BaseDesire => value * desireFactor;

    }
}
