using System;
using Game.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	private const bool IsRunToggle = true;
	private const bool IsCrouchToggle = false;

	private PlayerInputActions pia;
	private InputAction movementAction;
	private InputAction lookAction;
	private Action removeListeners;

	public bool IsReady { get; private set; }
	public bool HasMovementInput => MovementInput.x != 0 || MovementInput.y != 0;
	public Vector2 MovementInput { get; private set; }
	public Vector2 LookInput { get; private set; }

	public event Action Jump;
	public event Action<bool, bool> Crouch;
	public event Action<bool, bool> Run;
	public event Action<bool> Shoot;
	public event Action<bool> Aim;
	public event Action<bool, bool> Lean;
	public event Action Reload;
	public event Action SwitchWeapon;

	private void Start()
	{
		pia ??= new PlayerInputActions();

		EnableInput();
		IsReady = true;
	}

	private void Update()
	{
		if (!IsReady) return;

		MovementInput = movementAction.ReadValue<Vector2>();
		LookInput = lookAction.ReadValue<Vector2>();
	}

	private void OnEnable()
	{
		if (IsReady)
			EnableInput();
	}

	private void OnDisable()
	{
		DisableInput();
	}

	private void EnableInput()
	{
		movementAction = pia.Player.Move;
		movementAction.Enable();

		lookAction = pia.Player.Look;
		lookAction.Enable();

		pia.Player.Jump.performed += OnJumpPerformed;
		pia.Player.Jump.Enable();

		pia.Player.Crouch.performed += CrouchPressed;
		pia.Player.Crouch.canceled += CrouchCancelled;
		pia.Player.Crouch.Enable();

		pia.Player.Run.performed += RunPressed;
		pia.Player.Run.canceled += RunCancelled;
		pia.Player.Run.Enable();

		pia.Player.Shoot.performed += OnShootPerformed;
		pia.Player.Shoot.canceled += OnShootCancelled;
		pia.Player.Shoot.Enable();

		pia.Player.Aim.performed += OnAimPerformed;
		pia.Player.Aim.canceled += OnAimCancelled;
		pia.Player.Aim.Enable();

		pia.Player.Lean_Right.performed += OnLeanRightPerformed;
		pia.Player.Lean_Right.canceled += OnLeanRightCancelled;
		pia.Player.Lean_Left.performed += OnLeanLeftPerformed;
		pia.Player.Lean_Left.canceled += OnLeanLeftCancelled;
		pia.Player.Lean_Right.Enable();
		pia.Player.Lean_Left.Enable();

		pia.Player.Reload.performed += OnReloadPerformed;
		pia.Player.Reload.Enable();

		pia.Player.PrimaryWeapon.performed += OnPrimaryWeaponPerformed;
		pia.Player.PrimaryWeapon.Enable();

		pia.Player.SecondaryWeapon.performed += OnSecondaryWeaponPerformed;
		pia.Player.SecondaryWeapon.Enable();

		removeListeners = RemoveListeners;

		void OnJumpPerformed(InputAction.CallbackContext _) => Jump?.Invoke();
		void CrouchPressed(InputAction.CallbackContext _) => Crouch?.Invoke(true, IsCrouchToggle);
		void CrouchCancelled(InputAction.CallbackContext _) => Crouch?.Invoke(false, IsCrouchToggle);
		void RunPressed(InputAction.CallbackContext _) => Run?.Invoke(true, IsRunToggle);
		void RunCancelled(InputAction.CallbackContext _) => Run?.Invoke(false, IsRunToggle);
		void OnShootPerformed(InputAction.CallbackContext _) => Shoot?.Invoke(true);
		void OnShootCancelled(InputAction.CallbackContext _) => Shoot?.Invoke(false);
		void OnAimPerformed(InputAction.CallbackContext _) => Aim?.Invoke(true);
		void OnLeanRightPerformed(InputAction.CallbackContext _) => Lean?.Invoke(true, true);
		void OnLeanRightCancelled(InputAction.CallbackContext _) => Lean?.Invoke(false, true);
		void OnLeanLeftPerformed(InputAction.CallbackContext _) => Lean?.Invoke(true, false);
		void OnLeanLeftCancelled(InputAction.CallbackContext _) => Lean?.Invoke(false, false);
		void OnAimCancelled(InputAction.CallbackContext _) => Aim?.Invoke(false);
		void OnReloadPerformed(InputAction.CallbackContext _) => Reload?.Invoke();
		void OnPrimaryWeaponPerformed(InputAction.CallbackContext _) => SwitchWeapon?.Invoke();
		void OnSecondaryWeaponPerformed(InputAction.CallbackContext _) => SwitchWeapon?.Invoke();

		void RemoveListeners()
		{
			pia.Player.Jump.performed -= OnJumpPerformed;
			pia.Player.Crouch.performed -= CrouchPressed;
			pia.Player.Crouch.canceled -= CrouchCancelled;
			pia.Player.Run.performed -= RunPressed;
			pia.Player.Run.canceled -= RunCancelled;
			pia.Player.Shoot.performed -= OnShootPerformed;
			pia.Player.Shoot.canceled -= OnShootCancelled;
			pia.Player.Aim.performed -= OnAimPerformed;
			pia.Player.Aim.canceled -= OnAimCancelled;
			pia.Player.Lean_Right.performed -= OnLeanRightPerformed;
			pia.Player.Lean_Right.canceled -= OnLeanRightCancelled;
			pia.Player.Lean_Left.performed -= OnLeanLeftPerformed;
			pia.Player.Lean_Left.canceled -= OnLeanLeftCancelled;
			pia.Player.Reload.performed -= OnReloadPerformed;
			pia.Player.PrimaryWeapon.performed -= OnPrimaryWeaponPerformed;
			pia.Player.SecondaryWeapon.performed -= OnSecondaryWeaponPerformed;
		}
	}

	private void DisableInput()
	{
		removeListeners?.Invoke();

		movementAction.Disable();
		lookAction.Disable();

		pia.Player.Jump.Disable();
		pia.Player.Crouch.Disable();
		pia.Player.Run.Disable();
		pia.Player.Shoot.Disable();
		pia.Player.Aim.Disable();
		pia.Player.Lean_Right.Disable();
		pia.Player.Lean_Left.Disable();
		pia.Player.Reload.Disable();
	}
}