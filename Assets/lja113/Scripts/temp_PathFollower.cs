using System.Collections.Generic;
using UnityEngine;
using lja113;

[RequireComponent(typeof(Rigidbody))]
public class PathFollower : MonoBehaviour
{
    [Tooltip("The Transform you want to drive toward (e.g. the 'goal' object)")]
    public Transform target;

    [Tooltip("Units per second")]
    public float speed = 5f;

    [Tooltip("How close before we consider a node 'reached'")]
    public float reachThreshold = 0.2f;

    private TerrainGraph graph;
    private List<Node> path;
    private int currentIndex = 0;
    private float gridOffset = 0.5f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("goal").transform;

        // 1) Grab the graph (which you should have built on a manager or vehicle)
        graph = FindObjectOfType<TerrainGraph>();
        if (graph == null)
        {
            Debug.LogError("No TerrainGraph found in scene!");
            enabled = false;
            return;
        }

        // 2) Compute initial path
        Debug.LogWarning(graph); 
        ComputePath();
    }

    void ComputePath()
    {
        // Convert world position → grid indices
        Vector3 pos = transform.position;
        int sx = Mathf.FloorToInt(pos.x);
        int sz = Mathf.FloorToInt(pos.z);
        Node start = graph.grid[sx, sz];

        Vector3 tpos = target.position;
        int ex = Mathf.FloorToInt(tpos.x);
        int ez = Mathf.FloorToInt(tpos.z);
        Node end = graph.grid[ex, ez];

        // Run A* (here using Octile as an example)
        path = PathAlgorithm.AStar(graph, start, end, (int)PathRenderer.Algorithm.AStar_Octile);
        currentIndex = 0;
    }

    void Update()
    {
        if (path == null || currentIndex >= path.Count)
            return;

        // 3) Move toward the next node
        Node n = path[currentIndex];
        Vector3 dest = new Vector3(
            n.nodePosition.X + gridOffset,
            n.nodeHeight,
            n.nodePosition.Y + gridOffset
        );

        Vector3 dir = dest - transform.position;
        if (dir.magnitude <= reachThreshold)
        {
            // reached this node, go to next
            currentIndex++;
        }
        else
        {
            // drive straight toward it
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }
}
