using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public Vector3 startPos;
    Rigidbody2D rigidbody;
    private bool onShield;
    private float hitTimer;
    private bool hasCollide;
    void Start () 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        hasCollide = false;
        hitTimer = 0;
    }
    void OnEnable() 
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        //rigidbody.simulated = true;
    }
    void OnGameOverConfirmed() // reset the player position and velocity when restart
    {
        rigidbody.velocity = Vector2.zero; 
        transform.position = startPos;
    }
    void Update () 
    {
        if (transform.position.y < - 8) // end game when player falls offscreen
        { 
            OnPlayerDied();
        }
        onShield =  GameObject.Find("bubbleShield").GetComponent<Shield>().onShield;
        if (hasCollide)
        {
            hitTimer += Time.deltaTime;
        }
        if (hitTimer > 1)
        {
            hasCollide = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision) // End game when character collides with water or saw
    {
        if (collision.gameObject.name == "Water" || collision.gameObject.name == "Saw")
        {
            if (!onShield && !hasCollide)
            {
                OnPlayerDied();
                hasCollide = true;
            }
        }
    }
}
