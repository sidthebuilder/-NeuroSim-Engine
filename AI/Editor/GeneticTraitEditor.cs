using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CharacterModel {

    [CustomPropertyDrawer(typeof(GeneticTrait))]
    public class GeneticTraitEditor : PropertyDrawer {

        SerializedProperty geneA;
        SerializedProperty geneB;
        SerializedProperty fake;

        SerializedProperty raw;
        SerializedProperty value;

        bool complexMode = false;

        public void Init(SerializedProperty property) {
            geneA = property.FindPropertyRelative("geneA");
            geneB = property.FindPropertyRelative("geneB");
            fake = property.FindPropertyRelative("fake");
            raw = property.FindPropertyRelative("raw");
            value = property.FindPropertyRelative("value");
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Init(property);
            EditorGUI.BeginProperty(position, label, property);

            Rect foldOutRect = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldOutRect, property.isExpanded, label);
            if(property.isExpanded) {
                DrawLevelProperty(position, 1, value, "Value");
                float down = EditorGUIUtility.singleLineHeight * 2.0f;
                Rect complexRect = new Rect(position.min.x + 10, position.min.y + down, position.size.x, EditorGUIUtility.singleLineHeight);
                complexMode = EditorGUI.Foldout(complexRect, complexMode, new GUIContent("Full Data"));
                if(complexMode) {
                    DrawLevelProperty(position, 3, geneA, "Gene A");
                    DrawLevelProperty(position, 4, geneB, "Gene B");
                    DrawLevelProperty(position, 5, fake, "Complexity (\"fake gene\"))");
                }
            }

            if(complexMode) {
                value.intValue = GeneticTrait.ValueFromGenesEd(geneA.floatValue, geneB.floatValue, fake.floatValue);
            } else {
                float gene = GeneticTrait.GenesFromValueEd(value.intValue);
                geneA.floatValue = gene;
                geneB.floatValue = gene;
                fake.floatValue = gene;
            }

            EditorGUI.EndProperty();
        }





        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            int numLines = 1;

            // Increase when expanded
            if(property.isExpanded) {
                lineHeight *= 1.1f;
                numLines += 2;
                if(complexMode) {
                    numLines += 3;
                }
            }


            return (float)(lineHeight * numLines);
        }


        private void DrawLevelProperty(Rect position, int line, SerializedProperty property, string name) {
            float x = position.min.x;
            float y = position.min.y + (EditorGUIUtility.singleLineHeight * line);
            float w = position.size.x;
            float h = EditorGUIUtility.singleLineHeight;

            Rect drawArea = new Rect(x, y, w, h);
            EditorGUI.PropertyField(drawArea, property, new GUIContent(name));
        }

    }

}