using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedModifier = 6f;
    private Rigidbody _rb;
    private Transform _cam;

    
    public void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _cam = GetComponentInChildren<Camera>().transform;
    }
    
    public void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var playerVerticalRelative = vertical * _cam.forward;
        var playerHorizontalRelative = horizontal * _cam.right;
        var cameraMovementRelative = playerVerticalRelative + playerHorizontalRelative;
        _rb.velocity = cameraMovementRelative * speedModifier;
    }
}
