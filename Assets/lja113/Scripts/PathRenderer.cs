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

        [Header("Info Refs")]
        public TextMeshProUGUI startPosText;
        public TextMeshProUGUI endPosText;
        public TextMeshProUGUI accumulatedCostText;
        public TextMeshProUGUI nodeNumText;


        private TerrainGraph graph;
        private float gridOffset = 0.5f;
        private List<Node> path = new List<Node>();

        void Start()
        {
            graph = new TerrainGraph();
            start = this.transform;
            end = GameObject.FindGameObjectWithTag("goal").transform;

            Debug.LogWarning(graph);
            Debug.LogWarning("Start Position: " + start);
            Debug.LogWarning("End Position: " + end);
        }

        void Update()
        {
            RenderPath();
            //Debug.Log(path.Count);
            //UpdateInfo();
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
