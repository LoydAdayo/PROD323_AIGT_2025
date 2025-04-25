namespace lja113
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class PathRenderer : MonoBehaviour
    {
        public Transform start;
        public Transform end;
        public LineRenderer lr;

        public enum Algorithm
        {
            BreadthFirstSearch,
            Dijkstra,
            AStar,
            AStar_Euclidian,
            AStar_Manhattan,
            AStar_Diagonal,
            AStar_Octile
        }

        public Algorithm algorithm = Algorithm.BreadthFirstSearch;

        [Header("Movement Settings")]
        public float speed = 5f;
        public float acceleration = 5f;
        public float reachThreshold = 1f;

        private int currentIndex = 0;
        private Rigidbody rb;

        [Header("Info Refs")]
        public TextMeshProUGUI startPosText;
        public TextMeshProUGUI endPosText;
        public TextMeshProUGUI accumulatedCostText;
        public TextMeshProUGUI nodeNumText;


        private TerrainGraph graph;
        private float gridOffset = 0.5f;
        private List<Node> path = new List<Node>();

        void Awake()
        {
            graph = new TerrainGraph();
            start = this.transform;
            end = GameObject.FindGameObjectWithTag("goal").transform;

            //Debug.LogWarning(graph);
            Debug.LogWarning("Start Position: " + start);
            Debug.LogWarning("End Position: " + end);

            rb = GetComponent<Rigidbody>();

            RenderPath();
        }

        void Update()
        {
            
            //Debug.Log(path.Count);
            //UpdateInfo();
            MoveAlongPath();
        }

        private void RenderPath()
        {
            // Start node position
            int startX = (int)start.position.x;
            int startZ = (int)start.position.z;

            Node startNode = graph.grid[startX, startZ];

            // End node position
            int endX = (int)end.position.x;
            int endZ = (int)end.position.z;

            Node endNode = graph.grid[endX, endZ];

            // Get list of nodes making the path based on selected algorithm

            switch (algorithm)
            {
                case Algorithm.BreadthFirstSearch:
                    path = PathAlgorithm.BFS(graph, startNode, endNode);
                    break;
                case Algorithm.Dijkstra:
                    path = PathAlgorithm.Dijkstra(graph, startNode, endNode);
                    break;
                case Algorithm.AStar:
                    path = PathAlgorithm.AStar(graph, startNode, endNode, (int)Algorithm.AStar);
                    break;
                case Algorithm.AStar_Euclidian:
                    path = PathAlgorithm.AStar(graph, startNode, endNode, (int)Algorithm.AStar_Euclidian);
                    break;
                case Algorithm.AStar_Manhattan:
                    path = PathAlgorithm.AStar(graph, startNode, endNode, (int)Algorithm.AStar_Manhattan);
                    break;
                case Algorithm.AStar_Diagonal:
                    path = PathAlgorithm.AStar(graph, startNode, endNode, (int)Algorithm.AStar_Diagonal);
                    break;
                case Algorithm.AStar_Octile:
                    path = PathAlgorithm.AStar(graph, startNode, endNode, (int)Algorithm.AStar_Octile);
                    break;
            }

            if(path.Count == 0)
            {
                Debug.Log("No path found");
            }



            // Create the line using an array of vertices based on the nodes in the path
            // Grid offset points to the center of the node cell
            Vector3[] lineVertices = new Vector3[path.Count];
            int index = 0;

            foreach (Node n in path)
            {
                float x = n.nodePosition.X + gridOffset;
                float y = n.nodeHeight + gridOffset;
                float z = n.nodePosition.Y + gridOffset;

                lineVertices[index++] = new Vector3(x, y, z);
            }

            // Render the path
            lr.positionCount = path.Count;
            lr.SetPositions(lineVertices);

            // Debug nodes in path
            /* Debug.LogWarning($"[Path] count = {path.Count}");
            for (int i = 0; i < path.Count; i++)
            {
                var n = path[i];
                Debug.LogWarning($"[Path] {i}: ({n.nodePosition.X}, {n.nodePosition.Y}), h={n.nodeHeight}");
            } */
        }

        void MoveAlongPath()
        {
            // Destination of current node
            Node n = path[currentIndex];
            Vector3 worldTarget = new Vector3(
                n.nodePosition.X + gridOffset,
                start.position.y,
                n.nodePosition.Y + gridOffset
            );

            // Calculate Direction & flatten distance
            Vector3 toTarget = worldTarget - start.position;
            Vector3 toTargetFlat = new Vector3(toTarget.x, 0, toTarget.z);
            float flatDist = toTargetFlat.magnitude;

            // Has reached node target or no?
            if (flatDist <= reachThreshold)
            {
                Debug.Log($"Reached node {currentIndex}, advancing to {currentIndex+1}");
                currentIndex++;
                // zero horizontal velocity so not carry on
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                return;
            }

            Vector3 desiredVel = toTargetFlat.normalized * speed;

            rb.linearVelocity = new Vector3(desiredVel.x, rb.linearVelocity.y, desiredVel.z);
        
            Debug.DrawLine(start.position, worldTarget, Color.yellow);
            Debug.Log($"Heading to node {currentIndex}/{path.Count-1} at {worldTarget}");
        }

        private void UpdateInfo()
        {
            startPosText.text = "(" + Mathf.FloorToInt(start.transform.position.x) + ", " + Mathf.FloorToInt(start.transform.position.z) + ")";
            endPosText.text = "(" + Mathf.FloorToInt(end.transform.position.x) + ", " + Mathf.FloorToInt(end.transform.position.z) + ")";
            accumulatedCostText.text = PathAlgorithm.totalCost.ToString();
            nodeNumText.text = path.Count.ToString();
        }
    }

}
