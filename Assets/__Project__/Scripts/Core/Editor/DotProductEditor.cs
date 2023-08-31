using Core.Utilities;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class DotProductEditor : EditorWindow
    {
        #region PUBLIC_VARIABLES

        public Vector3 firstPoint;
        public Vector3 secondPoint;
        public Vector3 centerPoint;

        #endregion

        #region SERIALIZED_VARIABLES

        private SerializedObject selfObject;
        private SerializedProperty firstPointProperty;
        private SerializedProperty secondPointProperty;
        private SerializedProperty centerPointProperty;

        #endregion

        #region PRIVATE_VARIABLES

        private readonly GUIStyle guiStyle = new GUIStyle();

        #endregion

        #region PUBLIC_FUNCTIONS

        [MenuItem("Tools/Dot Product")]
        public static void ShowWindow()
        {
            DotProductEditor window = (DotProductEditor)GetWindow(typeof(DotProductEditor), true, "Dot Product");
            window.minSize = new Vector2(300, 115);
            window.maxSize = new Vector2(300, 115);
            window.Show();
        }

        #endregion

        #region MONO

        private void OnEnable()
        {
            ResetPoints();

            InitGUIStyle();
            InitProperties();

            AddListeners();
        }

        private void OnGUI()
        {
            selfObject.Update();

            DisplayProperties();
            DisplayResetButton();

            if (selfObject.ApplyModifiedProperties()) SceneView.RepaintAll();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        #endregion

        #region PRIVATE_VARIABLES

        private Vector3 GetMovePoint(Vector3 point)
        {
            float size = HandleUtility.GetHandleSize(Vector3.zero) * 0.25f;
            Vector3 newPoint = Handles.FreeMoveHandle(point, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);

            return newPoint;
        }

        private Vector3 GetWorldRotation(Vector3 point, Vector3 center, Vector3 destination)
        {
            Vector2 direction = (point - center).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            return center + rotation * destination;
        }

        private void AddListeners()
        {
            SceneView.duringSceneGui += SceneGUI;
        }

        private void RemoveListeners()
        {
            SceneView.duringSceneGui -= SceneGUI;
        }

        private void InitGUIStyle()
        {
            guiStyle.fontSize = 25;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.normal.textColor = Color.white;
        }

        private void InitProperties()
        {
            selfObject = new SerializedObject(this);

            firstPointProperty = selfObject.FindProperty("firstPoint");
            secondPointProperty = selfObject.FindProperty("secondPoint");
            centerPointProperty = selfObject.FindProperty("centerPoint");
        }

        private void DisplayResetButton()
        {
            if (!GUILayout.Button("Reset")) return;

            ResetPoints();

            SceneView.RepaintAll();
        }

        private void DisplayProperties()
        {
            DrawBlockGUI("First Point", firstPointProperty);
            DrawBlockGUI("Second Point", secondPointProperty);
            DrawBlockGUI("Center Point", centerPointProperty);
        }

        private void ResetPoints()
        {
            firstPoint = new Vector3(0.0f, 1.0f, 0.0f);
            secondPoint = new Vector3(0.5f, 0.5f, 0.0f);
            centerPoint = Vector3.zero;
        }

        private void SceneGUI(SceneView view)
        {
            DrawPoints(out Vector3 first, out Vector3 second, out Vector3 center);
            CheckForRepaint(first, second, center);
            DrawLines(first, second, center);
        }

        private void CheckForRepaint(Vector3 first, Vector3 second, Vector3 center)
        {
            if (firstPoint == first && secondPoint == second && centerPoint == center) return;

            firstPoint = first;
            secondPoint = second;
            centerPoint = center;

            Repaint();
        }

        private void DrawBlockGUI(string label, SerializedProperty prop)
        {
            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(label, GUILayout.Width(100));
            EditorGUILayout.PropertyField(prop, GUIContent.none);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawPoints(out Vector3 first, out Vector3 second, out Vector3 center)
        {
            Handles.color = Color.red;
            first = GetMovePoint(firstPoint);

            Handles.color = Color.green;
            second = GetMovePoint(secondPoint);

            Handles.color = Color.white;
            center = GetMovePoint(centerPoint);
        }

        private void DrawLines(Vector3 first, Vector3 second, Vector3 center)
        {
            Handles.Label(center, Utils.DotProduct(first, second, center).ToString("F2"), guiStyle);

            Handles.color = Color.black;

            Vector3 leftSurface = GetWorldRotation(first, center, new Vector3(0f, 1f, 0f));
            Vector3 rightSurface = GetWorldRotation(first, center, new Vector3(0f, -1f, 0f));

            Handles.DrawAAPolyLine(5f, first, center);
            Handles.DrawAAPolyLine(5f, second, center);
            Handles.DrawAAPolyLine(5f, center, leftSurface);
            Handles.DrawAAPolyLine(5f, center, rightSurface);
        }

        #endregion
    }
}