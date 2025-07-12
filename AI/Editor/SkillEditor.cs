using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CharacterModel {

    [CustomPropertyDrawer(typeof(Skill))]
    public class SkillEditor : PropertyDrawer {

        SerializedProperty xp;
        SerializedProperty minXp;
        SerializedProperty level;
        SerializedProperty highestReached;
        SerializedProperty bonus;
        SerializedProperty lastUsed;

        bool complexMode = false;

        public void Init(SerializedProperty property) {
            xp = property.FindPropertyRelative("xp");
            minXp = property.FindPropertyRelative("minXp");
            level = property.FindPropertyRelative("level");
            highestReached = property.FindPropertyRelative("highestReached");
            bonus = property.FindPropertyRelative("bonus");
            lastUsed = property.FindPropertyRelative("lastUsed");

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
                    DrawLevelProperty(position, 4, minXp, "Skill XP Floor");
                    DrawLevelProperty(position, 5, highestReached, "Max Level Achieved");
                    DrawLevelProperty(position, 6, bonus, "Talent Bonus");
                    DrawLevelProperty(position, 7, lastUsed, "Time of Last Use");
                }
            }

            if(complexMode) {
                level.intValue = Skill.LevelForXPEd(xp.doubleValue);
            } else {
                xp.doubleValue = Skill.XPForLevelED(level.intValue);
            }

            minXp.floatValue = Mathf.Max(Mathf.Sqrt((float)xp.doubleValue), minXp.floatValue);
            highestReached.intValue = level.intValue;

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
                    numLines += 5;
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