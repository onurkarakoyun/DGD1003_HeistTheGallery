using UnityEngine;

public class GuardAI : MonoBehaviour
{
    [Header("Devriye Ayarları")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Hedef Etiketleri")]
    public string thiefTag = "Player";
    
    [Header("Referanslar")]
    public Transform target;
    public Transform visionArea; 
    public Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool goingRight = true;
    public bool isChasing = false; 

    [Header("Ses Efektleri")]
    public AudioSource guardAudio;
    public AudioClip chaseSound;

    private float catchDistance = 1.0f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (anim == null) anim = GetComponent<Animator>();
        
        if (leftPoint != null) leftPoint.parent = null;
        if (rightPoint != null) rightPoint.parent = null;
    }

    [System.Obsolete]
    void Update()
    {
        GameObject activeThief = GameObject.FindGameObjectWithTag(thiefTag);
        if (activeThief == null || !activeThief.activeInHierarchy)
        {
            target = null;
            isChasing = false;
        }
        else
        {
            if (isChasing && target == null)
            {
                target = activeThief.transform;
            }
        }

        Vector2 moveDirection = Vector2.zero;
        if (isChasing && target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
            ChasePlayer(moveDirection);
            
            if (guardAudio != null && chaseSound != null && !guardAudio.isPlaying)
            {
                guardAudio.clip = chaseSound;
                guardAudio.Play();
            }

            // Yakalama Kontrolü
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer < catchDistance)
            {
                CatchThePlayer();
            }
        }
        else
        {
            Patrol();
            moveDirection = goingRight ? Vector2.right : Vector2.left;
            
            if (guardAudio != null && guardAudio.isPlaying)
            {
                guardAudio.Stop();
            }
        }

        // Görüş Alanı
        if (visionArea != null)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            visionArea.localRotation = Quaternion.Euler(0, 0, angle);
        }

        // Animasyon
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
            if (transform.position.x >= rightPoint.position.x) goingRight = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            sr.flipX = true;
            if (transform.position.x <= leftPoint.position.x) goingRight = true;
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
        if (collision.CompareTag(thiefTag))
        {
            target = collision.transform;
            isChasing = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(thiefTag))
        {
             if(!isChasing)
             {
                 target = collision.transform;
                 isChasing = true;
             }
        }
    }

    [System.Obsolete]
    void CatchThePlayer()
    {
        Debug.Log("YAKALANDIN!");
        if(target != null && target.CompareTag(thiefTag))
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            if (LevelManager.instance != null) LevelManager.instance.GameOver();
        }
    }
}