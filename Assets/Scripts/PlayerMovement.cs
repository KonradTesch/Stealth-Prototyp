using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 75;
    public KeyCode runKey;
    public float runSpeed = 130;

    private Rigidbody2D _rb;

    private float hInput;
    private float vInput;

    private 


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(runKey))
        {
            hInput = Input.GetAxisRaw("Horizontal") * runSpeed;
            vInput = Input.GetAxisRaw("Vertical") * runSpeed;
        }
        else
        {
            hInput = Input.GetAxisRaw("Horizontal") * moveSpeed;
            vInput = Input.GetAxisRaw("Vertical") * moveSpeed;
        }
        transform.rotation = GetDirection(hInput, vInput);

        float moveLimiter = 0.7f;

        if (vInput != 0 && hInput!= 0)
        {
            vInput *= moveLimiter;
            hInput *= moveLimiter;
        }
        _rb.velocity = new Vector2(hInput * Time.fixedDeltaTime, vInput * Time.fixedDeltaTime);
    }


    private Quaternion GetDirection(float hInput, float vInput)
    {
        if(hInput > 0)
        {
            if (vInput > 0)
                return Quaternion.Euler(0, 0, -45);
            else if(vInput == 0)
                return Quaternion.Euler(0, 0, -90);
            else
                return Quaternion.Euler(0, 0, -135);
        }
        else if(hInput == 0)
        {
            if (vInput > 0)
                return Quaternion.Euler(0, 0, 0);
            else if (vInput == 0)
                return Quaternion.Euler(transform.rotation.eulerAngles);
            else
                return Quaternion.Euler(0, 0, 180);
        }
        else
        {
            if (vInput > 0)
                return Quaternion.Euler(0, 0, 45);
            else if (vInput == 0)
                return Quaternion.Euler(0, 0, 90);
            else
                return Quaternion.Euler(0, 0, 135);
        }
    }
}
