using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using kfutils.UI;
using System;
using CharacterEngine;


namespace CharacterModel {

    [Serializable]
    public class EmotionalEffects {

        [Serializable]
        public class Effect {
            public readonly float positivity, avoidance;
            public readonly double expiration;
            public Effect(float positivity, float avoidance, double expiration) {
                this.positivity = positivity;
                this.avoidance = avoidance;
                this.expiration = expiration;
            }
        }


        private List<Effect> effects  = new List<Effect>();
        private List<Effect> toRemove = new List<Effect>();


        public void AddEffect(float positivity, float avoidance, double duration) {
            Effect effect = new Effect(positivity, avoidance, WorldTime.Instance.GameTime + duration);
            effects.Add(effect);
        }


        public void AddEffect(Emotion emotion, double duration) {
            Effect effect = new Effect(emotion.Positivity, emotion.Avoidance, WorldTime.Instance.GameTime + duration);
            effects.Add(effect);
        }


        public Emotion Update() {
            float positivity = 0.0f;
            float avoidance  = 0.0f;
            foreach(Effect effect in effects) {
                if(WorldTime.Instance.GameTime > effect.expiration) {
                    toRemove.Add(effect);
                    positivity += effect.positivity;
                    avoidance  += effect.avoidance;
                }
            }
            foreach(Effect effect in effects) {
                effects.Remove(effect);
            }
            effects.Clear();
            return new Emotion(positivity, avoidance);
        }
    }
}
