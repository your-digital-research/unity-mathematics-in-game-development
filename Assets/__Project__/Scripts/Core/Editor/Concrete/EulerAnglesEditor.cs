using Core.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    public class EulerAnglesEditor : CommonEditor
    {
        #region SERIALIZED_VARIABLES

        [Range(-180, 180)] public float angleX;
        [Range(-180, 180)] public float angleY;
        [Range(-180, 180)] public float angleZ;

        #endregion

        #region PRIVATE_VARIABLES

        private SerializedProperty angleXProperty;
        private SerializedProperty angleYProperty;
        private SerializedProperty angleZProperty;

        private List<Vector3> circleX;
        private List<Vector3> circleY;
        private List<Vector3> circleZ;
        private List<Vector3> arrow;

        #endregion

        #region OVERRIDDEN_FUNCTIONS

        protected override void Reset()
        {
            angleX = 0;
            angleY = 0;
            angleZ = 0;
        }

        protected override void OnSceneGUI(SceneView sceneView)
        {
            DrawAxis();
            DrawArrow();
        }

        protected override void DisplayProperties()
        {
            DrawBlockGUI("X", angleXProperty);
            DrawBlockGUI("Y", angleYProperty);
            DrawBlockGUI("Z", angleZProperty);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            angleXProperty = SelfObject.FindProperty("angleX");
            angleYProperty = SelfObject.FindProperty("angleY");
            angleZProperty = SelfObject.FindProperty("angleZ");
        }

        #endregion

        #region PUBLIC_FUNCTIONS

        [MenuItem("Tools/Euler Angles", false, 1)]
        public static void ShowWindow()
        {
            EulerAnglesEditor window = (EulerAnglesEditor)GetWindow(typeof(EulerAnglesEditor), true, "Euler Angles");
            window.minSize = new Vector2(300, 115);
            window.maxSize = new Vector2(300, 115);
            window.Show();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void DrawAxis()
        {
            DrawYAxis();
            DrawXAxis();
            DrawZAxis();
        }

        private void DrawXAxis()
        {
            float degreeY = -angleY * Mathf.PI / 180f;
            float degreeX = angleX * Mathf.PI / 180f;

            circleX = new List<Vector3>
            {
                new Vector3(0f, 1.00f, 0.00f) * 0.9f,
                new Vector3(0f, 0.71f, -0.71f) * 0.9f,
                new Vector3(0f, 0.00f, -1.00f) * 0.9f,
                new Vector3(0f, -0.71f, -0.71f) * 0.9f,
                new Vector3(0f, -1.00f, 0.00f) * 0.9f,
                new Vector3(0f, -0.71f, 0.71f) * 0.9f,
                new Vector3(0f, 0.00f, 1.00f) * 0.9f,
                new Vector3(0f, 0.71f, 0.71f) * 0.9f,
            };

            for (int i = 0; i < 8; i++)
            {
                circleX[i] = Utils.GetPitch(degreeY) * (Utils.GetRoll(degreeX) * circleX[i]);

                Handles.color = Color.red;
                Handles.SphereHandleCap(0, circleX[i], Quaternion.identity, 0.05f, EventType.Repaint);
            }

            for (int i = 0; i < 8; i++)
            {
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(circleX[i], circleX[(i + 1) % circleX.Count]);
            }
        }

        private void DrawYAxis()
        {
            float degreeY = -angleY * Mathf.PI / 180f;

            circleY = new List<Vector3>
            {
                new Vector3(0.00f, 0f, -1.00f),
                new Vector3(0.71f, 0f, -0.71f),
                new Vector3(1.00f, 0f, 0.00f),
                new Vector3(0.71f, 0f, 0.71f),
                new Vector3(0.00f, 0f, 1.00f),
                new Vector3(-0.71f, 0f, 0.71f),
                new Vector3(-1.00f, 0f, 0.00f),
                new Vector3(-0.71f, 0f, -0.71f),
            };

            for (int i = 0; i < 8; i++)
            {
                circleY[i] = Utils.GetPitch(degreeY) * circleY[i];

                Handles.color = Color.green;
                Handles.SphereHandleCap(0, circleY[i], Quaternion.identity, 0.05f, EventType.Repaint);
            }

            for (int i = 0; i < 8; i++)
            {
                Handles.color = Color.green;
                Handles.DrawAAPolyLine(circleY[i], circleY[(i + 1) % circleY.Count]);
            }
        }

        private void DrawZAxis()
        {
            float degreeY = -angleY * Mathf.PI / 180f;
            float degreeX = angleX * Mathf.PI / 180f;
            float degreeZ = angleZ * Mathf.PI / 180f;

            circleZ = new List<Vector3>
            {
                new Vector3(0.00f, 1.00f, 0f) * 0.8f,
                new Vector3(0.71f, 0.71f, 0f) * 0.8f,
                new Vector3(1.00f, 0.00f, 0f) * 0.8f,
                new Vector3(0.71f, -0.71f, 0f) * 0.8f,
                new Vector3(0.00f, -1.00f, 0f) * 0.8f,
                new Vector3(-0.71f, -0.71f, 0f) * 0.8f,
                new Vector3(-1.00f, 0.00f, 0f) * 0.8f,
                new Vector3(-0.71f, 0.71f, 0f) * 0.8f,
            };

            for (int i = 0; i < 8; i++)
            {
                circleZ[i] = Utils.GetPitch(degreeY) * (Utils.GetRoll(degreeX) * (Utils.GetYaw(degreeZ) * circleZ[i]));

                Handles.color = Color.cyan;
                Handles.SphereHandleCap(0, circleZ[i], Quaternion.identity, 0.05f, EventType.Repaint);
            }

            for (int i = 0; i < 8; i++)
            {
                Handles.color = Color.blue;
                Handles.DrawAAPolyLine(circleZ[i], circleZ[(i + 1) % circleZ.Count]);
            }
        }

        private void DrawArrow()
        {
            float degreeY = -angleY * Mathf.PI / 180f;
            float degreeX = angleX * Mathf.PI / 180f;
            float degreeZ = angleZ * Mathf.PI / 180f;

            arrow = new List<Vector3>
            {
                new Vector3(0.20f, 0f, 0.00f) * 0.5f,
                new Vector3(0.20f, 0f, -0.50f) * 0.5f,
                new Vector3(0.35f, 0f, -0.50f) * 0.5f,
                new Vector3(0.00f, 0f, -1.00f) * 0.5f,
                new Vector3(-0.35f, 0f, -0.50f) * 0.5f,
                new Vector3(-0.20f, 0f, -0.50f) * 0.5f,
                new Vector3(-0.20f, 0f, -0.00f) * 0.5f,
            };

            for (int i = 0; i < arrow.Count; i++)
            {
                arrow[i] = Utils.GetPitch(degreeY) * (Utils.GetRoll(degreeX) * (Utils.GetYaw(degreeZ) * arrow[i]));
            }

            for (int i = 0; i < arrow.Count; i++)
            {
                Handles.color = Color.white;
                Handles.DrawAAPolyLine(arrow[i], arrow[(i + 1) % arrow.Count]);
            }
        }

        #endregion
    }
}