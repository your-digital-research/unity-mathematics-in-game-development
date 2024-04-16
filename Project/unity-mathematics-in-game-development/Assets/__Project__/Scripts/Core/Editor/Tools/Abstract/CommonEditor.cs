using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public abstract class CommonEditor : EditorWindow
    {
        #region PROTECTED_VARIABLES

        protected SerializedObject SelfObject;
        protected readonly GUIStyle GuiStyle = new GUIStyle();

        #endregion

        #region ABSTRACT_FUNCTIONS

        protected abstract void ResetValues();
        protected abstract void OnSceneGUI(SceneView sceneView);
        protected abstract void DisplayProperties();

        #endregion

        #region MONO

        private void OnEnable()
        {
            ResetValues();

            InitGUIStyle();
            InitProperties();

            AddListeners();
        }

        private void OnGUI()
        {
            UpdateGUI();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        #endregion

        #region VIRTUAL_FUNCTIONS

        protected virtual void InitProperties()
        {
            SelfObject = new SerializedObject(this);
        }

        #endregion

        #region PROTECTED_FUNCTIONS

        protected void RepaintOnGUI()
        {
            Repaint();
        }

        protected void DrawBlockGUI(string label, SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(label, GUILayout.Width(100));
            EditorGUILayout.PropertyField(property, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void AddListeners()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            Undo.undoRedoPerformed += RepaintOnGUI;
        }

        private void RemoveListeners()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            Undo.undoRedoPerformed -= RepaintOnGUI;
        }

        private void InitGUIStyle()
        {
            GuiStyle.fontSize = 16;
            GuiStyle.fontStyle = FontStyle.Bold;
            GuiStyle.normal.textColor = Color.white;
        }

        private void UpdateGUI()
        {
            SelfObject.Update();

            DisplayProperties();
            DisplayResetButton();

            if (SelfObject.ApplyModifiedProperties()) SceneView.RepaintAll();
        }

        private void DisplayResetButton()
        {
            if (!GUILayout.Button("Reset")) return;

            ResetValues();

            SceneView.RepaintAll();
        }

        #endregion
    }
}