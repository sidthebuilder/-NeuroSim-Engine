using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {


    [Serializable]
    public class Need {
        public const float TIME_SCALE = 6f; // FIXME: Connect to universal time scale
        [SerializeField] /*[HideInInspector]*/ [Range(0, 1)] float value;
        [SerializeField] float decayRate = 0.25f;
        [SerializeField] float importance = 1.0f;
        [SerializeField][HideInInspector] float minValue = 0.0f;
        [SerializeField] float driveOrigin = 1.2f;
        [SerializeField] bool depleting = false;

        public float Value => value;
        public float DriveOrigin => driveOrigin;
        public float Importance => importance;


        public Need(float decayRate, float importance, float minimum = 0.0f, bool tracking = true) {
            value = 1.0f;
            this.decayRate = decayRate;
            this.importance = importance;
            minValue = minimum;
            this.depleting = tracking;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Bound() {
            value = Mathf.Clamp(value, minValue, 1.0f);
        }



        /// <summary>
        /// For simple needs that just decay over time; may be used as part of some less complex needs
        /// </summary>
        public void Decay() {
            value -= (decayRate * Time.deltaTime) / TIME_SCALE;
            //value -= (decayRate * WorldTime.Instance.DeltaTime) / TIME_SCALE;
            Bound();
        }


        /// <summary>
        /// For needs that decay under certain circumstances, such health of certain other needs are low
        /// </summary>
        /// <param name="rate"></param>
        public void SituationalDecay(float rate) {
            value -= (rate * Time.deltaTime) / TIME_SCALE;
            //value -= (rate * WorldTime.Instance.DeltaTime) / TIME_SCALE;
            Bound();
        }


        /// <summary>
        /// The reverse of SituationalDecay(), mostly to make it extra clear what it represents.
        /// </summary>
        /// <param name="rate"></param>
        public void SituationalIncrease(float rate) {
            value += (rate * Time.deltaTime) / TIME_SCALE;
            //value += (rate * WorldTime.Instance.DeltaTime) / TIME_SCALE;
            Bound();
        }


        /// <summary>
        /// For applying gradual changes caused by external influences, typically from other needs.
        /// This is notably used for the Health need, which decreases when other physiologic needs are low
        /// (or imbalanced?) and increases when those needs are in a good ranges.  Also likely to be used
        /// for needs that recharge slowly during activities like sleeping or eating.
        /// </summary>
        public void ApplySituationChange(float amount) {
            value += (amount * Time.deltaTime) / TIME_SCALE;
            //value += (amount * WorldTime.Instance.DeltaTime) / TIME_SCALE;
            Bound();
        }


        /// <summary>
        /// For needs that track a current value from elsewhere;
        /// mostly for the Situational need (environment + comfort + entertainment)
        /// </summary>
        public void TrackTargetValue(float target) {
            float amount = ((target - value) * Time.deltaTime) / TIME_SCALE;
            //float amount = ((target - value) * WorldTime.Instance.DeltaTime) / TIME_SCALE;
            value += (amount * 0.25f) + Mathf.Max(amount, 0);
            Bound();
        }


        /// <summary>
        /// Add a discrete amount to the need, as might happen if an action or in-game event changes the need by a
        /// certain amount all at once.  This is intended for both increases and decreases; for decreases, just add
        /// the number.
        /// A likely common use (with a negative value) would be applying the effects of an injury to health
        /// </summary>
        public void AddSafe(float amount) {
            if(depleting) {
                value += amount;
                Bound();
            }
        }


        /// <summary>
        /// Add a discrete amount to the need, as might happen if an action or in-game event changes the need by a
        /// certain amount all at once.  This is intended for both increases and decreases; for decreases, just add
        /// the number.
        /// A likely common use (with a negative value) would be applying the effects of an injury to health
        /// </summary>
        public void Add(float amount) {
            value += amount;
            Bound();
        }


        /// <summary>
        /// For rare potential circumstances when a need should be set directly.
        /// This should also be used for loading saves.
        /// </summary>
        public void Set(float amount) {
            value = Mathf.Clamp(amount,  0.0f, 1.0f);
        }


        public void SetMinValue(float minimum) {
            minValue = minimum;
        }


        /// <summary>
        /// Convenience method to instantly fill; probably only used for with cheats.
        /// </summary>
        public void Fill() {
            value = 1.0f;
        }


        /// <summary>
        /// Convenience method to instantly zero-out; With some needs this will lead to instant death.
        /// </summary>
        public void Empty() {
            value = 1.0f;
        }


        /// <summary>
        /// Get the strength of the current motivation to fulfill this need
        /// This may need to be tweaked through testing, though the basic concept should work.
        /// Basically the lower the need the higher the drive, with very low need becoming
        /// increasingly intense, especially when entering the danger zone for complete depletion.
        /// </summary>
        public float GetDrive() {
            return (Mathf.Max((driveOrigin - value), 0f) / Mathf.Clamp(value, 0.05f, 0.5f) * importance);
        }


        public static float GetDrive(float value) {
            return ((1.2f - value) / Mathf.Clamp(value, 0.05f, 0.5f));
        }


        public bool IsLow() => value < 0.35f;

        public float GetLowness() => Mathf.Max(0.35f - value, 0.0f);

        public float GetGoodness() => Mathf.Max(value - 0.65f, 0.0f);


    }

}
