using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Character;        //Public variable to store a reference to the player game object
    private float offset;            //Private variable to store the offset distance between the player and camera
    // Use this for initialization
    void Start () 
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position.x - Character.transform.position.x;
    }
    // LateUpdate is called after Update each frame
    void LateUpdate () 
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = new Vector3(Character.transform.position.x + offset, transform.position.y, transform.position.z);
    }
}
