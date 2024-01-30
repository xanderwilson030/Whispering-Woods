using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFollowMouse : MonoBehaviour
{
    public float rotationSpeed = 5.0f;

    private void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Make sure the z-coordinate is set to 0 for 2D space

        // Calculate the direction from the object to the mouse
        Vector3 direction = mousePosition - transform.position;

        // Calculate the rotation angle in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly interpolate the current rotation angle to the target angle
        float currentAngle = transform.rotation.eulerAngles.z;
        float lerpedAngle = Mathf.LerpAngle(currentAngle, targetAngle - 90, rotationSpeed * Time.deltaTime);

        // Rotate the object to face the mouse using the lerped angle
        transform.rotation = Quaternion.AngleAxis(lerpedAngle, Vector3.forward);
    }
}
