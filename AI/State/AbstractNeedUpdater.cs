using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    public abstract class AbstractNeedUpdater : ScriptableObject {
        public abstract void UpdateNeed(Need need);
    }

}