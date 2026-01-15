using System;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterModel {

    [Serializable]
    public class NeedSatisfier {
        [SerializeField] List<NeedEffect> effects;
        [Tooltip ("How long it takes to gain the full effect in world time.")]
        [SerializeField] float timeToUse;
        [SerializeField] EPreferences actionType;

        public List<NeedEffect> Effects => effects;
        public float  TimeToUse => timeToUse;

    }
}
