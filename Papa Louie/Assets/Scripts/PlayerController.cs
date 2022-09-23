using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedModifier = 6f;
    private Rigidbody _rb;
    
    
    private Transform _cam;
    public float sensitivity = 2f;
    private float _xRotation, _yRotation;


    private void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _cam = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        var playerVerticalRelative = Input.GetAxis("Vertical") * transform.forward.normalized;
        var playerHorizontalRelative = Input.GetAxis("Horizontal") * transform.right.normalized;
        var cameraMovementRelative = playerVerticalRelative + playerHorizontalRelative;
        cameraMovementRelative.y = 0f;
        _rb.velocity = cameraMovementRelative * speedModifier;
        
        _xRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        _yRotation += Input.GetAxis("Mouse X") * sensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        _cam.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        _rb.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
