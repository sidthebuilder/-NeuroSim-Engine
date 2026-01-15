using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    public class EmotionType {
        public const float NUM2EMO = 8.0f; //1.0f / 0.785398163398f;
        public static readonly EEmotionType[] emotions = new EEmotionType[] {
            EEmotionType.SURPRISED, EEmotionType.CONNECTED, EEmotionType.HAPPY,   EEmotionType.INTERESTED,
            EEmotionType.ANGER,     EEmotionType.DISGUST,   EEmotionType.SADNESS, EEmotionType.FEAR };


        public static EEmotionType GetTypeOfEmotion(Emotion emotion) {
            //Debug.Log(emotion.GetEmotionAngle() + " -> " + ((int)(emotion.GetEmotionAngle() * NUM2EMO)));
            return emotions[(int)(emotion.GetEmotionAngle() * NUM2EMO)];
        }


        // For testing, must mirror non-static version exactly
        public static EEmotionType GetTypeOfEmotion(float positivity, float avoidance) {
            //Debug.Log(emotion.GetEmotionAngle() + " -> " + ((int)(emotion.GetEmotionAngle() * NUM2EMO)));
            return emotions[(int)(Emotion.GetEmotionAngle(positivity, avoidance) * NUM2EMO)];
        }


    }


    public enum EEmotionType {
        SURPRISED,
        CONNECTED,
        HAPPY,
        INTERESTED,
        ANGER,
        DISGUST,
        SADNESS,
        FEAR
    }


    public static class EmotionNames {
        private static readonly string[][] names = new string[][]{
            new string[]{"Amazement", "Surprised", "Amazed", "Awe-Struck" },
            new string[]{"Connection", "Accepted", "Connected", "Loved" },
            new string[]{"Happiness", "Happy", "Joyful", "Ecstatic" },
            new string[]{"Interest", "Interested", "Entusiastic", "Fascinated" },
            new string[]{"Anger", "Annoyed", "Angry", "Enraged" },
            new string[]{"Disgust", "Put Off", "Disguested", "Appalled" },
            new string[]{"Sadness", "Sad", "Sorrowful", "Dispairing" },
            new string[]{"Fear", "Anxious", "Afraid", "Terrified" }
        };

        public static readonly string[] neutrals = new string[] {"Indifferent", "Mixed"};

        /// <summary>
        /// Returns the names of the type / category of emotion.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public static string GetName(EEmotionType emotion) => names[(int)emotion][0];

        /// <summary>
        /// Returns the name of the specific emotional state, including both type a strength (degree).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static string GetPreciseName(EEmotionType type, int degree) {
            return names[(int)type][Mathf.Clamp(degree, 1, 3)];
        }
    }


}
