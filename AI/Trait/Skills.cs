using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterEngine;


namespace CharacterModel {

    [System.Serializable]
    public class Skills {
        [SerializeField] Skill[] coreSkills = new Skill[17];





    }



    #region SkillEnums
    public enum ECoreSkills {
            //Physical
            Athletics = 0,
            Dancing = 1,
            Driving = 2,
            MartialArts = 3,

            //Creative
            Art = 4,
            Music = 5,
            Writing = 6,

            //Intellectual
            Science = 7,
            Mechanical = 8,
            Computers = 9,
            Gaming = 10,

            //Social
            Charm = 11,
            Performance = 12,
            Persuasion = 13,

            //Practical
            Business = 14,
            Cooking = 15,
            Housekeeping = 16,
            Naturalist = 17
    }


    public enum EChildSkills {
        Physical = 0,
        Creative = 1,
        Intellectual = 2,
        Social = 3,
        Practical = 4
    }


    public enum EHiddenSkills {
        Physical = 0,
        Creative = 1,
        Intellectual = 2,
        Social = 3,
        Practical = 4
    }
    #endregion


}