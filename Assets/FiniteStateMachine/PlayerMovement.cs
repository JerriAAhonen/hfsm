using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine
{
	public enum StateType { Idle, Walk, Run }
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Grounded")] 
		[SerializeField] private float _gravity = -30f;
		[SerializeField] private float _groundedGravity = -0.05f;
		[Header("Jump")] 
		[SerializeField] private float _jumpHeight = 1.5f;
		[Header("Walk")] 
		[SerializeField] private float _walkSpeed = 3f;
		[Header("Run")] 
		[SerializeField] private float _runSpeed = 6f;
		
		// States
		private readonly Dictionary<StateType, StateBase> states = new();
		private StateBase currentState;

		private CharacterController _cc;
		private Vector3 _horizontalVel;
		private Vector3 _verticalVel;

		public bool IsGrounded => _cc.isGrounded;

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

		// Input
		public bool RunRequested { get; set; }
		public bool JumpRequested { get; set; }
		public bool CrouchRequested { get; set; }

		private void Awake()
		{
			_cc = GetComponent<CharacterController>();
			
			states.Add(StateType.Walk, new WalkState(this));
			states.Add(StateType.Run, new RunState(this));
			states.Add(StateType.Idle, new IdleState(this));
			
			currentState = states[StateType.Idle];
			currentState.OnEnter();
		}

		private void Update()
		{
			RunRequested = Input.GetKey(KeyCode.LeftShift);
			
			if (!currentState.OnUpdate())
				SwitchState();
				
			var horizontalVel = _horizontalVel.x * transform.right + _horizontalVel.z * transform.forward;
			_cc.Move((horizontalVel + _verticalVel) * Time.deltaTime);
		}

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
	}
}