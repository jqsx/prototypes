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

    public static Vector2 mouseWorldPosition = Vector2.zero;

    public bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Cam = Camera.main.transform;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!instance) instance = this;

        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if (moveDirection.magnitude != 0) rb.velocity += Mathf.Clamp(PlayerAcceleration - rb.velocity.magnitude, 0, speedCap) * Time.deltaTime * moveDirection;
        //else rb.velocity *= 1 - Time.deltaTime * 10f;
        movement();

        Cam.position = transform.position + new Vector3(0, 0, -10f);
        updateMousePosition();
    }
    private void movement()
    {
        if (!canMove) return;
        rb.velocity = Vector2.Lerp(rb.velocity, moveDirection * speedCap, Time.deltaTime * PlayerAcceleration);
    }

    private void updateMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
