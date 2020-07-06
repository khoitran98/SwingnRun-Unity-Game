﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class Saw : MonoBehaviour
	{
		public delegate void PlayerDelegate();
		public static event PlayerDelegate OnPlayerDied;
		private Transform targetRotation;
		private float m_Speed = 3f;
		private bool m_RotateClockwise = false;
		private bool onShield;
		void Start ()
		{
			if (targetRotation == null) {
				targetRotation = transform;
			}
			onShield = false;
		}
		void Update ()
		{
			onShield =  GameObject.Find("bubbleShield").GetComponent<Shield>().onShield;
			Vector3 rotation = targetRotation.rotation.eulerAngles;
			if (!m_RotateClockwise) {
				rotation.z += m_Speed;
			} else {
				rotation.z -= m_Speed;
			}
			targetRotation.rotation = Quaternion.Euler (rotation);
		}
		void OnCollisionEnter2D (Collision2D collision2D) // end the game if collides with saw
		{
			if (!onShield)
				OnPlayerDied();
		} 
	}

