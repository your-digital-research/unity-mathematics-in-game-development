namespace Core.Types
{
    public struct CustomQuaternion
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        public CustomQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}