using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprite;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float speed = 50.0f;
    public float maxLifetime = 20.0f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprite[Random.Range(0, sprite.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * size;
        rigidBody.mass = this.size;
    }

    // similar to the Shoot function in bullet, it gets the spawners direction and determine asteroid 
    public void SetTrajectory(Vector2 direction)
    {
        rigidBody.AddForce(direction * this.speed);
        Destroy(this.gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if(this.size * 0.5f >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<AudioManager>().Play("Explosion");
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f; // offset the position within a circle, prevent overlaying 

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
