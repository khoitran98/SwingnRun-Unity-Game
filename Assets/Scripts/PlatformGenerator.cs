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
    int[] selector;
    int index;      // selector indexes
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        selector = new int[]  { 0, 2, 5, 1, 4, 3, 6, 2, 3, 4, 0, 1, 6, 5}; // platforms order appearance
        index = 0;
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
        index = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < generationPoint.position.x && index < selector.Length)
        {
            //platformSelector = Random.Range (0, theObjectPools.Length);      // make platforms appear random
            platformSelector = selector[index];
            index++;
            transform.position = new Vector3(transform.position.x + 18f, transform.position.y, transform.position.z);
            GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();
            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive (true);
        }
    }
}
