using UnityEngine;
using CharacterEngine;


namespace CharacterModel.UI {


    public class UICollator {
        Personality personality;
        EmotionalState emotion;
        CoreNeeds needs;


        // FIXME / TODO: Pass in the relevant UI controller to receive data
        public void GetData() {
            CoreNeeds.NeedsPacket needData = needs.RetrieveData();
            Emotion.EmotionPacket emotionData = emotion.RetrieveData(needData.psychWellbeing);
        }


        public static Color GetNeedColor(float need) {
            return Color.HSVToRGB(Mathf.Clamp(need * need, 0.0f, 0.5f), 1.0f, (need * 0.5f) + 0.5f);
        }


    }


}
