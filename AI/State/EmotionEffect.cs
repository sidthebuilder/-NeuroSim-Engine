using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using kfutils.UI;
using System;
using CharacterEngine;



namespace CharacterModel {

    [System.Serializable]
    public class EmotionEffect {
        [SerializeField] Emotion effect;
        [SerializeField] double duration;

        public Emotion Effect => effect;
        public double Duration => duration;

        public EmotionEffect(Emotion effect, double duration) {
            this.effect = effect;
            this.duration = duration;
        }


        public EmotionEffect(float positivity, float avoidance, double duration) {
            effect = new Emotion(positivity, avoidance);
            this.duration = duration;
        }


        public EmotionEffect(EmotionObject emotionObject) {
            effect = emotionObject.Effect;
            duration = emotionObject.Duration;
        }
    }

}
