using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CharacterEngine;


namespace CharacterModel {

    /*
    How to actually create and change emotions (instead of just represent them), current idea:

    The characters emotions should have both a current emotion and one that is tracked to (similar to how
    the Situational need tracks a target).

    When in emotion inspiring event occurs, both the real and the tracked emotion are updated, and the a representation
    of the emotion, including its dimensions and expiration time (in world time), is added to a list.

    The list is iterated (either every frame or on a custom AI tick which could be staggered) and any whose expiration
    has past will be remove from the list and their dimensions subtracted from the tracked emotion (but NOT the real
    emotion).

    During the same emotion update (whether per frame / engine update or less frequent custom tick) the emotion moves
    closer to the target -- preferable in a mix a relative and constant components.  This way emotions fade slowly,
    rather than ping-ponging around as effects are added and removed.
    */

    /// <summary>
    /// Based loosely on Plutnicks color wheel of emotions, but with some liberties to have
    /// it work better and make more sense ase a game.  Notably, fear and surprize are swaped
    /// and the positive axis goes through "love."
    /// </summary>
    [Serializable]
    public struct Emotion {

        public struct EmotionPacket {
            public readonly Color color;
            public readonly EEmotionType type;
            public readonly string name;
            public EmotionPacket(Color color, EEmotionType type, string name) {
                this.color = color;
                this.type = type;
                this.name = name;
            }
        }


        // Some directional units
        const float SQRT2   = 0.707106781187f;
        const float COS225  = 0.923879532511f;
        const float SIN225  = 0.382683432365f;
        const float TWOPI   = 3.14159265359f * 2.0f;
        const float BOUND   = 3.0f;
        const float NEEDFAC = 0.5f / BOUND;

        public static Emotion operator+(Emotion a, Emotion b)
                => new Emotion(a.positivity + b.positivity, a.avoidance + b.avoidance);
        public static Emotion operator-(Emotion a, Emotion b)
                => new Emotion(a.positivity - b.positivity, a.avoidance - b.avoidance);
        public static Emotion operator*(Emotion a, Emotion b)
                => new Emotion(a.positivity * b.positivity, a.avoidance * b.avoidance);
        public static Emotion operator*(Emotion a, float b)
                => new Emotion(a.positivity * b, a.avoidance * b);
        public static Emotion operator/(Emotion a, Emotion b)
                => new Emotion(a.positivity / b.positivity, a.avoidance / b.avoidance);
        public static Emotion operator/(Emotion a, float b)
                => new Emotion(a.positivity / b, a.avoidance / b);


        //Data
        [SerializeField] float positivity, avoidance;

        public float Positivity => positivity;
        public float Avoidance  => avoidance;
        public float Strength   => Mathf.Sqrt((positivity * positivity) + (avoidance * avoidance));
        public float Joy        => (Positivity * NEEDFAC) + 0.5f;


        public Color GetColor(float emoWellbeing) {
            return GetColorStatic(Mathf.Min(positivity, (emoWellbeing * 4.0f) - 1.0f), avoidance);
        }


        public Color GetColorDirect(float realPositivity) {
            return GetColorStatic(realPositivity, avoidance);
        }


        public static Color GetColorStatic(float positivity, float avoidance) {
            float angle = Mathf.Atan2(avoidance, -positivity)  / TWOPI;
            angle += 0.25f;
            angle = angle - Mathf.Floor(angle);
            float hue = 1.0f - angle;
            float strength = Mathf.Sqrt((positivity * positivity) + (avoidance * avoidance));
            float saturation = Mathf.Clamp(strength, 0.0f, 1.0f);
            float value = Mathf.Clamp(((strength * 0.25f) + 0.5f), 0.5f, 1.0f);
            return Color.HSVToRGB(hue, saturation, value);
        }


        public EmotionPacket RetrieveData(float emoWellbeing) {
            float realPositivity = Mathf.Min(positivity, positivity * emoWellbeing);
            float strength = Magnitude(realPositivity);
            EEmotionType type = EmotionType.GetTypeOfEmotion(realPositivity, avoidance);
            if(strength > 0.5f) {
                return new EmotionPacket(GetColorDirect(realPositivity),
                                         type, EmotionNames.GetPreciseName(type, Mathf.RoundToInt(strength)));
            } else {
                if((positivity - realPositivity) > 0.5) return new EmotionPacket(GetColorDirect(realPositivity),
                        type, EmotionNames.neutrals[1]);
                else return new EmotionPacket(GetColorDirect(realPositivity), type, EmotionNames.neutrals[0]);
            }
        }


        // Emotional Axes
        public static readonly Emotion UPOSITIVE   = new Emotion( 1, 0);
        public static readonly Emotion UAVOIDANCE  = new Emotion( 0, 1);

        // Unit feeling vectors -- primary
        public static readonly Emotion UHAPPY  = new Emotion( COS225, -SIN225);
        public static readonly Emotion UTRUST  = new Emotion( COS225,  SIN225);
        public static readonly Emotion UAMAZE  = new Emotion( SIN225,  COS225);
        public static readonly Emotion UFEAR   = new Emotion(-SIN225,  COS225);
        public static readonly Emotion USAD    = new Emotion(-COS225,  SIN225);
        public static readonly Emotion UGROSS  = new Emotion(-COS225, -SIN225);
        public static readonly Emotion URAGE   = new Emotion(-SIN225, -COS225);
        public static readonly Emotion UFOCUS  = new Emotion( SIN225, -COS225);

        // Unit feeling vectors -- secondary
        public static readonly Emotion ULOVE   = new Emotion( 1, 0);
        public static readonly Emotion USUB    = new Emotion( SQRT2,  SQRT2);
        public static readonly Emotion UAWE    = new Emotion( 0,  1);
        public static readonly Emotion UNOLIKE = new Emotion(-SQRT2, SQRT2);
        public static readonly Emotion UBAD    = new Emotion(-1,  0);
        public static readonly Emotion UHATE   = new Emotion(-SQRT2, -SQRT2);
        public static readonly Emotion UAGRRO  = new Emotion(0,  -1);
        public static readonly Emotion UHOPE   = new Emotion( SQRT2,  -SQRT2);

#region testing
        public static (float, float) GetTestValues(EEmotionType which) {
            switch (which) {
                case EEmotionType.SURPRISED:
                    return (UAMAZE.positivity, UAMAZE.avoidance);
                case EEmotionType.CONNECTED:
                    return (UTRUST.positivity, UTRUST.avoidance);
                case EEmotionType.HAPPY:
                    return (UHAPPY .positivity, UHAPPY .avoidance);
                case EEmotionType.INTERESTED:
                    return (UFOCUS.positivity, UFOCUS.avoidance);
                case EEmotionType.ANGER:
                    return (URAGE.positivity, URAGE.avoidance);
                case EEmotionType.DISGUST:
                    return (UGROSS.positivity, UGROSS.avoidance);
                case EEmotionType.SADNESS:
                    return (USAD.positivity, USAD.avoidance);
                case EEmotionType.FEAR:
                    return (UFEAR.positivity, UFEAR.avoidance);
                default: return (0, 0);
            }
        }
#endregion

        public Emotion(float positivity, float avoidance) {
            this.positivity = positivity;
            this.avoidance = avoidance;
        }


        public float GetEmotionAngle() {
            float angle = Mathf.Atan2(positivity, avoidance)  / TWOPI;
            angle = angle - Mathf.Floor(angle);
            return angle;
        }


        // For testing, must mirror non-static version exactly
        public static float GetEmotionAngle(float positivity, float avoidance) {
            float angle = Mathf.Atan2(positivity, avoidance)  / TWOPI;
            angle = angle - Mathf.Floor(angle);
            return angle;
        }


       //TODO: Set up a way to get the needs, either through direct access or a containing "Character" class if this is to be used for autonomy
        public float MagnitudeSq() {
            //float realPositivity = Mathf.Min(positivity, positivity * needs.Mental);
            //return (realPositivity * realPositivity) + (avoidance * avoidance);
            return (positivity * positivity) + (avoidance * avoidance);
       }


        public float MagnitudeSq(float realPositivity) {
            return (realPositivity * realPositivity) + (avoidance * avoidance);
        }


        //TODO: Set up a way to get the needs, either through direct access or a containing "Character" class if this is to be used for autonomy
        public float Magnitude() {
            //float realPositivity = Mathf.Min(positivity, positivity * needs.Mental);
            //return Mathf.Sqrt(realPositivity * realPositivity) + (avoidance * avoidance);
            return Mathf.Sqrt(positivity * positivity) + (avoidance * avoidance);
        }


        public float Magnitude(float realPositivity) {
            return Mathf.Sqrt(realPositivity * realPositivity) + (avoidance * avoidance);
        }


        public float Dot(Emotion a, Emotion b) {
            return ((a.positivity * b.positivity) + (a.avoidance * b.avoidance));
        }


        public Emotion GetNormalized() {
            float str = Strength;
            if(Strength > 0) {
                return new Emotion(positivity / str, avoidance / str);
            } else {
                return new Emotion(0, 0);
            }
        }


        /// <summary>
        /// Directly set the emotional dimensions, for the purpose of saving and loading.
        /// </summary>
        /// <param name="positivity"></param>
        /// <param name="avoidance"></param>
        public void Set(float positivity, float avoidance) {
            this.positivity = positivity;
            this.avoidance = avoidance;
        }


        // TODO: Profile to see if this is to see if it is too computationally expensive to be practical if called frequently at scale
        // Preferred way to do it...?
        public void BoundCircular() {
            float size = Strength;
            float factor = Mathf.Min(size, BOUND) / size;
            positivity *= factor;
            avoidance *= factor;
        }


        // TODO: Profile to see if this is to see if it is significantly more efficient than BoundCircular()
        public void BoundSimple() {
            positivity = Mathf.Clamp(positivity, -BOUND, BOUND);
            avoidance  = Mathf.Clamp(avoidance, -BOUND, BOUND);
        }


        public void TrackTarget(Emotion target) {
            Emotion dif = target - this;
            Emotion norm = dif.GetNormalized();
            positivity += ((dif.positivity  / (float)WorldTime.PER_4HOUR) * WorldTime.Instance.DeltaTime)
                        + ((norm.positivity / (float)WorldTime.PER_DAY)   * WorldTime.Instance.DeltaTime);
            avoidance  += ((dif.avoidance   / (float)WorldTime.PER_4HOUR) * WorldTime.Instance.DeltaTime)
                        + ((norm.avoidance  / (float)WorldTime.PER_DAY)   * WorldTime.Instance.DeltaTime);
        }

    }

}
