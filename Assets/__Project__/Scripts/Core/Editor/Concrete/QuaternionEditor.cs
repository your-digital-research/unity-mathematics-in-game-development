using Core.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class QuaternionEditor : CommonEditor
    {
        #region PUBLIC_VARIABLES

        [Range(-1, 1)] public float x;
        [Range(-1, 1)] public float y;
        [Range(-1, 1)] public float z;
        [Range(-360, 360)] public float angle;

        #endregion

        #region PRIVATE_VARIABLES

        private List<Vector3> vertices;

        private SerializedProperty xProperty;
        private SerializedProperty yProperty;
        private SerializedProperty zProperty;
        private SerializedProperty angleProperty;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void Reset()
        {
            x = 0;
            y = 1;
            z = 0;
            angle = 0;
        }

        protected override void OnSceneGUI(SceneView sceneView)
        {
            DrawCube();
        }

        protected override void DisplayProperties()
        {
            DrawBlockGUI("X", xProperty);
            DrawBlockGUI("Y", yProperty);
            DrawBlockGUI("Z", zProperty);
            DrawBlockGUI("Angle", angleProperty);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            xProperty = SelfObject.FindProperty("x");
            yProperty = SelfObject.FindProperty("y");
            zProperty = SelfObject.FindProperty("z");
            angleProperty = SelfObject.FindProperty("angle");
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        [MenuItem("Tools/Quaternion", false, 0)]
        public static void ShowWindow()
        {
            QuaternionEditor window = (QuaternionEditor)GetWindow(typeof(QuaternionEditor), true, "Quaternion");
            window.minSize = new Vector2(300, 145);
            window.maxSize = new Vector2(300, 145);
            window.Show();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void DrawCube()
        {
            vertices = new List<Vector3>
            {
                new(-0.5f, 0.5f, 0.5f),
                new(0.5f, 0.5f, 0.5f),
                new(0.5f, -0.5f, 0.5f),
                new(-0.5f, -0.5f, 0.5f),
                new(-0.5f, 0.5f, -0.5f),
                new(0.5f, 0.5f, -0.5f),
                new(0.5f, -0.5f, -0.5f),
                new(-0.5f, -0.5f, -0.5f)
            };

            float newAngle = angle * Mathf.PI / 180;

            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = Utils.RotateQuaternion(vertices[i], new Vector3(x, y, z), newAngle);
                Handles.SphereHandleCap(0, vertices[i], Quaternion.identity, 0.1f, EventType.Repaint);
            }

            int[][] index =
            {
                new[] { 0, 1 },
                new[] { 1, 2 },
                new[] { 2, 3 },
                new[] { 3, 0 },
                new[] { 4, 5 },
                new[] { 5, 6 },
                new[] { 6, 7 },
                new[] { 7, 4 },
                new[] { 4, 0 },
                new[] { 5, 1 },
                new[] { 6, 2 },
                new[] { 7, 3 },
            };

            for (int i = 0; i < index.Length; i++)
            {
                Handles.DrawAAPolyLine(vertices[index[i][0]], vertices[index[i][1]]);
            }
        }

        #endregion
    }
}