using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public Sprite[] spritesDown;  
    public Sprite[] spritesUp;
    public Sprite[] spritesLeft;
    public Sprite[] spritesRight;

    private SpriteRenderer sr;
    private float animTimer = 0f;
    private float animSpeed = 0.15f; 
    private int currentFrame = 0;
    private Sprite[] lastSprites;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 2f;
    public int attackDamage = 1;
    public KeyCode attackKey = KeyCode.Space;

    Vector2 movement;                                        
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

     
        
        if (Mathf.Abs(x) >= Mathf.Abs(y))
            movement = new Vector2(x, 0);
        else
            movement = new Vector2(0, y);
        if (movement.magnitude > 0.1f)
        {
            rb.MovePosition((Vector2)transform.position + movement.normalized * speed * Time.deltaTime);

            Sprite[] currentSprites;
            if (Mathf.Abs(x) >= Mathf.Abs(y))
                currentSprites = x > 0 ? spritesRight : spritesLeft;
            else
                currentSprites = y > 0 ? spritesUp : spritesDown;

            lastSprites = currentSprites;

            animTimer += Time.deltaTime;
            if (animTimer >= animSpeed)
            {
                animTimer = 0f;
                currentFrame = (currentFrame + 1) % currentSprites.Length;
            }
            sr.sprite = currentSprites[currentFrame];
        }
        else
        {
          
            currentFrame = 0;
            animTimer = 0f;
            if (lastSprites != null)
                sr.sprite = lastSprites[0];
        }

        if (Input.GetKeyDown(attackKey))
            Attack();
    }

    void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRange
        );
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                hit.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        Debug.Log("Ataque");
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0.1f)
        {
            rb.MovePosition((Vector2)transform.position + movement.normalized * speed * Time.deltaTime);
        }
    }

}


