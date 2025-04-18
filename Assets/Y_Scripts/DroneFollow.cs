using UnityEngine;

public class DroneFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the inspector
    public Vector3 offset = new Vector3(2,1, 2);
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        if (player == null) return;

        // Move the drone toward the target position with an offset
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Calculate direction to player
        Vector3 directionToPlayer = player.position - transform.position;
        if (directionToPlayer != Vector3.zero)
        {
            // Calculate the rotation needed to look at the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the drone towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


