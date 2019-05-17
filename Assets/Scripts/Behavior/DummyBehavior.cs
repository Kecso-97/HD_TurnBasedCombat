using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DummyBehavior /*: TurnBehavior*/
{
/*
	protected Dictionary<string, Skill> keyBinding;

	public float maxMovementDistance;

	public int moveCount;
	public int actionCount;

	protected int remainingMoveCount;
	protected int remainingActionCount;

	protected bool isInAction;


	protected override void Initialize()
	{
		base.Initialize();
		remainingMoveCount = moveCount;
		remainingActionCount = actionCount;
		Skill[] skills = GetComponents<Skill>();
		keyBinding = new Dictionary<string, Skill>();
		try
		{
			keyBinding.Add("q", skills[0]);
			keyBinding.Add("w", skills[1]);
			keyBinding.Add("e", skills[2]);
			keyBinding.Add("r", skills[3]);
		}
		catch (System.IndexOutOfRangeException)
		{
			
		}
	}

	protected override void OnTurnEnd()
	{
		remainingMoveCount = moveCount;
		remainingActionCount = actionCount;
	}

	// Start is called before the first frame update
	void Start()
    {
		Initialize();
    }

    // Update is called once per frame
    void Update()
    {
		if (HasControll)
		{
			if (Input.anyKeyDown && agent.isStopped)
			{
				string actionKeyPressed = Input.inputString;
				Debug.Log("Pressed [" + actionKeyPressed + "]");
				if (keyBinding.ContainsKey(actionKeyPressed))//needs some rework
				{
					activeSkill = keyBinding[actionKeyPressed];
					isInAction = true;
				}
				else if (!Input.GetMouseButtonDown(0))
				{
					isInAction = false;
				}
			}

			if (isInAction)
			{
				if(remainingActionCount > 0)
				{
					ActionCommand();
				}
				//Debug.Log(remainingMoveCount + " action left");
			}
			else
			{
				if (remainingMoveCount > 0)
				{
					MoveCommand();
				}
				if (DestinationDistance() == 0)
				{
					agent.isStopped = true;
				}
				//Debug.Log(remainingMoveCount + " moves left");
			}

			if(agent.isStopped && remainingMoveCount == 0 && remainingActionCount == 0)
			{
				EndTurn();
			}
		}
    }

	protected void ActionCommand()
	{
		if (Input.GetMouseButtonDown(0) && agent.isStopped)
		{
			if (MouseToWorld(out RaycastHit hit))
			{
				activeSkill.Execute();
				remainingActionCount--;
				isInAction = false;
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			isInAction = false;
		}
	}

	protected void MoveCommand()
	{
		if (Input.GetMouseButtonDown(1) && agent.isStopped)
		{
			if(MouseToWorld(out RaycastHit hit))
			{
				agent.destination = hit.point;
				if (DestinationDistance() < maxMovementDistance)
				{
					remainingMoveCount--;
					agent.isStopped = false;
				}
				else
				{
					agent.isStopped = true;
					Debug.Log("[DISTANCE]: Destination is out of range");
				}
			}
		}
	}

	private void LateUpdate()
	{
		SetRotation();
	}

	private void OnDrawGizmos()
	{
		if (HasControll)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, maxMovementDistance);
		}
	}

	public override void OnSuccessfulMovement()
	{
		throw new System.NotImplementedException();
	}

	public override void OnSuccessfulSkill()
	{
		throw new System.NotImplementedException();
	}
	*/
}
