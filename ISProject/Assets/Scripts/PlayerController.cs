using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input from keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create movement vector
        Vector3 movement = new Vector3(moveX, 0, moveZ);

        // Apply movement
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}

