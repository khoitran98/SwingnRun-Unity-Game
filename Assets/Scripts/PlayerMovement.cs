
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 0f;
    public bool isSwinging;
    public bool groundCheck;
    public bool groundCheck1;
    public bool groundCheck2;
    public bool groundCheck3;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private Animator animator;
    private float horizontalInput;
    public bool groundPull;
    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void OnEnable() 
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted ()
    {
        speed = 100f; // set the running speed when the game starts
        animator.SetBool("gameStarted", true);
        animator.SetBool("restart", false);
    }
    void OnGameOverConfirmed ()
    {
        speed = 0f; // prevent running when restart game
        animator.SetBool("gameStarted", false);
        animator.SetBool("restart", true);
        Time.timeScale = 1; //unpause game
    }
    void Update()
    {
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f); // a raycast to ignore player own collider box
        groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), new Vector2(1f,-1f), 0.5f); // second raycast to check ground to the player's right
        groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), new Vector2(-1f,-1f), 0.5f); // third raycast to check ground arround player's left
        groundCheck = groundCheck1 || (groundCheck2 || groundCheck3);
        if (isSwinging) 
        {
            animator.SetBool("isSwinging", true); 
            animator.SetBool("isGrounded", false);
            animator.SetBool("isFalling", false);
        }
            else if (groundCheck)
            {
                animator.SetBool("isSwinging", false);
                animator.SetBool("isGrounded", true);
                animator.SetBool("isFalling", false);
                rBody.velocity = new Vector2(speed * 0.03f, rBody.velocity.y);
            }
            else {
                animator.SetBool("isFalling", true);
                animator.SetBool("isGrounded", false);
                animator.SetBool("isSwinging", false);
            }
    }
}
