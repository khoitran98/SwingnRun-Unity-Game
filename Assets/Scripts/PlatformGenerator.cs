using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
 //  public GameObject thePlatform;
    public Transform generationPoint;
    private float[] platformWidth;
    private int platformSelector;
    public ObjectPooler[] theObjectPools;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }
    void OnEnable() 
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameOverConfirmed() 
    {
        transform.position = startPos; // reset the generator position after restart
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x)
        {
            platformSelector = Random.Range (0, theObjectPools.Length);
            transform.position = new Vector3(transform.position.x + 18f, transform.position.y, transform.position.z);
            GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive (true);
        }
    }
}
