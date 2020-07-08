using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool shieldAvailable;    // to determine whether the shield is available
    private bool turnOn;    // whether NNet has outputted to turn on shield
    public bool onShield;      // to determine whether shield is on
    private float cooldown;     // to determine the shield cooldown period
    private float duration;     // to determine the activation period
	protected SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        shieldAvailable = true;
        onShield = false;
        turnOn = false;
        cooldown = 0;
        duration = 0;
    }
    void OnEnable() 
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        RopeSystem.TurnOnShield += TurnOnShield;
    }
    void OnDisable() 
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
        RopeSystem.TurnOnShield -= TurnOnShield;
    }
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown ("space") && shieldAvailable)
        if (turnOn && shieldAvailable)
        {
            m_SpriteRenderer.enabled = true;
            onShield = true;
            shieldAvailable = false;
            turnOn = false;
        }
        if (onShield)
        {
            duration += Time.deltaTime;
        }
        if (duration >= 3)
        {
            m_SpriteRenderer.enabled = false;
            onShield = false;
            shieldAvailable = false;
            duration = 0;
            cooldown += Time.deltaTime;
        }
        if (cooldown > 0)
        {
            cooldown += Time.deltaTime;
        }
        if (cooldown >= 5)
        {
            cooldown = 0;
            shieldAvailable = true;
        }
    }
    void OnGameOverConfirmed() // Reset the shield when game is over
    {
        m_SpriteRenderer.enabled = false;
        shieldAvailable = true;
        onShield = false;
        cooldown = 0;
        duration = 0;
    }

    void TurnOnShield()
    {
        turnOn = true;
    }
}
