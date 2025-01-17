using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public float speed = 500.0f;
    public float maxLifeTime = 5.0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    //get the players direction and shoot the same direction 
    public void Project(Vector2 direction)
    {
        
        rigidBody.AddForce(direction * speed);
        Destroy(this.gameObject, this.maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
