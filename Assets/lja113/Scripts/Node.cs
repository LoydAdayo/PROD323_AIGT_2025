namespace lja113
{
    using System.Numerics;

    public class Node
    {
        public Vector2 nodePosition;
        public float nodeHeight;
        public float priority;

        public Node(int x, int z)
        {
            nodePosition = new Vector2(x, z);
        }
    }
}


