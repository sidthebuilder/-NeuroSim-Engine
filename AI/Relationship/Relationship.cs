using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterModel {

    [System.Serializable]
    public struct Relationship {
        public ulong otherID;
        //??? should these be discrte integers (-100 to 100) of floating point (-1.0 to 1.0)?
        [Range(-100, 100)]
        public sbyte social;
        [Range(-100, 100)]
        public sbyte romantic;


        public static bool operator ==(Relationship a, Relationship b) => a.otherID == b.otherID;
        public static bool operator ==(Character other, Relationship b) => other.ID == b.otherID;
        public static bool operator ==(Relationship b, Character other) => other.ID == b.otherID;
        public static bool operator ==(Relationship other, ulong b) => other.otherID == b;
        public static bool operator ==(ulong b, Relationship other) => other.otherID == b;

        public static bool operator !=(Relationship a, Relationship b) => a.otherID != b.otherID;
        public static bool operator !=(Character a, Relationship b) => a.ID != b.otherID;
        public static bool operator !=(Relationship b, Character a) => a.ID != b.otherID;
        public static bool operator !=(Relationship a, ulong b) => a.otherID != b;
        public static bool operator !=(ulong b, Relationship a) => a.otherID != b;


        public Relationship(Character character) {
            otherID = character.ID;
            social = 0;
            romantic = 0;
        }

    }

}
