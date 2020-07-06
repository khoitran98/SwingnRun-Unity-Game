using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D (Collision2D collision2D)
		{
			//OnPlayerDied();
		} 
    
}
