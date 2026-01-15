using UnityEngine;
using UnityEditor;
using CharacterModel;

namespace CharacterModel.EditorTools {

    public class NeuroDebugger : EditorWindow {
        
        [MenuItem("NeuroSim/Neural Debugger")]
        public static void ShowWindow() {
            GetWindow<NeuroDebugger>("Neural Debugger");
        }

        private Character selectedChar;
        private Vector2 scrollPos;

        void OnGUI() {
            EditorGUILayout.LabelField("🧠 NeuroSim Neural Debugger", EditorStyles.boldLabel);
            
            // Auto-select if a GameObject with Character is selected
            if (Selection.activeGameObject != null) {
                selectedChar = Selection.activeGameObject.GetComponent<Character>();
            }

            if (selectedChar == null) {
                EditorGUILayout.HelpBox("Select a GameObject with a Character component to inspect.", MessageType.Info);
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            DrawHeader();
            DrawEmotions();
            DrawNeeds();
            DrawMemories();

            EditorGUILayout.EndScrollView();

            // Force repaint for realtime updates
            if (Application.isPlaying) Repaint();
        }

        void DrawHeader() {
            EditorGUILayout.LabelField($"Subject: {selectedChar.name} (ID: {selectedChar.ID})", EditorStyles.largeLabel);
            EditorGUILayout.Space();
        }

        void DrawEmotions() {
            EditorGUILayout.LabelField("Emotions (Positivity vs Avoidance)", EditorStyles.boldLabel);
            
            Rect rect = GUILayoutUtility.GetRect(200, 200);
            EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f)); // BG

            // Draw Crosshair
            Vector2 center = rect.center;
            Handles.color = Color.gray;
            Handles.DrawLine(new Vector2(rect.x, center.y), new Vector2(rect.xMax, center.y));
            Handles.DrawLine(new Vector2(center.x, rect.y), new Vector2(center.x, rect.yMax));

            // Draw Point
            if (selectedChar.Emotions != null) {
                float x = selectedChar.Emotions.Positivity; // -3 to 3 approx
                float y = selectedChar.Emotions.Avoidance;
                
                // Map -3..3 to 0..200
                float mapX = center.x + (x * 30f); 
                float mapY = center.y - (y * 30f); // Invert Y for screen coords

                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(new Vector3(mapX, mapY, 0), Vector3.forward, 5f);
                
                EditorGUILayout.LabelField($"Pos: {x:F2}, Avoid: {y:F2}");
            }
        }

        void DrawNeeds() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Core Needs", EditorStyles.boldLabel);
            
            if (selectedChar.Needs != null) {
                DrawBar("Energy", selectedChar.Needs.GetNeed(ENeeds.ENERGY).Value);
                DrawBar("Social", selectedChar.Needs.GetNeed(ENeeds.SOCIAL).Value);
                DrawBar("Hunger", selectedChar.Needs.GetNeed(ENeeds.NOURISHMENT).Value);
                DrawBar("Fulfillment", selectedChar.Needs.GetNeed(ENeeds.ASPIRATIONAL).Value);
            }
        }

        void DrawBar(string label, float value) {
            // Value is 0..1 typically
            Rect r = EditorGUILayout.GetControlRect();
            EditorGUI.ProgressBar(r, value, $"{label}: {(value*100):F0}%");
        }

        void DrawMemories() {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Short Term Memory (Recent)", EditorStyles.boldLabel);
            
             if (selectedChar.Memory != null) {
                 // Inspect private list via reflection or just trust it works for now?
                 // Since we didn't expose the list publically, we can't iterate it easily here without reflection.
                 // For now, we will just show the Recalled Bias for a test topic.
                 
                 float biasSocial = selectedChar.Memory.Recall(EActivityCategory.Social);
                 EditorGUILayout.LabelField($"Social Bias: {biasSocial:F2}");
                 
                 float biasPhys = selectedChar.Memory.Recall(EActivityCategory.Physical);
                 EditorGUILayout.LabelField($"Physical Bias: {biasPhys:F2}");
             }
        }
    }
}
