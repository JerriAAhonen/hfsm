using System;
using System.Collections.Generic;
using System.Data;

public abstract class StateMachine
{
	private StateMachine currentSubState;
	private StateMachine defaultSubState;
	private StateMachine parent;

	private readonly Dictionary<Type, StateMachine> subStates = new();
	private readonly Dictionary<int, StateMachine> transitions = new();
	private readonly Dictionary<StateMachine, StateMachine> exitTransitions = new();

	protected PlayerMovement ctx;

	public void EnterStateMachine(PlayerMovement ctx)
	{
		this.ctx = ctx;

		OnEnter();
		if (currentSubState == null && defaultSubState != null)
			currentSubState = defaultSubState;
		currentSubState?.EnterStateMachine(ctx);
	}

	public bool UpdateStateMachine()
	{
		if (!OnUpdate())
			return false;
 
		if (currentSubState != null)
		{
			bool doContinue = currentSubState.UpdateStateMachine();
			if (!doContinue)
			{
				if (exitTransitions.TryGetValue(currentSubState, out StateMachine exitTo))
				{
					currentSubState.ExitStateMachine();
					currentSubState = exitTo;
					currentSubState.EnterStateMachine(ctx);
				}
				else
				{
					return false;
				}
			}
		}
 
		return true;
	}

	public void ExitStateMachine()
	{
		OnExit();
		currentSubState?.ExitStateMachine();
	}

	protected abstract void OnEnter();
	/// <returns>doContinue</returns>
	protected abstract bool OnUpdate();
	protected abstract void OnExit();

	public void LoadSubState(StateMachine subState)
	{
		if (subStates.Count == 0)
			defaultSubState = subState;

		subState.parent = this;
		subStates.Add(subState.GetType(), subState);
	}

	public void AddTransition(StateMachine from, StateMachine to, int trigger)
	{
		if (!subStates.TryGetValue(from.GetType(), out _))
			throw new Exception(
				$"State {GetType()} does not have a subState of type {from.GetType()} to transition from.");
		if (!subStates.TryGetValue(to.GetType(), out _))
			throw new Exception(
				$"State {GetType()} does not have a subState of type {to.GetType()} to transition from.");

		from.transitions.Add(trigger, to);
	}
	
	public void AddExitTransition(StateMachine from, StateMachine to)
	{
		exitTransitions[from] = to;
	}

	public void SendTrigger(int trigger)
	{
		var root = this;
		while (root?.parent != null)
			root = root.parent;

		while (root != null)
		{
			if (root.transitions.TryGetValue(trigger, out var toState))
			{
				root.parent?.ChangeSubState(toState);
				return;
			}

			root = root.currentSubState;
		}
	}

	private void ChangeSubState(StateMachine state)
	{
		currentSubState?.ExitStateMachine();
		var newState = subStates[state.GetType()];
		currentSubState = newState;
		newState.EnterStateMachine(ctx);
	}
}