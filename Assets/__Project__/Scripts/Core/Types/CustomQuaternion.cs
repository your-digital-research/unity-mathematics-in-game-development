namespace Core.Types
{
    public struct CustomQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public CustomQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}