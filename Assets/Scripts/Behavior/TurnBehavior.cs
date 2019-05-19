
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public abstract class TurnBehavior : MonoBehaviour
{
	public string team;

    public bool visualIndicationOfSkills;

	protected bool isSkillInProgress
	{
		get
		{
			if(activeSkill != null)
			{
				return activeSkill.Casted;
			}
			else
			{
				return false;
			}
		}
	}

	public abstract int RemainingMoveCount
	{
		get;
        set;
	}

	public abstract int Mana
	{
		get;
        set;
	}

	protected List<Skill> skills;

	protected Skill activeSkill;

    protected CharacterStats stats;
    
    protected bool HasControll
	{
		get
		{
			if (controller != null)
			{
				return controller.HasControll(gameObject);
			}
			else
			{
				return false;
			}
		}
	}
    
	protected Camera mainCamera;

	protected TurnController controller;

    protected EventController events;

	private Transform character;

	protected NavMeshAgent agent;

	protected virtual void Initialize()
	{
		controller = TurnController.GetInstance();
		controller.OnCharacterEnter(this.gameObject);

        events = EventController.GetInstance();

		mainCamera = Camera.main;
		character = transform.Find("body");
		agent = GetComponent<NavMeshAgent>();
		skills = new List<Skill>(GetComponents<Skill>());
		agent.isStopped = true;
        stats = GetComponent<CharacterStats>();
	}

	protected abstract void OnTurnEnd();
	
	public string GetTeam()
	{
		return team;
	}

	public void OnDestroy()
	{
		controller.OnCharactreExit(this.gameObject);
	}

	public void EndTurn()
	{
		OnTurnEnd();
		controller.EndTurn();
	}

	protected void SetRotation()
	{
		character.transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
	}

	

	protected float DestinationDistance()
	{
		//TODO calculate the length of the path

		float distance = (transform.position - agent.destination).magnitude;
		//Debug.Log("[DISTANCE]: " + distance);
		return distance;
	}

	public bool IsActiveSkill(Skill subject)
	{
		return activeSkill == subject;
	}

	public virtual void OnSuccessfulSkill(int consumedMana, int consumedMovement)
    {
        events.OnSkillCasted(this);
    }

	public abstract void OnCancelledSkill();

    public abstract void ForceEndTurn();

    public abstract void UseSkill(int skillIndex);

    public abstract void SetTarget(Vector3 target);

}