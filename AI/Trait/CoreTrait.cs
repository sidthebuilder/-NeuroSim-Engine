using UnityEngine;


namespace CharacterModel {


    /// <summary>
    /// An individual core personality trait.
    /// Very similar to Ability Score, but is used to represent something very different.
    /// </summary>
    [System.Serializable]
    public struct CoreTrait {
        [SerializeField] GeneticTrait inborn;
        [SerializeField] PersonalityDevelopment experiential;

        public GeneticTrait Inborn => Inborn;
        public PersonalityDevelopment Learned => experiential;
        public int Value => inborn.Value + Learned.Level;
        // FIXME?  Not sure if I will keep this; to show real value or relative to average, that is the question
        public int DisplayedValue => inborn.Value + Learned.Level - 10;

        public static CoreTrait FromValueInt(int value) {
            int genetics = Mathf.Max(Mathf.Min(value - 5, 10), 0);
            int learned = 5;
            if(value < 5) learned = value;
            if(value > 15) learned = value - 10;
            CoreTrait output = new CoreTrait();
            output.inborn = GeneticTrait.FromValueInt(genetics);
            output.experiential.SetLevel(learned);
            return output;
        }

        public static CoreTrait GenerateRandomly(bool randomizeExperience = true) {
            CoreTrait output = new CoreTrait();
            output.inborn = GeneticTrait.GetRandom();
            if(randomizeExperience) {
                // Produce a value ranging from 2 to 8 with moderate central tendency
                output.experiential.SetLevel(Random.Range(1, 5) +
                                             Random.Range(1, 5));
            } else {
                // Assign the most average value
                output.experiential.SetLevel(5);
            }
            return output;
        }
    }



    #region Helpers
    /// <summary>
    /// The core (dimensional) traits, based loosely on the Five Factor model (NEO-PI, OCEAN).
    /// I'm not the first to, apparently, base a personality system loosel on that theory, so I need
    /// to take different liberties.
    /// </summary>
    public enum CoreTraits {
        OPEN = 0,
        MORAL = 1,
        EXTROVERT = 2,
        SENSITIVE = 3,
        EMOTIONAL = 4,
        INDUSTRIOUS = 5
    }


    /// <summary>
    /// A container class for strings holding descriptions of the core traits.
    /// </summary>
    public sealed class CoreTraitText {
        public const string OPEN_TXT = "How open this person is to new or unusual experiences.  " +
                                       "Those who are high tend enjoy learning, exploration, and " +
                                       "creative persuits, and are often non-conformists.  Those " +
                                       "who score low are likely to be traditional and focus on " +
                                       "practical concerns.";

        public const string MORAL_TXT = "Those who score high in this have a sense of honor, and prefer to " +
                                        "do what seems right and avoid dishonesty.  Those who score low are more " +
                                        "likely to lie, cheat, or do whatever it takes to get what they want.";

        public const string EXTROVERT_TXT = "Those whoe score high in this are classic extroverts -- " +
                                            "friendly, outgoing, active, and cheerful.  Those who score low " +
                                            "are introverts who can be satisfied with less social interaction.";

        public const string SENSITIVE_TXT = "Those who score high are soft heart, kind, and generally easy to " +
                                            "to get along with.  Those who score low can seem cold or unfriendly, " +
                                            "but have a thick skin and experience fewer negative emotions from " +
                                            "negative social interactions.";

        public const string EMOTIONAL_TXT = "This is vulnerability to negative emotions, and those who score high " +
                                            "will experience these more often and more intensely.  On the upside, " +
                                            "they tend to be more responsible and have better survival instinct " +
                                            "- these kind of people who pay their bill as soon as they get them, for " +
                                            "fear of the consequences of not doing so.  Those who score low tend to " +
                                            "be easy-going and carefree, if a bit irresponsiible and wreckless.";

        public const string INDUSTRIOUS_TXT = "Those who score high are motivated, organized, and like to get the " +
                                              "job done right. They have tendencies to work-a-holism and " +
                                              "perfectionism. Those who score lower have a more relaxed attitude " +
                                              "toward work and goals, and tend to cut corners when they can.";

        public static readonly string[] CORE_TRAIT_TEXTS
                = {OPEN_TXT, MORAL_TXT, EXTROVERT_TXT, SENSITIVE_TXT, EMOTIONAL_TXT, INDUSTRIOUS_TXT};

        public static string GetText(CoreTraits trait) => CORE_TRAIT_TEXTS[(int)trait];
        public static string GetText(int index) => CORE_TRAIT_TEXTS[index];

    }


    public struct CoreTraitIntPacket {
        public readonly float open, moral, extrovert, sensitive, emotional, industrious;
        public CoreTraitIntPacket(int open, int moral, int extrovert, int sensitive, int emotional, int industrious) {
            this.open = open;
            this.moral = moral;
            this.extrovert = extrovert;
            this.sensitive = sensitive;
            this.emotional = emotional;
            this.industrious = industrious;
        }
    }


    #endregion

}
