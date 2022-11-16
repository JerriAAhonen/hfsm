using System;
using System.Collections;
using System.Collections.Generic;
using Game.Shared;
using UnityEngine;

public class asd : MonoBehaviour
{
	[Header("Grounded")] 
	[SerializeField] private float gravity = -30f;
	[SerializeField] private float groundedGravity = -0.05f;
	[Header("Jump")]
	[SerializeField] private float jumpHeight = 1.5f;
	[Header("Walk")]
	[SerializeField] private float walkSpeed = 3f;
	[Header("Run")]
	[SerializeField] private float runSpeed = 6f;

	private CharacterController cc;
	private Vector3 horizontalVel;
	private Vector3 verticalVel;
	
	// State
	public bool IsGrounded => cc.isGrounded;
	public float VerticalVel
	{
		get => verticalVel.y;
		set => verticalVel.y = value;
	}
	
	// Params
	public float Gravity => gravity;
	public float GroundedGravity => groundedGravity;
	public float JumpHeight => jumpHeight;
	public float WalkSpeed => walkSpeed;
	public float RunSpeed => runSpeed;
	
	// Input
	public bool RunRequested { get; private set; }
	public bool PendingJump { get; set; }

	#region MonoBehaviour
	
	private void Awake()
	{
		cc = GetComponent<CharacterController>();
	}

	private void Start()
	{
		InputManager.Instance.Jump += OnJump;
		InputManager.Instance.Run += OnRun;

		void OnJump() => PendingJump = true;
		void OnRun(bool pressed, bool isToggle) => RunRequested = pressed;
	}

	private void Update()
	{
		//currentState.OnUpdate();

		cc.Move((horizontalVel + verticalVel) * Time.deltaTime);
	}

	#endregion

	/*public void SetCurrentState(States newState)
	{
		currentState.OnExit();
		currentState = states[newState];
		currentState.OnEnter();
	}*/

	public void SetHorizontalVelocity(Vector2 vel)
	{
		horizontalVel.x = vel.x;
		horizontalVel.z = vel.y;
	}
}