using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(HnS_MainCharacter))]


	public class HnS_PlayerControls : MonoBehaviour 
	{
		bool m_Jump;

		private HnS_MainCharacter m_Character;


		// Use this for initialization
		void Start () {
			// get the third person character ( this should never be null due to require component )
			m_Character = GetComponent<HnS_MainCharacter>();
	
		}
		
		private void Update()
		{
			if (!m_Jump)
			{
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			}
		}

		private void FixedUpdate()
		{
			// read inputs
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");

			// pass all parameters to the character control script
			m_Character.move(h, v, m_Jump);
			m_Jump = false;
		}
	}
}
