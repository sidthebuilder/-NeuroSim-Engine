using System.Collections.Generic;
using UnityEngine;

namespace CharacterModel {

    /// <summary>
    /// The "Brain" of the character.
    /// Evaluates available choices based on Needs + Preferences and instructs the Motor to act.
    /// </summary>
    public class ActivityChooser : MonoBehaviour
    {
        [SerializeField] List<ActivityChoice> choices = new List<ActivityChoice>();
        [SerializeField] Character character;

        private float activityTimer = 0;
        private bool  executing     = false;

        // Testing/Debug
        [SerializeField] Activity currentChoice;
        
        // Cache refs
        private CharacterMotor motor;

        void Start()
        {
            if (character == null) character = GetComponent<Character>();
            motor = character.Motor; // Use the decoupled motor
        }

        void Update()
        {
            // Safety check
            if (character == null || motor == null) return;

            if(!executing) {
                // DECISION PHASE: Choose a new activity
                if(activityTimer <= 0) {
                    currentChoice = Choose();
                    
                    // Setup new task
                    activityTimer = currentChoice.timeToDo * Need.TIME_SCALE;
                    
                    // Apply immediate situational effects (simplified)
                    if(currentChoice.need == ENeeds.SITUATIONAL) 
                        character.Needs.Situation = currentChoice.satisfaction;
                    else 
                        character.Needs.Situation = 0.2f;

                    // Command Motor
                    motor.MoveTo(currentChoice.actorLocation.position);
                    executing = true;
                }
            } else {
                // EXECUTION PHASE: Monitor progress
                if (motor.HasReachedDestination()) {
                    // We are at the location, do the activity
                    EnsureRotation(); // Optional polish

                    activityTimer -= Time.deltaTime;
                    
                    // Apply benefit over time
                    character.Needs.GetNeed(currentChoice.need).AddSafe(
                        (currentChoice.satisfaction / currentChoice.timeToDo) * Time.deltaTime
                    );

                    // Task Complete?
                    if (activityTimer <= 0) {
                        executing = false; // Ready to choose again
                    }

                } 
                // Else: Still walking...
            }

            // Global Updates
            character.Emotions.EmoUpdate(Time.deltaTime);
            character.Needs.UpdateNeedsTesting();
            
            // Death Check (Should probably be in a Health system, but keeping here for legacy consistancy)
            if(character.Needs.GetNeed(ENeeds.HEALTH).Value == 0) {
                Debug.Log(character.name + " Died");
                Destroy(gameObject);
            }
        }

        private void EnsureRotation() {
             if (currentChoice.actorLocation != null) {
                transform.rotation = Quaternion.Slerp(transform.rotation, currentChoice.actorLocation.rotation, Time.deltaTime * 5f);
             }
        }

        public void SortChoices() {
            // Recalculate all scores using Needs, Personality Preferences, and MEMORY
            foreach(ActivityChoice choice in choices) {
                // Base Utility + Preference
                float baseScore = choice.GetDesirability(character.Needs, character.Needs.Situation, character.prefs);
                
                // Memory Bias (+/- 50% max influence)
                // We add the memory recall value directly to the score
                float bias = character.Memory.Recall(choice.category);
                
                // Final Score
                choice.desirability = baseScore + bias; 
            }
            choices.Sort(); // Sorts by Desirability (Highest first)
        }

        public Activity Choose() {
            if (choices.Count == 0) return null;

            SortChoices();

            // Utility Theory Selection (Weighted Random)
            // Instead of pure random, we favor the top choices heavily.
            
            // 1. Sum total desirability of top X choices
            int numToConsider = Mathf.Clamp(choices.Count / 3, 3, 10);
            numToConsider = Mathf.Min(numToConsider, choices.Count);

            float totalScore = 0;
            for(int i = 0; i < numToConsider; i++) {
                // Ensure non-negative for roulette wheel
                totalScore += Mathf.Max(choices[i].desirability, 0.01f); 
            }

            // 2. Spin the wheel
            float randomPoint = Random.Range(0, totalScore);
            float currentSum = 0;

            for(int i = 0; i < numToConsider; i++) {
                currentSum += Mathf.Max(choices[i].desirability, 0.01f);
                if (currentSum >= randomPoint) {
                    return choices[i].activity;
                }
            }

            // Fallback (should rarely happen)
            return choices[0].activity;
        }

        public void AssignChoices(List<ActivityChoice> availableChoices) {
            choices.Clear();
            foreach(ActivityChoice choice in availableChoices) {
                 choices.Add(choice.Duplicate());
            }
        }
    }
}
