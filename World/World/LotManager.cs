using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterModel {


    [System.Serializable]
    public class LotManager : MonoBehaviour {
        [SerializeField] List<ActivityHolder> usableObjects;
        [SerializeField] List<Character> characters;

        private List<ActivityChoice> choices;

        // Start is called before the first frame update
        void Start() {
            choices = new List<ActivityChoice>();
            foreach(ActivityHolder usableObject in usableObjects) {
                usableObject.AddActivities(ref choices);
            }
            foreach(Character character in characters) {
                character.AI.AssignChoices(choices);
            }
        }

        /*// Update is called once per frame
        void Update() {

        }*/
    }

}