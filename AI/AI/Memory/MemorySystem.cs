using System.Collections.Generic;
using UnityEngine;

namespace CharacterModel {

    [System.Serializable]
    public class MemorySystem {

        [SerializeField]
        private List<Memory> longTermMemory = new List<Memory>();

        public void AddMemory(EActivityCategory topic, float impact, ulong specificID = 0) {
            // TODO: Connect to WorldTime. Currently using Time.time as placeholder
            double now = Time.time; 
            longTermMemory.Add(new Memory(now, topic, impact, specificID));
            
            // Limit capacity to prevent memory leaks in long sims
            if (longTermMemory.Count > 100) {
                // Remove weakest memory (simple optimization)
                longTermMemory.RemoveAt(0);
            }
        }

        /// <summary>
        /// What do I feel about this topic?
        /// Returns a modifier (e.g., 0.1 means "I have +10% good memories about this").
        /// </summary>
        public float Recall(EActivityCategory topic, ulong specificID = 0) {
            float totalBias = 0;
            
            foreach(var mem in longTermMemory) {
                bool relevant = false;
                
                // Match Topic
                if (mem.topic == topic) relevant = true;
                
                // If specific ID is requested (e.g. specific person), prioritize that
                if (specificID != 0 && mem.specificID == specificID) {
                    relevant = true;
                    // Boost relevance if it's EXACTLY this person
                    if (mem.topic == topic) totalBias += mem.GetCurrentInfluence() * 2.0f; // Double weight for specific matches
                } 
                else if (specificID == 0 && mem.topic == topic) {
                    // General recall
                    totalBias += mem.GetCurrentInfluence();
                }
            }

            // Clamp resonance to avoid insane biases
            return Mathf.Clamp(totalBias, -0.5f, 0.5f);
        }

        public void UpdateMemories(float deltaTime) {
            // Decay rate: 1.0f represents "Full Forget" in X seconds.
            // Let's say we forget everything in 3 game-days (approx).
            // Tuned low for stability.
            float decayRate = deltaTime * 0.001f; 

            for (int i = longTermMemory.Count - 1; i >= 0; i--) {
                Memory mem = longTermMemory[i];
                if (mem.Decay(decayRate)) {
                    longTermMemory.RemoveAt(i);
                } else {
                    longTermMemory[i] = mem; // Update struct in list
                }
            }
        }
    }
}
