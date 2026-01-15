using UnityEngine;


namespace CharacterModel {

    [System.Serializable]
    public class EmotionalState {
        public const float TIME_TO_LOOSE_ONE = 24;
        public const float TIME_FACTOR = 1 / TIME_TO_LOOSE_ONE;

        [SerializeField] Emotion emotion = new Emotion();


#region Wrappers
        public float Positivity => emotion.Positivity;
        public float Avoidance  => emotion.Avoidance;
        public float Strength   => emotion.Strength;
        public float Joy        => emotion.Joy;

        public Color GetColor(float emoWellbeing) => emotion.GetColor(emoWellbeing);
        public Emotion.EmotionPacket RetrieveData(float emoWellbeing) => emotion.RetrieveData(emoWellbeing);
        public float GetEmotionAngle() => emotion.GetEmotionAngle();
        public float MagnitudeSq() => emotion.MagnitudeSq();
        public float Magnitude => emotion.Strength;
        public float Dot(Emotion a, Emotion b) => emotion.Dot(a, b);
        public void SetEmotion(float positivity, float avoidance) => emotion.Set(positivity, avoidance);
        public void BoundCircular() => emotion.BoundCircular();
        public void BoundSimple() => emotion.BoundSimple();
#endregion


        public void AddEmotion(EmotionEffect effect) {
            emotion += effect.Effect;
        }


        public void AddEmotion(EmotionObject effect) {
            emotion += effect.Effect;
        }


        public void EmoUpdate(float deltaTime) {
            //TODO?: Allow centers other than 0 to allow for cheerful and melancholy traits
            emotion -= emotion.GetNormalized() * (TIME_FACTOR * deltaTime / Need.TIME_SCALE);
        }


    }

}
