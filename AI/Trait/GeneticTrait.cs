using UnityEngine;


namespace CharacterModel {

    [System.Serializable]
    public class GeneticTrait {
        const float REAL_MAX = 10.0f / 3.0f;
        const float FAKE_MAX = REAL_MAX;
        const float AVERAGE = REAL_MAX / 2.0f;

        [SerializeField] [Range(0, REAL_MAX)] float geneA = AVERAGE, geneB = AVERAGE, fake = AVERAGE;
        [SerializeField] float raw;
        [SerializeField] int value;

        public float Raw => raw;
        public int   Value => value;

        public float[] Data() => new float[]{geneA, geneB, fake}; 


        // FIXME: Move to a static utility class, since it may be used elsewhere.
        public static bool CoinToss() {
            return Random.Range(0, 2) > 0; // 50%/50%
        }


        /// <summary>
        /// This must be called anything the gene values (including fake) are created or changed.
        /// </summary>
        private void UpdateValue() {
            raw = geneA + geneB + fake;
            value = Mathf.RoundToInt(raw);
        }


        private GeneticTrait(float a, float b, float complexity) {
            geneA = a;
            geneB = b;
            fake  = complexity;
            UpdateValue();
        }


        private GeneticTrait() {
            geneA = AVERAGE;
            geneB = AVERAGE;
            fake  = AVERAGE;
            UpdateValue();
        }


        public static GeneticTrait GetRandom() {
            return new GeneticTrait(Random.Range(0f, REAL_MAX),
                                    Random.Range(0f, REAL_MAX),
                                    Random.Range(0f, FAKE_MAX));
        }


        public static GeneticTrait GetAverage() {
            return new GeneticTrait();
        }


        /// <summary>
        /// Use to create a Genetic repressentation from a single value, such
        /// as from player inpput.
        /// This will create one with equal genes so as to produce intuitive
        /// inheritence patterns.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static GeneticTrait FromValueFloat(float value) {
            float part = Mathf.Clamp(value / 3f, 0, REAL_MAX);
            return new GeneticTrait(part, part, part);
        }


        /// <summary>
        /// Use to create a Genetic repressentation from a single value, such
        /// as from player inpput.
        /// This will create one with equal genes so as to produce intuitive
        /// inheritence patterns.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static GeneticTrait FromValueInt(int value) {
            float part = Mathf.Clamp(((float)value) / 3f, 0, REAL_MAX);
            return new GeneticTrait(part, part, part);
        }


        public static GeneticTrait Mate(GeneticTrait moms, GeneticTrait dads, int mutationChance = 0) {
            GeneticTrait output = new GeneticTrait();
            if(CoinToss()) {
                output.geneA = moms.geneA;
                output.geneB = dads.geneB;
            } else {
                output.geneA = dads.geneA;
                output.geneB = moms.geneB;
            }
            if(CoinToss()) output.Crossover();
            if(Random.Range(0, 99) < mutationChance) output.Mutate();
            output.fake = Random.Range(0f, FAKE_MAX);
            output.UpdateValue();
            return output;
        }


        private void Crossover() {
            float tmp = geneA;
            geneA = geneB;
            geneB = tmp;
        }


        private void Mutate() {
            if(CoinToss()) {
                geneA = Random.Range(0f, REAL_MAX);
            } else {
                geneB = Random.Range(0f, REAL_MAX);
            }
        }





        #region Editor Helpers
        public static int ValueFromGenesEd(float a, float b, float f) => Mathf.RoundToInt(a + b + f);
        public static float GenesFromValueEd(int value) => ((float)value) / 3.0f;
        #endregion


    }

}
