using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public float distancefromPlayer = 5.0f;
    public float sensitivity = 2.0f;
    public float minYAngle = -30.0f;
    public float maxYAngle = 60.0f;
    private float Vertical = 0.0f;
    private float Horizontal = 0.0f;
    
    void LateUpdate()
    {
        Horizontal += Input.GetAxis("Mouse X") * sensitivity;
        Vertical -= Input.GetAxis("Mouse Y") * sensitivity;
        Vertical = Mathf.Clamp(Vertical, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(Vertical, Horizontal, 0);
        Vector3 position = player.position - (rotation * Vector3.forward * distancefromPlayer);

        transform.rotation = rotation;
        transform.position = position;
    }
}
