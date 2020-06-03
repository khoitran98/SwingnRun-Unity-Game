using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerLoseScore;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("master");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D (Collision2D collision2D) // Deduct score when collides with mace
		{
			OnPlayerLoseScore();
		} 
}
