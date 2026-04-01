using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum STATE { IDLE, CHASE, ATTACK };
public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    private Transform player;
    public float minDistance = 1f;
    public float maxDistance = 5f;
    public float distance;
    private STATE state = STATE.IDLE;
    private Rigidbody2D rb2D;

   

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);

        ChangeState();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage");
        health -= damage;
        if (health <= 0) Destroy(gameObject);
       
    }
    void HandleState()
    {
        switch (state)
        {
            case STATE.IDLE:
                Debug.Log("just chillin");
                break;
            case STATE.CHASE:
                Chase();
                break;
            case STATE.ATTACK:
                Debug.Log("Attacking");
                break;
        }
    }

    void ChangeState()
    {
        if (distance <= minDistance)
        {
            state = STATE.ATTACK;
        }
        else if (distance <= maxDistance)
        {
            state = STATE.CHASE;
        }
        else
        {
            state = STATE.IDLE;
        }
        HandleState();
    }
    void Chase()
    {
        rb2D.MovePosition(Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        ));
    }
}
