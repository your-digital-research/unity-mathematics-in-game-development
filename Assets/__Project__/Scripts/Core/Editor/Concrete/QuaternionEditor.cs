using Core.Editor;
using UnityEditor;
using UnityEngine;

public class QuaternionEditor : CommonEditor
{
    #region OVERRIDDEN_FUNCTIONS

    protected override void Reset()
    {
        throw new System.NotImplementedException();
    }

    protected override void DisplayProperties()
    {
        throw new System.NotImplementedException();
    }

    protected override void DrawContext(out Vector3 a, out Vector3 b, out Vector3 c)
    {
        throw new System.NotImplementedException();
    }

    protected override void CheckForRepaint(Vector3 a, Vector3 b, Vector3 c)
    {
        throw new System.NotImplementedException();
    }

    protected override void DrawLines(Vector3 a, Vector3 b, Vector3 c)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region PUBLIC_FUNCTIONS

    [MenuItem("Tools/Quaternion", false, 0)]
    public static void ShowWindow()
    {
        QuaternionEditor window = (QuaternionEditor)GetWindow(typeof(QuaternionEditor), true, "Quaternion");
        window.minSize = new Vector2(300, 115);
        window.maxSize = new Vector2(300, 115);
        window.Show();
    }

    #endregion
}