using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static CharacterModel.ENeeds;
using CharacterEngine;


namespace CharacterModel {


    [Serializable]
    public class UsableItem /*: ScriptableObject*/ {
        [SerializeField] EPreferences actionType;
        [SerializeField] float desirability;
        [SerializeField] NeedEffect[] needEffects;
        [SerializeField] float situationEffect;
        [SerializeField] readonly ISpecialEffect specialEffect;
        
        public EPreferences type => actionType;
        public float Desirability => desirability;
        public NeedEffect[] NeedEffects => needEffects;
        public float Situational => situationEffect;
        public ISpecialEffect Special => specialEffect;


        /// <summary>
        /// This calculates the actual desirability to a given character based on the base desirability, the characters
        /// personality (vie preferences), and the characters current needs (and probably emotions as well).
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public float GetDesirability(Character character) {
            // TODO
            return desirability; // FIXME: A stand-in until other systems are complete
        }


        public void UseItem(Character character) {
            // TODO
        }

    }

}
