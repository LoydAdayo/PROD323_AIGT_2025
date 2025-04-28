using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 5f;
    public float moveSpeed = 2f;    

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate new position
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        transform.position = startPosition + transform.right * offset;
    }
}