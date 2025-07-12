using System;


namespace CharacterModel {

    // Here should be a list of desirability factors for all the activity item types, relatively narrowly defined and
    // keyed to EPreferences (below); activities largely corresponding to usable item types, but also including things
    // like socialization.  This is based on personality (traits of all types, any kind of like / dislike / favorites
    // system added, and updated if any of these change.  This should be a far simpler and more efficient system
    // than using every influence every time, on every decision.  Essentially, a cached version of checking the
    // effects of all influences.  (The question is array keyed to list using or dictionary; EPreference should be
    // contiguous either way.)
    [Serializable]
    public class Preferences {


    }


    public enum EPreferences {
        // Here should be a list of all the activity item types, relatively narrowly defined; activities largely
        // corresponding to usable item types, but also including things like socialization.
    }

}
