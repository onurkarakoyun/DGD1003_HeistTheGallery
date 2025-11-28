using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform player;
    private Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool goingRight = true;
    private bool isChasing = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
        anim.SetBool("isChasing", isChasing);
    }
    void Patrol()
    {
        if (goingRight)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, rb.linearVelocity.y);
            sr.flipX = false;

            if (transform.position.x >= rightPoint.position.x)
                goingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, rb.linearVelocity.y);
            sr.flipX = true;

            if (transform.position.x <= leftPoint.position.x)
                goingRight = true;
        }
    }
    void ChasePlayer()
    {
        float direction = player.position.x - transform.position.x;

        if (direction > 0)
        {
            rb.linearVelocity = new Vector2(chaseSpeed, rb.linearVelocity.y);
            sr.flipX = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-chaseSpeed, rb.linearVelocity.y);
            sr.flipX = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Güvenlik seni gördü! KOŞUYOR!");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("YAKALANDIN!");
        }
    }
}
