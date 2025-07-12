using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterModel {


    [System.Serializable]
    public class ActivityHolder : MonoBehaviour {

        [SerializeField] ActivityChoice[] activities;

        public ActivityChoice[] Activities => activities;
        public ActivityChoice GetActivityChoice(int n) => activities[n];
        public Activity GetActivity(int n) => activities[n].activity;


        public void AddActivities(ref List<ActivityChoice> mainList) {
            foreach(ActivityChoice activity in Activities) {
                mainList.Add(activity);
            }
        }


        // TODO: It seems I need more, but not sure what yet

    }



}