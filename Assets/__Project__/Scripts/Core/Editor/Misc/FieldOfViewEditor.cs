using Core.Misc;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        #region MONO

        private void OnSceneGUI()
        {
            FieldOfView fieldOfView = (FieldOfView)target;

            Handles.color = Color.white;
            var position = fieldOfView.transform.position;

            Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360, fieldOfView.ViewRadius);

            Vector3 viewAngleA = fieldOfView.GetDirectionFromAngle(-fieldOfView.ViewAngle / 2, false);
            Vector3 viewAngleB = fieldOfView.GetDirectionFromAngle(fieldOfView.ViewAngle / 2, false);

            Handles.DrawLine(position, position + viewAngleA * fieldOfView.ViewRadius);
            Handles.DrawLine(position, position + viewAngleB * fieldOfView.ViewRadius);

            Handles.color = Color.red;

            foreach (Transform visibleTarget in fieldOfView.VisibleTargets)
            {
                Handles.DrawLine(fieldOfView.transform.position, visibleTarget.position);
            }
        }

        #endregion
    }
}