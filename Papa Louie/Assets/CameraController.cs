using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse Y") * sensitivity;
        var mouseY = Input.GetAxis("Mouse X") * sensitivity;
        mouseX = Mathf.Clamp(mouseX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(mouseX, mouseY, 0f);
    }
}
