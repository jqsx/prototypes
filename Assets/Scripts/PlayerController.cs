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
    public float cameraShake = 0f;

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

        cameraShake = Mathf.Lerp(cameraShake, 0f, Time.deltaTime * 5f);

        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if (moveDirection.magnitude != 0) rb.velocity += Mathf.Clamp(PlayerAcceleration - rb.velocity.magnitude, 0, speedCap) * Time.deltaTime * moveDirection;
        //else rb.velocity *= 1 - Time.deltaTime * 10f;
        movement();

        Cam.position = transform.position + new Vector3(0, Mathf.Sin(Time.time * 20f) * Mathf.Clamp(cameraShake, -1f, 1f) / 10f, -10f);
        Cam.rotation = Quaternion.Euler(0, 0, cameraShake);

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

    public static void CameraShake(float force = 1f)
    {
        float shake = instance.cameraShake;
        instance.cameraShake = -(shake + Mathf.Abs(force) * Mathf.Sign(shake));
    }
}
