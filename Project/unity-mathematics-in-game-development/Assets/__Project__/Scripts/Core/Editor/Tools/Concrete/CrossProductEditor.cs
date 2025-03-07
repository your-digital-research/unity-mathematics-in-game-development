using Core.Utilities;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class CrossProductEditor : CommonEditor, IContextDrawer
    {
        #region PUBLIC_VARIABLES

        public Vector3 firstVector;
        public Vector3 secondVector;
        public Vector3 crossProduct;

        #endregion

        #region SERIALIZED_VARIABLES

        private SerializedProperty firstVectorProperty;
        private SerializedProperty secondVectorProperty;
        private SerializedProperty crossProductProperty;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void ResetValues()
        {
            firstVector = new Vector3(0.0f, 1.0f, 0.0f);
            secondVector = new Vector3(1.0f, 0.0f, 0.0f);
            crossProduct = Utils.CrossProduct(firstVector, secondVector);
        }

        protected override void OnSceneGUI(SceneView sceneView)
        {
            DrawContext(out Vector3 first, out Vector3 second, out Vector3 cross);
            CheckForRepaint(first, second, cross);
            DrawLines(first, second, cross);
        }

        protected override void DisplayProperties()
        {
            DrawBlockGUI("First Vector", firstVectorProperty);
            DrawBlockGUI("Second Vector", secondVectorProperty);
            DrawBlockGUI("Cross Product", crossProductProperty);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            firstVectorProperty = SelfObject.FindProperty("firstVector");
            secondVectorProperty = SelfObject.FindProperty("secondVector");
            crossProductProperty = SelfObject.FindProperty("crossProduct");
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        [MenuItem("Tools/Cross Product", false, 3)]
        public static void ShowWindow()
        {
            CrossProductEditor window = (CrossProductEditor)GetWindow(typeof(CrossProductEditor), true, "Cross Product");
            window.minSize = new Vector2(300, 115);
            window.maxSize = new Vector2(300, 115);
            window.Show();
        }

        public void DrawContext(out Vector3 first, out Vector3 second, out Vector3 cross)
        {
            first = Handles.PositionHandle(firstVector, Quaternion.identity);
            second = Handles.PositionHandle(secondVector, Quaternion.identity);

            Handles.color = Color.blue;

            cross = Utils.CrossProduct(first, second);

            Handles.DrawSolidDisc(cross, Vector3.forward, 0.05f);
        }

        public void CheckForRepaint(Vector3 first, Vector3 second, Vector3 cross)
        {
            if (firstVector == first && secondVector == second) return;

            Undo.RecordObject(this, "Tool Move");

            firstVector = first;
            secondVector = second;
            crossProduct = cross;

            RepaintOnGUI();
        }

        public void DrawLines(Vector3 first, Vector3 second, Vector3 cross)
        {
            DrawLineGUI(first, "First Vector", Color.green);
            DrawLineGUI(second, "Second Vector", Color.red);
            DrawLineGUI(cross, "Cross Product", Color.blue);
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void DrawLineGUI(Vector3 point, string text, Color color)
        {
            Handles.color = color;
            Handles.Label(point, text, GuiStyle);
            Handles.DrawAAPolyLine(5f, point, Vector3.zero);
        }

        #endregion
    }
}