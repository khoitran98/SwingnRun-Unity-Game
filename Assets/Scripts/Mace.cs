using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerLoseScore;
    private bool deducted;  // determine if player score has been deducted
    // Start is called before the first frame update
    void OnEnable() 
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void Start()
    {
        deducted = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D (Collision2D collision2D) // Deduct score when collides with mace
    {
        if (!deducted)
        {
            OnPlayerLoseScore();
            deducted = true;
        }
    } 
    void OnGameOverConfirmed ()
    {
        deducted = false;
    }
}
