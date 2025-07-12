using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [Serializable]
    public enum ENeeds {
        ENERGY = 0,
        FOOD = 1,
        POTTY = 2,
        HEALTH = 3,
        SOCIAL = 4,
        EMOTIONAL = 5,
        SITUATIONAL = 6,
        ASPIRATION = 7
    }


    [Serializable]
    public enum ENeedType {
        SIMPLE,
        TARGETED,
        CALCULATED
    }

}
