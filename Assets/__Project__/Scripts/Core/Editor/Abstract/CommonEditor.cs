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

        protected abstract void Reset();
        protected abstract void DisplayProperties();
        protected abstract void DrawContext(out Vector3 a, out Vector3 b, out Vector3 c);
        protected abstract void CheckForRepaint(Vector3 a, Vector3 b, Vector3 c);
        protected abstract void DrawLines(Vector3 a, Vector3 b, Vector3 c);

        #endregion

        #region MONO

        private void OnEnable()
        {
            Reset();

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

        protected virtual void UpdateGUI()
        {
            SelfObject.Update();

            DisplayProperties();
            DisplayResetButton();

            if (SelfObject.ApplyModifiedProperties()) SceneView.RepaintAll();
        }

        protected virtual void DrawBlockGUI(string label, SerializedProperty property, float width)
        {
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(label, GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region PROTECTED_FUNCTIONS

        protected void RepaintOnGUI()
        {
            Repaint();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnSceneGUI(SceneView sceneView)
        {
            DrawContext(out Vector3 first, out Vector3 second, out Vector3 center);
            CheckForRepaint(first, second, center);
            DrawLines(first, second, center);
        }

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

        private void DisplayResetButton()
        {
            if (!GUILayout.Button("Reset")) return;

            Reset();

            SceneView.RepaintAll();
        }

        #endregion
    }
}