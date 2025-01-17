using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    private Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;

    private bool thrusting; //move forward
    private float turnDirection;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Checking input
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rigidBody.AddForce(this.transform.up * this.thrustSpeed);
        }
        if (turnDirection != 0.0f)
        {
            rigidBody.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);

        FindObjectOfType<AudioManager>().Play("Shoot");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            //stop all players movement
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Explosion");
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
