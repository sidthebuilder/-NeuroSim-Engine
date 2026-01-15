using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterModel {

    /// <summary>
    /// Categorizes activities to allow for broad preference rules (e.g., "Likes Socializing").
    /// </summary>
    public enum EActivityCategory {
        None = 0,
        Physical,       // Exercise, Manual Labor
        Intellectual,   // Reading, Research, Puzzles
        Social,         // Chatting, Partying
        Creative,       // Painting, Writing
        Restorative,    // Sleeping, Meditating
        Entertainment   // TV, Games
    }

    /// <summary>
    /// Caches and manages the desirability modifiers for different activity types.
    /// This acts as the "Personality Filter" for decision making.
    /// </summary>
    [Serializable]
    public class Preferences {
        
        // Dictionary is not serializable by Unity by default, so we rebuild it on Init
        private Dictionary<EActivityCategory, float> categoryModifiers = new Dictionary<EActivityCategory, float>();

        // Debug view for Inspector (optional, could be removed in production)
        [SerializeField] private List<PreferenceEntry> debugView = new List<PreferenceEntry>();

        [Serializable]
        private struct PreferenceEntry {
            public EActivityCategory category;
            public float modifier;
        }

        public void Init(Personality personality) {
            CalculatePreferences(personality);
            SyncDebugView();
        }

        /// <summary>
        /// Gets the personalized multiplier for a given activity category.
        /// 1.0f = Neutral, >1.0f = Likes, <1.0f = Dislikes.
        /// </summary>
        public float GetModifier(EActivityCategory category) {
            if (categoryModifiers.TryGetValue(category, out float mod)) {
                return mod;
            }
            return 1.0f;
        }

        private void CalculatePreferences(Personality p) {
            categoryModifiers.Clear();

            // Default baseline
            foreach (EActivityCategory cat in Enum.GetValues(typeof(EActivityCategory))) {
                categoryModifiers[cat] = 1.0f;
            }

            // --- Apply Trait Logic ---
            // Note: Trait values are typically 0-20. 10 is average.
            // (Value - 10) gives us a centered range (-10 to +10).

            // 1. EXTROVERSION -> Affects Social vs Solitary
            float extEffect = (p.Extroverted.Value - 10) * 0.05f; // +/- 0.5 range
            categoryModifiers[EActivityCategory.Social] += extEffect;
            categoryModifiers[EActivityCategory.Intellectual] -= (extEffect * 0.2f); // Slight inverse

            // 2. OPENNESS -> Affects Creative & Intellectual
            float openEffect = (p.Open.Value - 10) * 0.04f;
            categoryModifiers[EActivityCategory.Creative] += openEffect;
            categoryModifiers[EActivityCategory.Intellectual] += (openEffect * 0.5f);

            // 3. INDUSTRIOUSNESS -> Affects Physical (Work) vs Entertainment (Lazy)
            float indEffect = (p.Industrious.Value - 10) * 0.04f;
            categoryModifiers[EActivityCategory.Physical] += indEffect; // Assuming physical = work often
            categoryModifiers[EActivityCategory.Entertainment] -= (indEffect * 0.5f);

            // 4. EMOTIONAL -> Affects Restorative (High neuroticism needs more calm?)
            float emoEffect = (p.Emotional.Value - 10) * 0.03f;
            if (emoEffect > 0) {
                 // Neurotic characters might Value restorative acts more to calm down
                categoryModifiers[EActivityCategory.Restorative] += emoEffect;
            }
            
            // --- Clamp Values ---
            // Prevent multipliers from going negative or becoming absurdly high
            List<EActivityCategory> keys = new List<EActivityCategory>(categoryModifiers.Keys);
            foreach(var key in keys) {
                categoryModifiers[key] = Mathf.Clamp(categoryModifiers[key], 0.1f, 3.0f);
            }
        }

        private void SyncDebugView() {
            debugView.Clear();
            foreach (var kvp in categoryModifiers) {
                debugView.Add(new PreferenceEntry { category = kvp.Key, modifier = kvp.Value });
            }
        }
    }
}
