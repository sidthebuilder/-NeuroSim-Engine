using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using kfutils.UI;
using CharacterEngine;

namespace CharacterModel {

    [System.Serializable]
    public class Character : MonoBehaviour {
        // The idCounter starts at 0 in a new empty world, and should be equal to the number of characters who have
        // ever existed in a save file (world here is the entire universe / timeline, i.e., as would be represented
        // by a given save file (or newly created setting).
        private static ulong idCounter = 0;

        [SerializeField] string name;
        [SerializeField] ulong id; // Must be unique (but UUID would be overkill)

        [SerializeField] float age;

        [SerializeField] Personality personality;
        [SerializeField] CoreNeeds needs;
        [SerializeField] EmotionalState emotions;
        [SerializeField] Preferences preferences;
        [SerializeField] Relationships relationships;
        [SerializeField] MemorySystem memory; // New System

        [SerializeField] ActivityChooser ai;
        [SerializeField] CharacterMotor motor;

        public static ulong IDCount => idCounter;

        public Personality Persona => personality;
        public CoreNeeds Needs => needs;
        public EmotionalState Emotions => emotions;
        public Preferences prefs => preferences;
        public CharacterMotor Motor => motor;
        public MemorySystem Memory => memory; // Public Accessor

        public ActivityChooser AI => ai; // FIXME? TODO? Should it really be part of a bigger AI module?

        public ulong ID => id;


        /// <summary>
        /// Set the ID of a new character; this should be done when character is created and should never change
        /// under any circumstance.  This number will them be used to uniquely track that character throughout
        /// the game and between play sessions (i.e., both during run time and in save files).
        /// </summary>
        /// <returns></returns>
        private ulong SetID() {
            int nameHash = name.GetHashCode();
            if(nameHash < 0) nameHash = Mathf.Abs(nameHash) | 0x1 << 31;
            int rnadomPart = Random.Range(0, 65536);
            id = ((ulong)nameHash) |  (idCounter << 32) | (((ulong)rnadomPart) << 47);
            idCounter++;
            return id;
        }


        void Awake() {
            needs.Init(this);
            preferences.Init(personality); 
            if (memory == null) memory = new MemorySystem(); // Init memory
            
            if(motor == null) motor = GetComponent<CharacterMotor>();
            if(motor == null) motor = gameObject.AddComponent<CharacterMotor>();
        }

        void Update() {
            // Update systems that need time
            if (memory != null) memory.UpdateMemories(Time.deltaTime);
        }


        public override int GetHashCode() => (int)(id ^ (id >> 32));
        public override bool Equals(object other) {
            Character o = other as Character;
            return (o != null) && (o.id == id);
        }
        public static bool operator ==(Character a, Character b) => a.id == b.id;
        public static bool operator !=(Character a, Character b) => a.id != b.id;

    }


}
