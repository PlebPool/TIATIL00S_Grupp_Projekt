using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    private float _xRotation, _yRotation;
    private void Update()
    {
        _xRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        _yRotation += Input.GetAxis("Mouse X") * sensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
    }
}
