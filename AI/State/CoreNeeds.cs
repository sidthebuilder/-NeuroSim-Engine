using UnityEngine;
using static CharacterModel.ENeeds;
using CharacterEngine;


namespace CharacterModel {


/*****************************************************************************************************
//•   Energy: (Physical) Decreases somewhat slowly, and is restored by rest (sleep); exertion may
//    drain it faster. Running out causes passing out.
//
//•   Hunger: (Physical) Decreases at a moderate pace and is restored by eating. Running out causes
//    death.
//
//•   Bowels: (Physical) Decreases at a moderate speed, decreased some by eating, restored by
//    bathrooms – because bladder has already been used, but its basically the same need with a
//    different name. Running out causes an embarrassing loose of bowel control.
//
//•   Health: (Physical) Based on several thing; slowly decreased when other physical needs are low,
//    when diet is not balanced, or when sick or injured. Improved slowly by keeping other physical
//    needs high and when recovering from illness. Running out causes death.
//
//•   Social: (Psychological) Decreases over time at moderate speed, increased be socialization; rates
//    in each direction are effected by Extroversion and by some extroverted Minor Traits. When low
//    an emotion in the pure negative direction is produced (thus it can effect the next need, they are
//    not completely orthogonal.)
//
//•   Emotional: (Psychological) Based on positivity component of the current emotional state vector.
//    When very low it become hard to continue or focus on tasks, so they may be abandoned
//    prematurely.
//
//•   Situational: (Psychological) A mixture of current environment and comfort; these are added to
//    create a target that the need itself tracks (but does not jump to equaling).
//
//•   Aspirational: (Psychological) Decrease very slowly over time, and is increased by doing thing
//    that the character would enjoy doing or find meaningful based on Minor Traits, Dreams, and
//    Interests.
********************************************************************************************************/

    [System.Serializable]
    public class CoreNeeds {

        public struct NeedsPacket {
            public readonly float energy, nourishment, excretion, health, social, emotional, situational, aspirational,
                                  physWellbeing, psychWellbeing, wellbeing ;
            public NeedsPacket(float energy, float nourishment, float excretion, float health,
                               float social, float emotional, float situational, float aspirational,
                               float physWellbeing, float psychWellbeing, float wellbeing) {
                this.energy = energy;
                this.nourishment = nourishment;
                this.excretion = excretion;
                this.health = health;
                this.social = social;
                this.emotional = emotional;
                this.situational = situational;
                this.aspirational = aspirational; // "Fulfilment" in UI
                this.physWellbeing = physWellbeing;
                this.psychWellbeing = psychWellbeing;
                this.wellbeing = wellbeing;
            }
        }

        // Physical Needs
        [SerializeField] Need energy; // = new Need(0.03f, 1.2f);
        [SerializeField] Need nourishment; // = new Need(0.015f, 1.5f);
        [SerializeField] Need excretion; // = new Need(0.125f, 1.25f);
        [SerializeField] Need health; // = new Need(0.0f, 0.75f, 0.0f, false);
        // Psychological Needs
        [SerializeField] Need social; // = new Need(0.027f, 1.0f);
        [SerializeField] Need emotional; // = new Need(0.0f, 1.0f, 0.0f, false);
        [SerializeField] Need situational; // = new Need(0.0f, 1.0f, 0.0f, false);
        [SerializeField] Need aspirational; // = new Need(0.0030154821598f, 0.5f); //NOTE: Call this "Fulfillment in game!

        [SerializeField] float physicalWellbeing = 1.0f;
        [SerializeField] float mentalWellbeing = 1.0f;
        [SerializeField] float totalWellbeing = 1.0f;

        public Character character;

        public float Physical => physicalWellbeing;
        public float Mental => mentalWellbeing;
        public float Wellbeing => totalWellbeing;

        private Need[] allNeeds;

        [SerializeField] float situation = 0.0f; // the current target for the situational need to track

        public float Situation {
            get => situation;
            set {situation = value; }
        }


        public void Init(Character owner) {
            character = owner;
            allNeeds = new Need[]{energy, nourishment, excretion, health, social, emotional, situational, aspirational};
        }


        public Need GetNeed(ENeeds need) => allNeeds[(int)need];


        public void UpdateNeeds() {
            energy.Decay();
            nourishment.Decay();
            excretion.Decay();
            social.Decay();
            aspirational.Decay();

            emotional.Set(character.Emotions.Joy);
            situational.ApplySituationChange(situation);

            CalculateMentalWellbeing();
            UpdateHealth();
            CalculatePhysicalbeing();
            CalculateTotalWellbeing();
        }


        public void UpdateNeedsTesting() {
            energy.Decay();
            nourishment.Decay();
            excretion.Decay();
            social.Decay();
            aspirational.Decay();

            emotional.Set(character.Emotions.Joy);
            situational.TrackTargetValue(situation);

            CalculateMentalWellbeing();
            UpdateHealth();
            CalculatePhysicalbeing();
            CalculateTotalWellbeing();
        }


        // For testing purposes only, allowing for the calculations without an actual game or complete character
        public void UpdateWellbeingForTesting() {
            CalculateMentalWellbeing();
            CalculatePhysicalbeing();
            CalculateTotalWellbeing();
        }


        private void UpdateHealth() {
            float decay = ((nourishment.GetLowness() + (energy.GetLowness() * 0.2f))
                            + (((Mathf.Max(0.35f - mentalWellbeing, 0.0f)) * 0.2f)) * 0.5f);
            if(decay > 0) health.SituationalDecay(decay);
            else health.SituationalIncrease(Mathf.Max(nourishment.GetGoodness(), energy.GetGoodness()));
        }


        private void CalculatePhysicalbeing() {
            physicalWellbeing = Mathf.Clamp(
                                ((energy.Value * energy.GetDrive()) + (nourishment.Value * nourishment.GetDrive())
                                        + (excretion.Value * excretion.GetDrive()) + (health.Value * health.GetDrive()))
                                    / (energy.GetDrive() + nourishment.GetDrive()
                                        + excretion.GetDrive() + health.GetDrive()),
                                0, 1.0f);
        }


        private void CalculateMentalWellbeing() {
            mentalWellbeing = Mathf.Clamp(
                                ((social.Value * social.GetDrive()) + (emotional.Value * emotional.GetDrive())
                                        + (situational.Value * situational.GetDrive())
                                        + (aspirational.Value * aspirational.GetDrive()))
                                    / (social.GetDrive() + emotional.GetDrive()
                                        + situational.GetDrive() + aspirational.GetDrive()),
                                0, 1.0f);
        }


        private void CalculateTotalWellbeing() {
            float physicalDrive = (1.2f - physicalWellbeing) / Mathf.Clamp(physicalWellbeing, 0.01f, 0.5f);
            float mentalDrive = ((1.2f - mentalWellbeing) / Mathf.Clamp(mentalWellbeing, 0.01f, 0.5f));
            totalWellbeing = Mathf.Clamp(
                                ((physicalWellbeing * physicalDrive) + (mentalWellbeing * mentalDrive))
                                    / (physicalDrive + mentalDrive),
                                0.0f, 1.0f);
        }


        public NeedsPacket RetrieveData() {
            return new NeedsPacket(energy.Value, nourishment.Value, excretion.Value, health.Value,
                                   social.Value, emotional.Value, situational.Value, aspirational.Value,
                                   physicalWellbeing, mentalWellbeing, totalWellbeing);
        }


        public void AlterNeedGradual(NeedEffect effect, float timeForEffect) {
            Need need = allNeeds[(int)effect.Need];
            need.Add((effect.Effect / timeForEffect) * WorldTime.t.DeltaTime);
        }


        public void AlterNeedInstant(NeedEffect effect) {
            Need need = allNeeds[(int)effect.Need];
            need.Add(effect.Effect);
        }


        public void AddSituationalEffect(float effect) {
            situation += effect;
        }


        public void RemoveSituationalEffect(float effect) {
            situation -= effect;
        }


        /// <summary>
        /// This is the one that will probably be used, collecting the total effects whenever situation changes;
        /// for example, changing rooms, start/stopping using a comforting or entertaining item, etc.
        /// </summary>
        /// <param name="effects"></param>
        public void SetSituation(float[] effects) {
            situation = 0.0f;
            foreach(float effect in effects) situation += effect;
        }


    }



}
