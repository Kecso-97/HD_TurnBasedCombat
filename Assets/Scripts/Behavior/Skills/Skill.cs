using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
	protected TurnBehavior behavior;

	private Camera mainCamera;

    protected bool useVisuals;

    public int manaCost;
    public int movementCost;

	private Vector3 targetPosition;
	private bool targetEnabled;

	public Vector3 Target
	{
		get
		{
			return targetPosition;
		}
		set
		{
			targetPosition = value;
			targetEnabled = true;
		}
	}

	public bool TargetEnabled
	{
		get
		{
			return targetEnabled;
		}
	}

	protected bool casted;

	public bool Casted
	{
		get
		{
			return casted;
		}
	}

	public bool isActive
	{
		get
		{
			if(behavior != null)
			{
				return behavior.IsActiveSkill(this);
			}
			else
			{
				return false;
			}
		}
	}

	protected virtual void Initialize()
	{
		behavior = GetComponent<TurnBehavior>();
		mainCamera = Camera.main;
        useVisuals = behavior.visualIndicationOfSkills;
	}

	public virtual void ResetState()
	{
		casted = false;
        targetEnabled = false;
	}

	protected bool MouseToWorld(out RaycastHit hit)
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			return true;
		}
		return false;
	}

	protected void Finish()
	{
		ResetState();
        behavior.OnSuccessfulSkill(manaCost, movementCost);
	}

    protected void Cancel()
    {
        ResetState();
        behavior.OnCancelledSkill();
    }

    public bool HasEnoughMana()
    {
        if(behavior.Mana < manaCost)
        {
            Debug.Log("You do not have enough mana");
            return false;
        }
        if (behavior.RemainingMoveCount < movementCost)
        {
            Debug.Log("You do not have any movement left");
            return false;
        }
        return true;
    }
}
