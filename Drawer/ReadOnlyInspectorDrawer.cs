using UnityEditor;
using UnityEngine;

namespace CustomEditor.Attributes.Drawer
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string value;
            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    value = property.objectReferenceValue == null ? "null" : property.objectReferenceValue.name;
                    break;
                case SerializedPropertyType.Vector2:
                    value = property.vector2Value.ToString();
                    break;
                case SerializedPropertyType.Vector3:
                    value = property.vector3Value.ToString();
                    break;
                case SerializedPropertyType.Integer:
                    value = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    value = property.boolValue.ToString();
                    break;
                case SerializedPropertyType.Enum:
                    value = property.enumValueIndex.ToString();
                    break;
                case SerializedPropertyType.String:
                    value = property.stringValue.ToString();
                    break;
                default:
                    value = "(not supported)";
                    break;
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.LabelField(position, label.text, value);
            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}