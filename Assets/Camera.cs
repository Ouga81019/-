using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 2f;
    public float sensitivity = 200f;

    float yaw = 0f;
    float pitch = 20f;

    void LateUpdate()
    {
        if (!target) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -30f, 70f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 direction = rotation * Vector3.back;
        Vector3 desiredPos = target.position + Vector3.up * height + direction * distance;

        transform.position = desiredPos;
        transform.LookAt(target.position + Vector3.up * height);
    }
}


