using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class Coin : MonoBehaviour
	{
		public delegate void PlayerDelegate();
		public static event PlayerDelegate OnPlayerScored;
		protected SpriteRenderer m_SpriteRenderer;
		protected BoxCollider2D m_Collider2D;
		void Start() 
		{
			m_SpriteRenderer = GetComponent<SpriteRenderer>();
			m_Collider2D = GetComponent<BoxCollider2D>();
		}
		void OnEnable() 
		{
        	GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    	}
    	void OnDisable() 
		{
        	GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    	}
		void OnGameOverConfirmed() // rerender the coin after restart
		{
			m_SpriteRenderer.enabled = true;
			m_Collider2D.enabled = true;
		}
		void OnCollisionEnter2D (Collision2D collision2D) // allow player to walk through the coin after collecting
		{
			OnPlayerScored();
			m_SpriteRenderer.enabled = false;
			m_Collider2D.enabled = false;
		}
	}
