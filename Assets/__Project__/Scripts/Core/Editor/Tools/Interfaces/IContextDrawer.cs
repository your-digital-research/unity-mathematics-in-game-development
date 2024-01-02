using UnityEngine;

namespace Core.Editor
{
    public interface IContextDrawer
    {
        public void DrawContext(out Vector3 a, out Vector3 b, out Vector3 c);
        public void CheckForRepaint(Vector3 a, Vector3 b, Vector3 c);
        public void DrawLines(Vector3 a, Vector3 b, Vector3 c);
    }
}