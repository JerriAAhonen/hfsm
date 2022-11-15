using System;
using System.Collections;
using System.Collections.Generic;
using Game.Shared;
using UnityEngine;

public enum States
{
	Grounded,
	Jump
}

public class MovementStateMachine : MonoBehaviour
{
	[Header("Grounded")] 
	[SerializeField] private float gravity = -30f;
	[SerializeField] private float groundedGravity = -0.05f;
	[Header("Jump")]
	[SerializeField] private float jumpHeight = 1.5f;
	
	private Dictionary<States, BaseState> states = new();
	private BaseState currentState;

	private CharacterController cc;
	private Vector3 verticalVel;
	
	// State
	public bool IsGrounded => cc.isGrounded;
	
	// Params
	public float Gravity => gravity;
	public float GroundedGravity => groundedGravity;
	public float JumpHeight => jumpHeight;

	#region MonoBehaviour
	
	private void Awake()
	{
		cc = GetComponent<CharacterController>();
		states = new Dictionary<States, BaseState>
		{
			{ States.Grounded, new GroundedState(this) },
			{ States.Jump, new JumpState(this) }
		};

		currentState = states[States.Grounded];
		SetCurrentState(States.Grounded);
	}

	private void Update()
	{
		currentState.OnUpdate();

		cc.Move(verticalVel * Time.deltaTime);
	}

	#endregion

	public void SetCurrentState(States newState)
	{
		currentState.OnExitState();
		currentState = states[newState];
		currentState.OnEnterState();
	}

	public float VerticalVel
	{
		get => verticalVel.y;
		set => verticalVel.y = value;
	}
}