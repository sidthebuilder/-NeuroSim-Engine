using UnityEngine;
using System.Reflection;
using CharacterModel;

public class LogicValidator : MonoBehaviour {

    void Start() {
        RunPreferenceTests();
    }

    void RunPreferenceTests() {
        Debug.Log("--- STARTING LOGIC VALIDATION ---");

        // 1. Test Extroversion Logic
        Personality p = new Personality();
        
        // Use Reflection to set private CoreTrait 'extroverted' to High Value (20)
        SetTraitValue(p, "extroverted", 20); // Max Extroversion
        SetTraitValue(p, "open", 10);        // Average Open
        SetTraitValue(p, "industrious", 10); // Average Ind

        Preferences prefs = new Preferences();
        prefs.Init(p);

        float socialMod = prefs.GetModifier(EActivityCategory.Social);
        // (20 - 10) * 0.05 = +0.5. Base is 1.0. Expected = 1.5.
        
        if (Mathf.Approximately(socialMod, 1.5f)) {
            Debug.Log("<color=green>PASS: Extroversion correctly boosts Social activities (1.5x).</color>");
        } else {
            Debug.LogError($"FAIL: Extroversion check failed. Expected 1.5, got {socialMod}");
        }

        // 2. Test Introversion Logic
        SetTraitValue(p, "extroverted", 0); // Logic: (0 - 10) * 0.05 = -0.5. Base = 0.5.
        prefs.Init(p);
        
        socialMod = prefs.GetModifier(EActivityCategory.Social);
        if (Mathf.Approximately(socialMod, 0.5f)) {
             Debug.Log("<color=green>PASS: Introversion correctly reduces Social activities (0.5x).</color>");
        } else {
            Debug.LogError($"FAIL: Introversion check failed. Expected 0.5, got {socialMod}");
        }
        
        // 3. Test Memory Logic
        MemorySystem memSystem = new MemorySystem();
        memSystem.AddMemory(EActivityCategory.Physical, 1.0f); // Positive memory of Exercise

        // Recall should be positive (approx 1.0, decayed slightly by update if called, but here immediate)
        float recall = memSystem.Recall(EActivityCategory.Physical);
        
        if (recall > 0.0f) {
             Debug.Log($"<color=green>PASS: Memory correctly boosts Physical activity preference (Bias: {recall}).</color>");
        } else {
            Debug.LogError($"FAIL: Memory Recall failed. Expected > 0, got {recall}");
        }

        Debug.Log("--- VALIDATION COMPLETE ---");
    }

    // Helper to set private nested fields for testing
    void SetTraitValue(Personality p, string fieldName, int desiredValue) {
        
        // 1. Get the CoreTrait field from Personality
        FieldInfo traitField = typeof(Personality).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (traitField == null) { Debug.LogError($"Field {fieldName} not found"); return; }
        
        object traitObj = traitField.GetValue(p); // Boxed CoreTrait

        // 2. We need to set the 'inborn' GeneticTrait's value. 
        // CoreTrait has '[SerializeField] GeneticTrait inborn;'
        FieldInfo inbornField = typeof(CoreTrait).GetField("inborn", BindingFlags.NonPublic | BindingFlags.Instance);
        object geneticObj = inbornField.GetValue(traitObj);

        // 3. GeneticTrait has 'value' and 'raw'.
        FieldInfo valField = typeof(GeneticTrait).GetField("value", BindingFlags.NonPublic | BindingFlags.Instance);
        valField.SetValue(geneticObj, desiredValue);

        // 4. Write back (because Structs are value types, we must re-set the Modified object)
        inbornField.SetValue(traitObj, geneticObj);
        traitField.SetValue(p, traitObj);
    }
}
