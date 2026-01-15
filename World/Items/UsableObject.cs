using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterModel;


namespace CharacterEngine.Items {

    public class UsableObject : MonoBehaviour {

        [SerializeField] Transform actionPoint;
        [SerializeField] Transform userLocations;

        [SerializeField] NeedSatisfier needSatisfier;



        // Start is called before the first frame update
        void Start() {

        }
    }

}