using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerAcceleration = 10f;
    public float speedCap = 10f;
    Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;
    private Transform Cam;
    public static PlayerController instance;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Cam = Camera.main.transform;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if (moveDirection.magnitude != 0) rb.velocity += Mathf.Clamp(PlayerAcceleration - rb.velocity.magnitude, 0, speedCap) * Time.deltaTime * moveDirection;
        //else rb.velocity *= 1 - Time.deltaTime * 10f;

        rb.velocity = Vector2.Lerp(rb.velocity, moveDirection * speedCap, Time.deltaTime * PlayerAcceleration);
        Cam.position = transform.position + new Vector3(0, 0, -10f);
    }
}
