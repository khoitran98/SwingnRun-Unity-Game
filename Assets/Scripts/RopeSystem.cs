
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    public LineRenderer ropeRenderer; 
    public LayerMask ropeLayerMask; // which physiscs layers the grappling hook raycast can hit
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    // public Transform crosshair;
    // public SpriteRenderer crosshairSprite;
    public PlayerMovement playerMovement;
    private bool ropeAttached; // when rope hits
    private Vector2 playerPosition;
    private List<Vector2> ropePositions = new List<Vector2>(); // list to store the hitting point of the rope
    private float ropeMaxCastDistance = 15f;
    private Rigidbody2D ropeHingeAnchorRb;
    private Rigidbody2D character;
    private bool distanceSet; // whether rope distance has been set correctly
    private SpriteRenderer ropeHingeAnchorSprite;
    private float counter;
    private float aimAngle;
    private bool gameStarted;
    void Awake ()
    {   
        ropeJoint.enabled = false;
	    playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        character = GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }
    void OnEnable() 
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.Dead += Dead;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.Dead -= Dead;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted() 
    {
        gameStarted = true;
    }
    void OnGameOverConfirmed () 
    {
        gameStarted = false;
    }   // Update is called once per frame
    void Dead()
    {
        gameStarted = false;
        ResetRope();
    }
    void Update ()
	{  
        if (counter >= ropeJoint.distance && !distanceSet && ropeAttached) //update the rope distance instantly after the rope line is rendered to prevent rope pulling back by distance joint
        { 
            ropeJoint.distance = Vector2.Distance(transform.position, ropePositions[0]);
            if (playerMovement.groundPull)
                character.velocity = new Vector2(6f, character.velocity.y);
            else
                character.velocity = new Vector2(character.velocity.x * 1.5f, character.velocity.y);
            distanceSet = true;      
        }
        playerPosition = transform.position;
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        if (!ropeAttached)
        {
            playerMovement.isSwinging = false;
	    }
	    else 
        {
            playerMovement.isSwinging = true;
        }
        UpdateRopePositions();
        HandleInput(aimDirection);  
    }
    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {  
            if (aimAngle < .52 || aimAngle > 1.4) return; // setting the range for aiming angle
            if (ropeAttached) return; // Prevent creating a new rope when already swinging
            if (!gameStarted) return; // Preventing creating rope when game hasn't started
            ropeRenderer.enabled = true;
            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);
            if (hit.collider != null) // if the rope hits a valid target
            {
                ropeAttached = true;
                if (!ropePositions.Contains(hit.point))
                {
                    if (!playerMovement.groundCheck)
                    {
                        character.velocity = new Vector2(character.velocity.x, character.velocity.y * 0.25f);  // speed up the swing and slowdown freefall to prevent the rope from stretching too much
                        ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    }
                    else 
                    {
                        ropeJoint.distance = Vector2.Distance(playerPosition, hit.point) - 0.1f; // lift the character from the ground to swing
                        playerMovement.groundPull = true;
                    }
                    ropePositions.Add(hit.point);
                    ropeJoint.enabled = true;
                    ropeHingeAnchorSprite.enabled = true;
                    ropeHingeAnchorRb.transform.position = hit.point;
                }
            }
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;  
            }
        }
        if (Input.GetMouseButton(1))
        {
            ResetRope(); //  right click to disable the rope
        }
    }
    private void ResetRope() // reset parameter
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerMovement.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        distanceSet = false;
        playerMovement.groundPull = false;
        counter = 0f;
    }
    private void UpdateRopePositions()
    {
        if (distanceSet) 
        { // reset rope when swing to maximum distance
            var dist = Vector2.Distance(character.velocity, Vector2.zero);
            if ( dist >= 0 && dist <= 0.28) 
            { 
                 ResetRope();
            }  
        }
        if (!distanceSet && ropeAttached) // slowly render the rope
        { 
            if (playerMovement.groundPull)
            {
                if (counter < ropeJoint.distance) // render rope differently when pull from ground
                { 
                    counter += .2f; // animating speed
                    float x = Mathf.Lerp(0, Vector2.Distance(playerPosition, ropePositions[0]), counter); // prevent short rope bug
                    Vector3 pointA = transform.position;
                    Vector3 pointB = ropePositions[0];
                    Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
                    ropeRenderer.SetPosition(1, transform.position);
                    ropeRenderer.SetPosition(0, pointAlongLine);
                }
            }
            else
            {
                if (counter < Vector2.Distance(playerPosition, ropePositions[0])) // prevent short rope bug
                { 
                    counter += .2f; // animating speed
                    float x = Mathf.Lerp(0, Vector2.Distance(playerPosition, ropePositions[0]), counter); // prevent short rope bug
                    Vector3 pointA = transform.position;
                    Vector3 pointB = ropePositions[0];
                    Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
                    ropeRenderer.SetPosition(1, transform.position);
                    ropeRenderer.SetPosition(0, pointAlongLine);
                }
            }
        }
        else {
            if (ropeAttached) // constanly updating player position and anchor for swinging
            {     
                ropeRenderer.SetPosition(1, playerPosition);
                ropeHingeAnchorRb.transform.position = ropePositions[0];
            } 
        }
       
    }
}
