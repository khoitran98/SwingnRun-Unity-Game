
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class RopeSystem : MonoBehaviour
{
    public delegate void PlayerDelegate();
	public static event PlayerDelegate TurnOnShield;
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
    public NNet network; //testing neural net
    public float leftMouse;
    public float rightMouse;
    public float resetRope;
    public float cheat;
    public float nothing;
    public float shootAngle;
    public float[] outputs;
    public float isRopeAttached;
    [Header("Network Options")]
    public int LAYERS = 1;
    public int NEURONS = 10;
    public float overallFitness;
    public float mace1X, mace1Y, mace2X, mace2Y, mace3X, mace3Y, saw1X, saw1Y, saw2X, saw2Y, saw3X, saw3Y, water1X, water1Y, water2X, water2Y, water3X, water3Y, tile1X, tile1Y, tile2X, tile2Y, longTile1X, longTile1Y, coin1X, coin1Y, coin2X, coin2Y;    // variables to hold distances from player to obstacles as inputs
    public List<float> mace, saw, water, coin, tile, longTile;
    public List<float> correctOutputs; 
    private int index; 
    void Awake ()
    {   
        ropeJoint.enabled = false;
	    playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        character = GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        network = GetComponent<NNet>();
        network.InitialiseAndLoad();
        // network.loadNetwork();   
        leftMouse = rightMouse = 0;
        ropeAttached = false;
        outputs = new float[5];
        // LoadGameplay();
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
        //ResetRope();
        // SaveGameplay(); // saving personal sequence
        // GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    }
    void Update ()
	{  
        Vector2 center = new Vector2(transform.position.x + 5f, 0);
        float radius = 5f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius); // Gather obstacles' distances to character as inputs to NNet
        // Debug.Log(hitColliders.Length);
        mace.Clear();
        water.Clear();
        saw.Clear();
        tile.Clear();
        longTile.Clear();
        coin.Clear();
        foreach (Collider2D col in hitColliders)
        {
            if (col.gameObject.name == "Ceiling (1)" || col.gameObject.name == "Character" || col.gameObject.name == "Ceiling (3)" || col.gameObject.name == "Ceiling (2)")
                continue;
            if (col.gameObject.name == "Mace")
            {
                mace.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                mace.Add((col.gameObject.transform.position.y - transform.position.y)/10);
            }
            else if (col.gameObject.name == "Saw")
            {
                saw.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                saw.Add((col.gameObject.transform.position.y - transform.position.y)/10);
            }
            else if (col.gameObject.name == "Water")
            {
                water.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                water.Add((col.gameObject.transform.position.y - transform.position.y)/10);
                
            }
            else if (col.gameObject.name == "Tile")
            {
                tile.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                tile.Add((col.gameObject.transform.position.y - transform.position.y)/10);
            }
            else if (col.gameObject.name == "LongTile")
            {
                longTile.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                longTile.Add((col.gameObject.transform.position.y - transform.position.y)/10);
            }
            else if (col.gameObject.name == "Coin")
            {
                coin.Add((col.gameObject.transform.position.x - transform.position.x)/10);
                coin.Add((col.gameObject.transform.position.y - transform.position.y)/10);
            }
        }
        switch (mace.Count)
        {
            case 0:
                mace1X = mace1Y = mace2X = mace2Y = mace3X = mace3Y = 0;
                break;
            case 2:
                mace2X = mace2Y = mace3X = mace3Y = 0;
                mace1X = mace[0];
                mace1Y = mace[1];
                break;
            case 4:
                mace3X = mace3Y = 0;
                mace1X = mace[0];
                mace1Y = mace[1];
                mace2X = mace[2];
                mace2Y = mace[3];
                break;
            default:
                mace1X = mace[0];
                mace1Y = mace[1];
                mace2X = mace[2];
                mace2Y = mace[3];
                mace3X = mace[4];
                mace3Y = mace[5];
                break;
        }
        switch (saw.Count)
        {
            case 0:
                saw1X = saw1Y = saw2X = saw2Y = saw3X = saw3Y = 0;
                break;
            case 2:
                saw2X = saw2Y = saw3X = saw3Y = 0;
                saw1X = saw[0];
                saw1Y = saw[1];
                break;
            case 4:
                saw3X = saw3Y = 0;
                saw1X = saw[0];
                saw1Y = saw[1];
                saw2X = saw[2];
                saw2Y = saw[3];
                break;
            default:
                saw1X = saw[0];
                saw1Y = saw[1];
                saw2X = saw[2];
                saw2Y = saw[3];
                saw3X = saw[4];
                saw3Y = saw[5];
                break;
        }
        switch (water.Count)
        {
            case 0:
                water1X = water1Y = water2X = water2Y = water3X = water3Y = 0;
                break;
            case 2:
                water2X = water2Y = water3X = water3Y = 0;
                water1X = water[0];
                water1Y = water[1];
                break;
            case 4:
                water3X = water3Y = 0;
                water1X = water[0];
                water1Y = water[1];
                water2X = water[2];
                water2Y = water[3];
                break;
            default:
                water1X = water[0];
                water1Y = water[1];
                water2X = water[2];
                water2Y = water[3];
                water3X = water[4];
                water3Y = water[5];
                break;
        }
        switch (tile.Count)
        {
            case 0:
                tile1X = tile1Y = tile2X = tile2Y = 0;
                break;
            case 2:
                tile2X = tile2Y = 0;
                tile1X = tile[0];
                tile1Y = tile[1];
                break;
            default:
                tile1X = tile[0];
                tile1Y = tile[1];
                tile2X = tile[2];
                tile2Y = tile[3];
                break;
        }
        switch (longTile.Count)
        {
            case 0:
                longTile1X = longTile1Y = 0;
                break;
            default:
                longTile1X = longTile[0];
                longTile1Y = longTile[1];
                break;
        }
        switch (coin.Count)
        {
            case 0:
                coin1X = coin1Y = coin2X = coin2Y = 0;
                break;
            case 2:
                coin2X = coin2Y = 0;
                coin1X = coin[0];
                coin1Y = coin[1];
                break;
            default:
                coin1X = coin[0];
                coin1Y = coin[1];
                coin2X = coin[2];
                coin2Y = coin[3];
                break;
        }
        overallFitness = cam.GetComponent<GameManager>().overallFitness;
        if (!ropeAttached)
        {
            isRopeAttached = 0;
	    }
	    else 
        {
            isRopeAttached = 1;
        }
        (shootAngle, resetRope, nothing, cheat) = network.RunNetwork(mace1X, mace1Y, mace2X, mace2Y, mace3X, mace3Y, saw1X, saw1Y, saw2X, saw2Y, saw3X, saw3Y, water1X, water1Y, water2X, water2Y, water3X, water3Y, tile1X, tile1Y, tile2X, tile2Y, longTile1X, longTile1Y, coin1X, coin1Y, coin2X, coin2Y); // neural net
        // Makes 3 possible control options and the rope shoot angle to be outputs of neural network
        outputs[0] = shootAngle; // option to shoot the rope with shoot angle
        outputs[1] = resetRope; // option to reset the rope
        outputs[2] = nothing; // option to do nothing
        outputs[3] = cheat; // option to render character unkillable
        float temp = 0; // temp variable for comparison
        index = 0; // variable to store the index of highest output
        for (int i = 0; i < 4; i++) // compare outputs to select the highest output
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
        //         //Debug.Log("setRope");
        //         break;
        //     case 1:
        //         //Debug.Log("resetRope");
        //       //  ResetRope();
        //         break;
        //     case 2:
        //         //Debug.Log("nothing");
        //         break;
        //     case 3:
        //         //Debug.Log("cheat");
        //         //TurnOnShield();
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
        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x); // non neural
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right; // normal non neural
        // or 0.88f???
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
        //if (index == 0)
       // if (!ropeAttached) // testing neural net
        {  
            // if (index != 0)
            //     return;
            //aimAngle = (0.52f + 0.88f * outputs[index]) * Mathf.Rad2Deg; // neural net
            //aimDirection = Quaternion.Euler(0, 0, aimAngle) * Vector2.right; // neural net
            if (aimAngle < .52 || aimAngle > 1.4) return; // setting the range for aiming angle, non neural net

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
            //float aimOutput = (aimAngle - 0.52f)/0.88f;
            //float[] correctOutput = {aimOutput, 0f, 0f, 0f};
            //network.Train(correctOutput, frame);
           // correctOutputs.Add(aimOutput);
        }

        else if (Input.GetMouseButton(1)) // non neural net
        //else if (index == 1) // neural net
        {

            ResetRope(); //  right click to disable the rope
            //float[] correctOutput = {0f, 1f, 0f, 0f};
            //correctOutputs.Add(1f);
            //network.Train(correctOutput, frame);
        }
        else if (Input.GetKeyDown ("space"))
        //else if (index == 3) // neural net
        {
            TurnOnShield();
            //float[] correctOutput = {0f, 0f, 0f, 1f};
            //correctOutputs.Add(3f);
            //network.Train(correctOutput, frame);
        }
        else
        {
            //float[] correctOutput = {0f, 0f, 1f, 0f};
            //correctOutputs.Add(2f);
            //network.Train(correctOutput, frame);
        }
    }
    public void ResetRope() // reset parameter
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
    // public void SaveGameplay()
    // {
    //      System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\gameplay.txt",string.Empty);
    //     using (System.IO.StreamWriter file =
    //         new System.IO.StreamWriter(@"C:\Users\Khoi Tran\gameplay.txt", true))
    //     {
    //         for (int i = 0; i < correctOutputs.Count; i++)
    //         {
    //             file.WriteLine(correctOutputs[i]);
    //         }
    //     }
    // }
    // public void LoadGameplay()
    // {
    //      using (System.IO.StreamReader file =
    //         new System.IO.StreamReader(@"C:\Users\Khoi Tran\gameplay.txt", true))
    //     {
    //         while(file.Peek() >= 0)
    //         {
    //             correctOutputs.Add(float.Parse(file.ReadLine()));
    //         }
    //     }
    //     System.IO.File.WriteAllText(@"C:\Users\Khoi Tran\gameplay.txt",string.Empty);
    // }
}
