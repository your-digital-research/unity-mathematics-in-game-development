using Core.Utilities;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class DotProductEditor : CommonEditor, IContextDrawer
    {
        #region PUBLIC_VARIABLES

        public Vector3 firstPoint;
        public Vector3 secondPoint;
        public Vector3 centerPoint;

        #endregion

        #region SERIALIZED_VARIABLES

        private SerializedProperty firstPointProperty;
        private SerializedProperty secondPointProperty;
        private SerializedProperty centerPointProperty;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void Reset()
        {
            firstPoint = new Vector3(0.0f, 1.0f, 0.0f);
            secondPoint = new Vector3(0.5f, 0.5f, 0.0f);
            centerPoint = Vector3.zero;
        }

        protected override void OnSceneGUI(SceneView sceneView)
        {
            DrawContext(out Vector3 first, out Vector3 second, out Vector3 center);
            CheckForRepaint(first, second, center);
            DrawLines(first, second, center);
        }

        protected override void DisplayProperties()
        {
            DrawBlockGUI("First Point", firstPointProperty);
            DrawBlockGUI("Second Point", secondPointProperty);
            DrawBlockGUI("Center Point", centerPointProperty);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            firstPointProperty = SelfObject.FindProperty("firstPoint");
            secondPointProperty = SelfObject.FindProperty("secondPoint");
            centerPointProperty = SelfObject.FindProperty("centerPoint");
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        [MenuItem("Tools/Dot Product", false, 2)]
        public static void ShowWindow()
        {
            DotProductEditor window = (DotProductEditor)GetWindow(typeof(DotProductEditor), true, "Dot Product");
            window.minSize = new Vector2(300, 115);
            window.maxSize = new Vector2(300, 115);
            window.Show();
        }

        public void DrawContext(out Vector3 first, out Vector3 second, out Vector3 center)
        {
            Handles.color = Color.red;
            first = GetMovePoint(firstPoint);

            Handles.color = Color.green;
            second = GetMovePoint(secondPoint);

            Handles.color = Color.white;
            center = GetMovePoint(centerPoint);
        }

        public void CheckForRepaint(Vector3 first, Vector3 second, Vector3 center)
        {
            if (firstPoint == first && secondPoint == second && centerPoint == center) return;

            Undo.RecordObject(this, "Tool Move");

            firstPoint = first;
            secondPoint = second;
            centerPoint = center;

            RepaintOnGUI();
        }

        public void DrawLines(Vector3 first, Vector3 second, Vector3 center)
        {
            Handles.Label(center, Utils.DotProduct(first, second, center).ToString("F2"), GuiStyle);

            Handles.color = Color.black;

            Vector3 leftSurface = GetWorldRotation(first, center, new Vector3(0f, 1f, 0f));
            Vector3 rightSurface = GetWorldRotation(first, center, new Vector3(0f, -1f, 0f));

            Handles.DrawAAPolyLine(5f, first, center);
            Handles.DrawAAPolyLine(5f, second, center);
            Handles.DrawAAPolyLine(5f, center, leftSurface);
            Handles.DrawAAPolyLine(5f, center, rightSurface);
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

        #endregion
    }
}