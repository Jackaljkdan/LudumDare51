using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JK.Utils;
using System.Reflection;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace JK.Injection.PropertyDrawers
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(InjectedAttribute))]
    public class InjectedPropertyDrawer : PropertyDrawer
    {
        public static readonly GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonStyle.alignment = TextAnchor.MiddleLeft;

            float buttonWidth = position.width / 2;

            position.width -= buttonWidth;
            EditorGUI.LabelField(position, label + " (Injected)", GUI.skin.label);
            position.x += position.width;
            position.width = buttonWidth;

            if (!property.propertyType.IsUnityObjectType())
            {
                EditorGUI.LabelField(position, $"{GetInjectedValue(property)} ({fieldInfo.FieldType.Name})", GUI.skin.box);
                return;
            }

            bool clicked = GUI.Button(position, $"{GetInjectedValue(property)} ({fieldInfo.FieldType.Name})", ButtonStyle);

            if (clicked)
            {
                object injectedValue = GetInjectedValue(property);

                Assert.IsTrue(injectedValue is UnityEngine.Object, "Field is not a unity object despite property type " + property.propertyType);

                EditorGUIUtility.PingObject((UnityEngine.Object)injectedValue);
            }
        }

        private object GetInjectedValue(SerializedProperty property)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                return "Unavailable in prefab mode";

            if (!(property.serializedObject.targetObject is Component component))
                return null;

            InitContexts(component);

            MethodInfo awakeMethod = component.GetType().GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (awakeMethod == null)
            {
                Debug.LogError(component.GetType() + " has no Awake method");
                return null;
            }

            awakeMethod.Invoke(component, null);

            object injectedValue = component.GetType().GetField(property.name).GetValue(component);
            return injectedValue;
        }

        private void InitContexts(Component monoBehaviour)
        {
            var monoContexts = monoBehaviour.GetComponentsInParent<MonoContext>();

            var projectContext = ProjectContext.Get();

            if (projectContext != null)
            {
                projectContext.Clear();
                projectContext.InitIfNeeded();
                InitBindings(projectContext);
            }

            if (monoContexts.Length > 0)
            {
                for (int i = monoContexts.Length - 1; i >= 0; i--)
                {
                    monoContexts[i].Clear();
                    monoContexts[i].InitIfNeeded();
                }

                InitBindings(monoContexts[monoContexts.Length - 1]);
            }
        }

        private void InitBindings(MonoBehaviour root)
        {
            foreach (var binding in root.GetComponentsInChildren<InjectionBinding>())
                binding.Bind();
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class InjectedAttribute : PropertyAttribute
    {
    }
}