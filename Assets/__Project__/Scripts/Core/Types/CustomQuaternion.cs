namespace Core.Types
{
    public struct CustomQuaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public CustomQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}