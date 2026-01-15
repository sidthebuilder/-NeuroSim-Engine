using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterModel {

    [System.Serializable]
    public class Relationships {
        [SerializeField] List<Relationship> relationships;


        public Relationship AddRelationship(Character character) {
            Relationship rel = new Relationship(character);
            relationships.Add(rel);
            return rel;
        }


        public Relationship? Find(ulong id) {
            foreach(Relationship rel in relationships) {
                if(rel.otherID == id) return rel;
            }
            return null;
        }


        public void Remove(ulong id) {
            int i = 0;
            for(; i < relationships.Count; i++) {
                if(relationships[i].otherID == id) break;
            }
            relationships.RemoveAt(i);
        }


    }

}