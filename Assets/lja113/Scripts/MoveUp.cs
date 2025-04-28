using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;    // How fast it moves
    public Vector3 moveDirection = new Vector3(0, 1, 0); // Direction to move
    public float moveDistance = 5f; // How far it should move before stopping

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move constantly in the specified direction until moveDistance is reached
        float traveledDistance = Vector3.Distance(startPosition, transform.position);
        
        if (traveledDistance < moveDistance)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }
    }
}