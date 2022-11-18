using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine
{
	public enum StateType { Idle, Walk, Run, Jump, Crouch }
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Grounded")] 
		[SerializeField] private float _gravity = -30f;
		[SerializeField] private float _groundedGravity = -0.05f;
		[SerializeField] private float _isGroundedRadius = 0.08f;
		[SerializeField] private LayerMask _groundMask;
		[Header("Jump")] 
		[SerializeField] private float _jumpHeight = 1.5f;
		[Header("Walk")] 
		[SerializeField] private float _walkSpeed = 3f;
		[Header("Run")] 
		[SerializeField] private float _runSpeed = 6f;
		[Header("Crouch")] 
		[SerializeField] private float _crouchSpeed = 1.5f;
		
		// States
		private readonly Dictionary<StateType, StateBase> states = new();
		private StateBase currentState;

		private CharacterController _cc;
		private Vector3 _horizontalVel;
		private Vector3 _verticalVel;

		public bool IsGrounded { get; private set; }

		public float VerticalVel
		{
			get => _verticalVel.y;
			set => _verticalVel.y = value;
		}

		public void SetHorizontalVelocity(Vector2 vel)
		{
			_horizontalVel.x = vel.x;
			_horizontalVel.z = vel.y;
		}

		// Params
		public float Gravity => _gravity;
		public float GroundedGravity => _groundedGravity;
		public float JumpHeight => _jumpHeight;
		public float WalkSpeed => _walkSpeed;
		public float RunSpeed => _runSpeed;
		public float CrouchSpeed => _crouchSpeed;

		// Input
		public bool RunRequested { get; set; }
		public bool Running { get; set; }
		public bool JumpRequested { get; set; }
		public bool CrouchRequested { get; set; }
		public bool Crouching { get; set; }
		public bool LeanRight { get; private set; }
		public bool LeanLeft { get; private set; }

		private void Awake()
		{
			_cc = GetComponent<CharacterController>();
			
			states.Add(StateType.Jump, new State_Jump(this));
			states.Add(StateType.Crouch, new State_Crouch(this));
			states.Add(StateType.Walk, new State_Walk(this));
			states.Add(StateType.Run, new State_Run(this));
			states.Add(StateType.Idle, new State_Idle(this));
			
			currentState = states[StateType.Idle];
			currentState.OnEnter();
		}

		private void Start()
		{
			InputManager.Instance.Run += OnRun;
			InputManager.Instance.Crouch += OnCrouch;
			InputManager.Instance.Jump += OnJump;
			InputManager.Instance.LeanRight += OnLeanRight;
			InputManager.Instance.LeanLeft += OnLeanLeft;
		}

		private void Update()
		{
			RefreshGrounded();
			
			if (!currentState.OnUpdate())
				SwitchState();
				
			var horizontalVel = _horizontalVel.x * transform.right + _horizontalVel.z * transform.forward;
			_cc.Move((horizontalVel + _verticalVel) * Time.deltaTime);
		}

		#region Input

		private void OnJump()
		{
			if (!IsGrounded) return;
			
			JumpRequested = true;
			SwitchState();
			JumpRequested = false;
		}

		private void OnCrouch(bool pressed, bool isToggle)
		{
			Debug.Log("A");
			if (!IsGrounded) return;
			Debug.Log("B");
			
			if (!isToggle)
			{
				Debug.Log($"Crouching: {pressed}");
				Crouching = pressed;
				CrouchRequested = pressed;
				SwitchState();
				CrouchRequested = false;
			}
			else if (pressed)
			{
				Debug.Log("C");
				Crouching = !Crouching;
				CrouchRequested = Crouching;
				SwitchState();
				CrouchRequested = false;
			}
		}

		private void OnRun(bool pressed, bool isToggle)
		{
			if (!IsGrounded) return;
			
			if (!isToggle)
			{
				Running = pressed;
				RunRequested = pressed;
				SwitchState();
				RunRequested = false;
			}
			else if (pressed)
			{
				Running = !Running;
				RunRequested = Running;
				SwitchState();
				RunRequested = false;
			}
		}

		private void OnLeanRight(bool lean)
		{
			LeanRight = lean;
		}
		
		private void OnLeanLeft(bool lean)
		{
			LeanLeft = lean;
		}

		#endregion
		
		private void SwitchState()
		{
			foreach (var (_, state) in states)
			{
				if (currentState == state)
					continue;

				if (state.CanEnter())
				{
					currentState = state;
					currentState.OnEnter();
					return;
				}
			}
		}

		private void RefreshGrounded()
		{
			IsGrounded = Physics.CheckSphere(transform.position, _isGroundedRadius, _groundMask);
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, _isGroundedRadius);
		}
	}
}