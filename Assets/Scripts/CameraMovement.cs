using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 2f;
    public float yRotationLimit = 88f; // Limits vertical camera rotation

    private Vector2 rotation = Vector2.zero;

    void Update()
    {
        if (GameManager.Instance.isPaused)
            return;
        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        // Apply horizontal rotation to the parent (player character)
        transform.parent.localRotation = Quaternion.Euler(0, rotation.x, 0);

        // Apply vertical rotation to the camera itself
        transform.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
    }
}
