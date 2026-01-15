using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using kfutils.UI;
using CharacterEngine;

namespace CharacterModel {

    [System.Serializable]
    public class Activity {

        // TODO: Need for multi-need satisfying activities to be represented (I think...?)

        public ENeeds need;
        public float satisfaction;
        public float timeToDo;
        public Transform actorLocation;

        public bool available =  true;
        public bool shareable = false;

    }


}