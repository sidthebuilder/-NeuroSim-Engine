using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CharacterModel {

    [CustomPropertyDrawer(typeof(PersonalityDevelopment))]
    public class PersonalityDevelopmentEditor : PropertyDrawer {

        SerializedProperty xp;
        SerializedProperty level;

        bool complexMode = false;

        public void Init(SerializedProperty property) {
            xp = property.FindPropertyRelative("xp");
            level = property.FindPropertyRelative("level");

        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Init(property);
            EditorGUI.BeginProperty(position, label, property);

            Rect foldOutRect = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldOutRect, property.isExpanded, label);
            if(property.isExpanded) {
                DrawLevelProperty(position, 1, level, "Level");
                float down = EditorGUIUtility.singleLineHeight * 2.0f;
                Rect complexRect = new Rect(position.min.x + 10, position.min.y + down, position.size.x, EditorGUIUtility.singleLineHeight);
                complexMode = EditorGUI.Foldout(complexRect, complexMode, new GUIContent("Full Data"));
                if(complexMode) {
                    DrawLevelProperty(position, 3, xp, "Skill XP");
                }
            }

            if(complexMode) {
                level.intValue = PersonalityDevelopment.LevelForXPEd(xp.doubleValue);
            } else {
                xp.doubleValue = PersonalityDevelopment.XPForLevelED(level.intValue);
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
                    numLines += 1;
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