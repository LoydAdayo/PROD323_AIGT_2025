namespace lja113
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TerrainGraph : MonoBehaviour
    {
        private TerrainData tData;
        private int tWidth;
        private int tLength;
        private float gridOffset = 0.5f;

        public Node[,] grid;
        public float[,,] cost;

        private float maxHeight = 5f; // If node cost is over 5, it is considered impassable

        public TerrainGraph()
        {
            // Get reference of the active terrain on the scene
            tData = Terrain.activeTerrain.terrainData;

            // Create a representation of the graph using the terrain size
            // Taking the x (width) and z (length) values only
            // Grid offset points to the center of the node cell

            tWidth = Mathf.FloorToInt(tData.size.x);
            tLength = Mathf.FloorToInt(tData.size.z);
            grid = new Node[tWidth, tLength];
            cost = new float[tWidth, tLength, 8]; // cost towards each 8 direction from current node

            // Populate the grid with nodes
            for (int x = 0; x < tWidth; x++)
            {
                for (int z = 0; z < tLength; z++)
                {
                    grid[x, z] = new Node(x, z);
                    grid[x, z].nodeHeight = Terrain.activeTerrain.SampleHeight(new Vector3(x + gridOffset, 0, z + gridOffset));
                }
            }

            // Store costs (height difference between nodes) of the terrain in cost array
            for (int x = 0; x < tWidth; x++)
            {
                for (int z = 0; z < tLength; z++)
                {
                    if (x > 0 && z > 0 && x < tWidth - 1 && z < tLength - 1)
                    {
                        // Set the proper edge/connection cost for each of the 8 directions
                        cost[x, z, 0] = Mathf.Abs(grid[x, z].nodeHeight - grid[x - 1, z].nodeHeight); // west of current node  
                        cost[x, z, 1] = Mathf.Abs(grid[x, z].nodeHeight - grid[x - 1, z + 1].nodeHeight); // north-west of current node  
                        cost[x, z, 2] = Mathf.Abs(grid[x, z].nodeHeight - grid[x, z + 1].nodeHeight); // north of current node  
                        cost[x, z, 3] = Mathf.Abs(grid[x, z].nodeHeight - grid[x + 1, z + 1].nodeHeight); // north-east of current node  
                        cost[x, z, 4] = Mathf.Abs(grid[x, z].nodeHeight - grid[x + 1, z].nodeHeight); // east of current node  
                        cost[x, z, 5] = Mathf.Abs(grid[x, z].nodeHeight - grid[x + 1, z - 1].nodeHeight); // south-east of current node  
                        cost[x, z, 6] = Mathf.Abs(grid[x, z].nodeHeight - grid[x, z - 1].nodeHeight); // south of current node  
                        cost[x, z, 7] = Mathf.Abs(grid[x, z].nodeHeight - grid[x - 1, z - 1].nodeHeight); // south-west of current node  
                    }
                }
            }
        }

        public List<Node> GetNeighbours(Node n)
        {
            List<Node> neighbours = new List<Node>();

            // Take all the nodes from all cardinal and ordinal directions
            // Assume current node is at Vector2(0,0)

            Vector2[] directions =
            {
                new Vector2(-1, 0), // west
                new Vector2(-1, 1), // north-west
                new Vector2(0, 1),  // north
                new Vector2(1, 1),  // north-east
                new Vector2(1, 0),  // east
                new Vector2(1, -1), // south-east
                new Vector2(0, -1), // south
                new Vector2(-1, -1) // south-west
            };

            // Find all nodes via the 8 directions
            foreach (Vector2 dir in directions)
            {
                Vector2 v = new Vector2(dir.x, dir.y) + new Vector2(n.nodePosition.X, n.nodePosition.Y);

                // Check if the neighbouring node actually exist in the terrain
                // y here is actually the z
                bool doExist = (v.x >= 0 && v.x < tWidth && v.y >= 0 && v.y < tLength) ? true : false;
                Debug.Log(dir.x + "," + dir.y + "," + n.nodePosition.X + "," + n.nodePosition.Y + "," + tWidth + "," + tLength);
                if(!doExist)
                {
                    continue;
                }
                // Check if the neighbouring node is too high. If it is, deem it impassable
                bool passable = grid[(int)v.x, (int)v.y].nodeHeight < maxHeight; 

                if (doExist && passable)
                {
                    neighbours.Add(grid[(int)v.x, (int)v.y]);
                }
            }

            return neighbours;
        }

        // This function searches and returns the least cost among all the 8 neighbors of a node
        public float NextMinimumCost(Node n)
        {
            float minCost = 5000f; // dummy value 

            for (int index = 0; index < 8; index++)
            {
                //Debug.Log(n.nodePosition.X + "," + n.nodePosition.Y + "," + index);
                if (cost[(int)n.nodePosition.X, (int)n.nodePosition.Y, index] < minCost)
                {
                    minCost = cost[(int)n.nodePosition.X, (int)n.nodePosition.Y, index];
                }
            }

            // Since graph is a tile grid, horizontal cost is 1
            return (minCost) + 1;
        }

        void OnDrawGizmosSelected()
        {
            if (grid == null) return;

            Gizmos.color = Color.yellow;
            float cellSizeX = tData.size.x / (tWidth - 1);
            float cellSizeZ = tData.size.z / (tLength - 1);

            for (int x = 0; x < tWidth; x++)
            {
                for (int z = 0; z < tLength; z++)
                {
                    Vector3 worldPos = new Vector3(
                        x * cellSizeX,
                        grid[x,z].nodeHeight,
                        z * cellSizeZ
                    );
                    Gizmos.DrawSphere(worldPos, 0.1f);
                }
            }
        }

    }

}
