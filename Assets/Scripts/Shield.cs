using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool shieldAvailable;    // to determine whether the shield is available
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
        cooldown = 0;
        duration = 0;
    }
    void OnEnable() 
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable() 
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp ("space") && shieldAvailable)
        {
            m_SpriteRenderer.enabled = true;
            onShield = true;
            shieldAvailable = false;
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
}
