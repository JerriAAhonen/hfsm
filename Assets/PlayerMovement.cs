using UnityEngine;

public class MovementStateTriggers
{
	public const int Walk = 1;
	public const int Run = 2;
	public const int Jump = 3;
}

public class PlayerMovement : MonoBehaviour
{
	[Header("Grounded")] 
	[SerializeField] private float gravity = -30f;
	[SerializeField] private float groundedGravity = -0.05f;
	[Header("Jump")] 
	[SerializeField] private float jumpHeight = 1.5f;

	// Level 0
	private readonly StateMachine rootState = new RootState();
	
	// Level 1
	private readonly StateMachine idleState = new IdleState();
	private readonly StateMachine moveState = new MoveState();
	
	// Level 2
	private readonly StateMachine walkState = new WalkState();
	private readonly StateMachine runState = new RunState();
	private readonly StateMachine jumpState = new JumpState();

	private CharacterController cc;
	private Vector3 horizontalVel;
	private Vector3 verticalVel;

	public bool IsGrounded => cc.isGrounded;
	public float VerticalVel
	{
		get => verticalVel.y;
		set => verticalVel.y = value;
	}

	public float Gravity => gravity;
	public float GroundedGravity => groundedGravity;
	public float JumpHeight => jumpHeight;
	
	// Input
	public bool RunRequested { get; private set; }
	public bool JumpRequested { get; set; }
	public bool CrouchRequested { get; set; }

	private void Awake()
	{
		cc = GetComponent<CharacterController>();
	}

	private void Start()
	{
		rootState.LoadSubState(idleState);
		rootState.LoadSubState(moveState);
		
		moveState.LoadSubState(walkState);
		moveState.LoadSubState(runState);
		moveState.LoadSubState(jumpState);
		moveState.AddTransition(walkState, runState, MovementStateTriggers.Run);
		moveState.AddTransition(runState, jumpState, MovementStateTriggers.Jump);
		moveState.AddExitTransition(moveState, idleState);
		
		rootState.EnterStateMachine(this);
	}

	private void Update()
	{
		moveState.UpdateStateMachine();
		cc.Move(verticalVel * Time.deltaTime);
	}
}