using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestructionPoint;
    // Start is called before the first frame update
    void Start()
    {
        platformDestructionPoint = GameObject.Find ("PlatformDestructionPoint"); // no need to include destruction point in the inspector
    }
    void OnEnable() 
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameOverConfirmed() // reset the platform when restart game
    {
        gameObject.SetActive(false);
    } 
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < platformDestructionPoint.transform.position.x)
        { 
            //gameObject.SetActive(false); // deavtivate offscreen object
            // Platform destroyer causes a coin bug so currently commented out
        }

    }
}
