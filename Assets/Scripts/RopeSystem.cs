
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
    public GameObject cam; // testing neural net, adding camera object
    private float sensorA, sensorB, sensorC; // sensors from GameManager script
    public NNet network; //testing neural net
    public float leftMouse;
    public float rightMouse;
    public float resetRope;
    public float setRope;
    public float cheat;
    public float nothing;
    public float shootAngle;
    public float[] outputs;
    public float isRopeAttached;
    [Header("Network Options")]
    public int LAYERS = 1;
    public int NEURONS = 10;
    public float overallFitness;
    void Awake ()
    {   
        ropeJoint.enabled = false;
	    playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        character = GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        network = GetComponent<NNet>();   
        leftMouse = rightMouse = 0;
        ropeAttached = false;
        outputs = new float[5];
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
        GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    }
    void Update ()
	{  
        Vector2 center = new Vector2(transform.position.x + 5f, 0);
        float radius = 5f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius); // Gather obstacles' distances to character as inputs to NNet
        // Debug.Log(hitColliders.Length);
        foreach (Collider2D col in hitColliders)
        {
            if (col.gameObject.name == "Ceiling (1)" || col.gameObject.name == "Character" || col.gameObject.name == "Ceiling (3)" || col.gameObject.name == "Ceiling (2)")
                continue;
            // Debug.Log(col.gameObject.name);
            // Debug.Log("X distance: " + (col.gameObject.transform.position.x - transform.position.x)/10);
            // Debug.Log("Y distance: " + (col.gameObject.transform.position.y - transform.position.y)/10);        
        }

        overallFitness = cam.GetComponent<GameManager>().overallFitness;
        sensorA = cam.GetComponent<GameManager>().aSensor;
        sensorB = cam.GetComponent<GameManager>().bSensor;
        sensorC = cam.GetComponent<GameManager>().cSensor;
        if (!ropeAttached)
        {
            isRopeAttached = 0;
	    }
	    else 
        {
            isRopeAttached = 1;
        }
        // (leftMouse, rightMouse) = network.RunNetwork(sensorA, sensorB, sensorC, ropeAttached); // neural net
        (setRope, resetRope, nothing, cheat, shootAngle) = network.RunNetwork(sensorA, sensorB, sensorC, ropeAttached); // neural net
        // Makes 4 possible control options and the rope shoot angle to be outputs of neural network
        outputs[0] = setRope; // option to shoot the rope
        outputs[1] = resetRope; // option to reset the rope
        outputs[2] = nothing; // option to do nothing
        outputs[3] = cheat; // option to render character unkillable
        outputs[4] = shootAngle; // angle to shoot rope
        float temp = 0; // temp variable for comparison
        int index = 0; // variable to store the index of highest output
        for (int i = 0; i < 3; i++) // compare outputs to select the highest output
        {
            if (outputs[i] > temp)
            {   
                temp = outputs[i];
                index = i;
            }
        }
        // switch (index)
        // {
        //     case 0:
        //         Debug.Log("setRope");
        //         break;
        //     case 1:
        //         Debug.Log("resetRope");
        //         break;
        //     case 2:
        //         Debug.Log("nothing");
        //         break;
        //     case 3:
        //         Debug.Log("cheat");
        //         break;  
        //     default:
        //         break;
        // }
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
        // or 0.88f???
        // aimAngle = (0.52f + 0.88f * leftMouse) * Mathf.Rad2Deg; // neural net
        // var aimDirection = Quaternion.Euler(0, 0, aimAngle) * Vector2.right; // neural net
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
        
       // if (!ropeAttached) // testing neural net
        {  
            //Debug.Log(aimAngle);
            if (aimAngle < .52 || aimAngle > 1.4) return; // setting the range for aiming angle
            //if (aimAngle < 30 || aimAngle > 80) return; // setting the range for aiming angle, neural net
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
                    //Debug.Log("ropeattached");
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
       // if (rightMouse >= 0.5) neural net
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
    public void ResetWithNetwork (NNet net)
    {
        network = net;
    }
}
