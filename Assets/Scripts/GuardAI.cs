using UnityEngine;

public class GuardAI : MonoBehaviour
{
    [Header("Devriye Ayarları")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Referanslar")]
    public Transform player;
    public Transform visionArea; 
    public Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool goingRight = true;
    
    public bool isChasing = false; 
    [Header("Ses Efektleri")]
    public AudioSource guardAudio;
    public AudioClip chaseSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (anim == null)
            anim = GetComponent<Animator>();

        if (leftPoint != null) leftPoint.parent = null;
        if (rightPoint != null) rightPoint.parent = null;
    }

    void Update()
    {
        if (player == null || !player.gameObject.activeInHierarchy || !player.CompareTag("Player"))
        {
            isChasing = false;
        }

        Vector2 moveDirection = Vector2.zero;

        if (isChasing)
        {
            moveDirection = (player.position - transform.position).normalized;
            ChasePlayer(moveDirection);
            if (!guardAudio.isPlaying)
            {
            guardAudio.clip = chaseSound;
            guardAudio.Play();
            }
        }
        else
        {
            Patrol();
            moveDirection = goingRight ? Vector2.right : Vector2.left;
            if (guardAudio.isPlaying)
            {
            guardAudio.Stop();
            }
        }

        if (visionArea != null)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            visionArea.localRotation = Quaternion.Euler(0, 0, angle);
        }

        if (anim != null)
        {
            anim.SetBool("isChasing", isChasing);
        }
    }

    void Patrol()
    {
        if (leftPoint == null || rightPoint == null) return;

        float speed = patrolSpeed;

        if (goingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            sr.flipX = false;

            if (transform.position.x >= rightPoint.position.x)
                goingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            sr.flipX = true;

            if (transform.position.x <= leftPoint.position.x)
                goingRight = true;
        }
    }

    void ChasePlayer(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
        
        if (direction.x > 0) sr.flipX = false;
        else if (direction.x < 0) sr.flipX = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            isChasing = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (LevelManager.instance != null)
                LevelManager.instance.GameOver();
        }
    }
}