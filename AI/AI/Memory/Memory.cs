using System;
using UnityEngine;

namespace CharacterModel {

    [Serializable]
    public struct Memory {
        public double timeStamp;       // When it happened (World Time)
        public EActivityCategory topic; // What it was about (e.g., "Social")
        public float emotionalImpact;  // +/- Value. Positive = Good memory.
        public ulong specificID;       // Optional: ID of person/object involved (0 if general)
        public float strength;         // 0.0 to 1.0. Fades over time.

        public Memory(double time, EActivityCategory topic, float impact, ulong specificID = 0) {
            this.timeStamp = time;
            this.topic = topic;
            this.emotionalImpact = impact;
            this.specificID = specificID;
            this.strength = 1.0f; // Fresh memory
        }

        public float GetCurrentInfluence() {
            return emotionalImpact * strength;
        }

        /// <summary>
        /// Fades the memory. Returns true if the memory is forgotten (strength <= 0).
        /// </summary>
        public bool Decay(float amount) {
            // Stronger emotions fade slower
            float resistance = Mathf.Abs(emotionalImpact) * 0.5f; 
            float effectiveDecay = amount / (1.0f + resistance);
            
            strength -= effectiveDecay;
            return strength <= 0.01f;
        }
    }
}
