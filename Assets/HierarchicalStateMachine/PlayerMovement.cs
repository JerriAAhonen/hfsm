using UnityEngine;

namespace HierarchicalSM
{

	public static class MovementStateTriggers
	{
		/*
		 * Walk
		 * Run
		 * Jump
		 * Crouch
		 */

		public const int WalkToRun = 1;
		public const int WalkToJump = 2;
		public const int WalkToCrouch = 3;
		public const int RunToWalk = 4;
		public const int RunToJump = 5;
		public const int RunToCrouch = 6;
		public const int JumpToWalk = 7;

		public const int JumpToRun = 8;

		//public const int JumpToCrouch = 9;
		public const int CrouchToWalk = 10;

		public const int CrouchToRun = 11;
		//public const int CrouchToJump = 12;
	}

	public class PlayerMovement : MonoBehaviour
	{
		[Header("Grounded")] [SerializeField] private float _gravity = -30f;
		[SerializeField] private float _groundedGravity = -0.05f;
		[Header("Jump")] [SerializeField] private float _jumpHeight = 1.5f;
		[Header("Walk")] [SerializeField] private float _walkSpeed = 3f;
		[Header("Run")] [SerializeField] private float _runSpeed = 6f;

		// Level 0
		private readonly StateMachine rootState = new RootState();

		// Level 1
		private readonly StateMachine idleState = new IdleState();
		private readonly StateMachine moveState = new MoveState();

		// Level 2
		private readonly StateMachine walkState = new WalkState();
		private readonly StateMachine runState = new RunState();
		private readonly StateMachine jumpState = new JumpState();
		private readonly StateMachine crouchState = new CrouchState();

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
		}

		private void Start()
		{
			rootState.LoadSubState(idleState);
			rootState.LoadSubState(moveState);
			rootState.AddExitTransition(moveState, idleState);
			rootState.AddExitTransition(idleState, moveState);

			moveState.LoadSubState(walkState);
			moveState.LoadSubState(runState);
			moveState.LoadSubState(jumpState);
			moveState.LoadSubState(crouchState);
			moveState.AddTransition(walkState, runState, MovementStateTriggers.WalkToRun);
			moveState.AddTransition(walkState, jumpState, MovementStateTriggers.WalkToJump);
			moveState.AddTransition(walkState, crouchState, MovementStateTriggers.WalkToCrouch);
			moveState.AddTransition(runState, walkState, MovementStateTriggers.RunToWalk);
			moveState.AddTransition(runState, jumpState, MovementStateTriggers.RunToJump);
			moveState.AddTransition(runState, crouchState, MovementStateTriggers.RunToCrouch);
			moveState.AddTransition(jumpState, runState, MovementStateTriggers.JumpToRun);
			moveState.AddTransition(jumpState, walkState, MovementStateTriggers.JumpToWalk);
			moveState.AddTransition(crouchState, walkState, MovementStateTriggers.CrouchToWalk);
			moveState.AddTransition(crouchState, runState, MovementStateTriggers.CrouchToRun);

			rootState.EnterStateMachine(this);
		}

		private void Update()
		{
			JumpRequested = Input.GetKeyDown(KeyCode.Space);
			CrouchRequested = Input.GetKeyDown(KeyCode.LeftControl);
			RunRequested = Input.GetKeyDown(KeyCode.LeftShift);

			rootState.UpdateStateMachine();

			var horizontalVel = _horizontalVel.x * transform.right + _horizontalVel.z * transform.forward;
			_cc.Move((horizontalVel + _verticalVel) * Time.deltaTime);
		}
	}
}