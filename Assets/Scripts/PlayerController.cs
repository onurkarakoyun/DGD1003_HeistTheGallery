using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private Rigidbody2D rb; 
    private SpriteRenderer sr;
    private Animator anim;
    float moveX;
    bool isGrounded;
    public bool isHidden = false;
    [Header("Ses Efektleri")]
    public AudioSource audioSource;
    public AudioClip runSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (isHidden) 
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0);
            return;
        }
        moveX = Input.GetAxisRaw("Horizontal");
        if (moveX > 0)
        {
            sr.flipX = false;
        }
        else if (moveX < 0)
        {
            sr.flipX = true;
        }
        anim.SetFloat("Speed", Mathf.Abs(moveX));
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }
        HandleFootsteps();
    }
    void FixedUpdate()
    {
        if (isHidden) return;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }
    void HandleFootsteps()
{
    // Yerdeysek VE hareket ediyorsak (Hız 0.1'den büyükse)
    if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
    {
        // Eğer ses zaten çalmıyorsa başlat
        if (!audioSource.isPlaying)
        {
            audioSource.clip = runSound;
            audioSource.Play();
        }
    }
    else
    {
        // Durduysak veya zıpladıysak sesi kes
        audioSource.Stop();
    }
}
}
