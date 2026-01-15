using UnityEngine;


namespace CharacterModel {

    /// <summary>
    /// Represents a core, basic ability type.  Will probably be called
    /// "talents" in game.
    /// </summary>
    [System.Serializable]
    public struct Talent {
        [SerializeField] GeneticTrait inborn;
        [SerializeField] Skill learnedSkill;

        public GeneticTrait Inborn => Inborn;
        public Skill Learned => learnedSkill;
        public int Value => inborn.Value + Learned.Level;

        public static Talent FromValueInt(int value) {
            int genetics = Mathf.Max(Mathf.Min(value - 5, 10), 0);
            int learned = 5;
            if(value < 5) learned = value;
            if(value > 15) learned = value - 10;
            Talent output = new Talent();
            output.inborn = GeneticTrait.FromValueInt(genetics);
            output.learnedSkill.SetLevel(learned);
            return output;
        }

        public static Talent GenerateRandomly(bool randomizeExperience = true) {
            Talent output = new Talent();
            output.inborn = GeneticTrait.GetRandom();
            if(randomizeExperience) {
                // Produce a value ranging from 2 to 8 with moderate central tendency
                output.learnedSkill.SetLevel(Random.Range(1, 5) +
                Random.Range(1, 5));
            } else {
                // Assign the most average value
                output.Learned.SetLevel(5);
            }
            return output;
        }
    }



#region Helpers
    /// <summary>
    /// A list of talent types.
    /// </summary>
    public enum Talents {
        PHYSICAL,
        INTELLECTUAL,
        PRACTICAL,
        CREATIVE,
        SOCIAL
    }


    public sealed class TalentTexts {
        public const string PHYSICAL_TXT = "Those who are physically talented are agile and athletic.  They " +
                                           "learn skill based on strength, stamina or agility more quickly, " +
                                           "and are also generally healthier.  They are sick less often, " +
                                           "recover from poor health more quickly, and are generally healthier. " +
                                           "They may even live a bit longer than others.";

        public const string INTELLECTUAL_TXT = "Those who are intellectually talented learn skill that involve " +
                                               "reasoning, problem solving, or learning a lot of information " +
                                               "more quickly and are likely to better at jobs that use such skills.";

        public const string PRACTICAL_TXT = "Those with a talent for the practical are good at down-to-earth taks, " +
                                            "not to mention organizing people and things.  They make good office " +
                                            "workers, managers, and business people, as well as being good around the " +
                                            "house.";

        public const string CREATIVE_TXT = "Those who are creatively talented are good at coming up with new ideas " +
                                           "and producing oringal and creative works.  They tend to be good at " +
                                           "the arts and at related jobs.";

        public const string SOCIAL_TXT = "Those who are socially talented are good at dealing with people and often " +
                                         "get along well with others.  They learn social skills more quickly.";

        public static readonly string[] TALENT_TEXTS
                = {PHYSICAL_TXT, INTELLECTUAL_TXT, PRACTICAL_TXT, CREATIVE_TXT, SOCIAL_TXT};

        public static string GetText(Talents talent) => TALENT_TEXTS[(int)talent];
        public static string GetText(int index) => TALENT_TEXTS[index];

    }


    public struct TalentIntPacket {
        public readonly int physical, intellectual, practical, creative, social;
        public TalentIntPacket(int physical, int intellectual, int practical, int creative, int social) {
            this.physical = physical;
            this.intellectual = intellectual;
            this.practical = practical;
            this.creative = creative;
            this.social = social;
        }
    }
#endregion

}
