using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterModel {


    public class ActivityChooser : MonoBehaviour
    {
        [SerializeField] List<ActivityChoice> choices = new List<ActivityChoice>();
        [SerializeField] Character character;

        private float activityTimer = 0;
        private bool  atLoction     = false;
        private bool  ready         = false;

        //Testing Stuff
        [SerializeField] Activity currentChoice;
        [SerializeField] NavMeshAgent navAgent;


        // Start is called before the first frame update
        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        // FIXME/TODO: This ultimately needs to be removed, but first it must be replaced with a call from outside
        void Update()
        {
            if(!ready) return;
            if(activityTimer <= 0) {
                currentChoice.available = true;
                currentChoice = Choose();
                //testingPlaceShower.transform.rotation = currentChoice.actorLocation.rotation;
                activityTimer = currentChoice.timeToDo * Need.TIME_SCALE;
                if(currentChoice.need == ENeeds.SITUATIONAL) character.Needs.Situation = currentChoice.satisfaction;
                else character.Needs.Situation = 0.2f;
                navAgent.SetDestination(currentChoice.actorLocation.position);
                atLoction = false;
                currentChoice.available = false;
            } else {
                //FIXME: Remember, in the real game anything similar must use worled (simulation) time, not engine game time!
                if(atLoction) {
                    transform.rotation = currentChoice.actorLocation.rotation;
                    activityTimer -= Time.deltaTime;
                    character.Needs.GetNeed(currentChoice.need).AddSafe((currentChoice.satisfaction / currentChoice.timeToDo)
                    * Time.deltaTime);
                } else {
                    atLoction = navAgent.remainingDistance < 0.1f;
                }
            }
            character.Emotions.EmoUpdate(Time.deltaTime);
            character.Needs.UpdateNeedsTesting();
            if(character.Needs.GetNeed(ENeeds.HEALTH).Value == 0) {
                GameObject.Destroy(gameObject);
                Debug.Log(character.name + " Died");
            }
        }


        public void SortChoices() {
            foreach(ActivityChoice choice in choices) {
                choice.SetDesirability(character.Needs, character.Needs.Situation);

            }
            choices.Sort();
        }


        public Activity Choose() {
            SortChoices();
            int numToConsider = choices.Count;
            if(choices.Count > 3) {
                numToConsider = Mathf.Min(Mathf.Max((choices.Count / 5), 2), 6);
            }
            float selector = 0;
            for(int i = 0; i < numToConsider; i++) {
                selector += choices[i].desirability;
            }
            selector = Random.Range(0, selector);
            int selection = 0;
            while(selector > choices[selection].desirability) {
                selector -= choices[selection].desirability;
                selection++;
                #if UNITY_EDITOR
                //Testing Failsage
                if(selection >= numToConsider) {
                    selection = 0;
                    selector = 0.0f;
                    Debug.LogError("ActivityChooser.Choose(): Selector overran range; something is wrong!");
                    break;
                }
                #endif
            }
            return choices[selection].activity;
            return choices[selection].activity;
        }


        public void AssignChoices(List<ActivityChoice> availableChoices) {
            choices.Clear();
            foreach(ActivityChoice choice in availableChoices) choices.Add(choice.Duplicate());
            ready = true;
        }



    }

}
