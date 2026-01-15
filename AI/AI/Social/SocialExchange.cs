using UnityEngine;

namespace CharacterModel {

    public struct SocialResult {
        public bool Success;
        public float ImpactA; // Emotional change for A
        public float ImpactB; // Emotional change for B
    }

    public static class SocialExchange {
        
        /// <summary>
        /// Simulates a social interaction between two characters.
        /// </summary>
        public static SocialResult Interact(Character a, Character b) {
            
            // 1. Calculate Base Compatibility (0.0 to 1.0)
            float compatibility = a.Persona.Compatibility(b.Persona);
            
            // 2. Add Situation/Mood factors (e.g., if A is Angry, chance drops)
            float moodFactorA = a.Emotions.Positivity; // -1 to 1
            float moodFactorB = b.Emotions.Positivity;
            
            // Final Success Chance (-1.0 to 2.0 range roughly)
            float successChance = compatibility + (moodFactorA * 0.2f) + (moodFactorB * 0.2f);
            
            // 3. Roll Dice
            bool success = Random.value < successChance;

            // 4. Determine Outcome
            SocialResult res = new SocialResult();
            res.Success = success;
            
            if (success) {
                // Good interaction: Boost Social Score + Joy
                res.ImpactA = 0.2f + (compatibility * 0.5f);
                res.ImpactB = 0.2f + (compatibility * 0.5f);
            } else {
                // Bad interaction: Hurt Social Score + Anger/Sadness
                res.ImpactA = -0.3f;
                res.ImpactB = -0.3f; // Negative impact is usually stronger than positive
            }

            return res;
        }
    }
}
