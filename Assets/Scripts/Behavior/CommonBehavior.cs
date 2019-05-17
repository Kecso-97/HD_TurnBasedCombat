using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBehavior : TurnBehavior
{
    private bool forceEndTurn;
	
	public override int RemainingMoveCount
	{
		get
		{
			return stats.MovementCount;
		}

        set
        {
            stats.MovementCount = value;
        }
	}

	public override int Mana
	{
		get
		{
			return stats.Mana;
		}

        set
        {
            stats.Mana = value;
        }
	}

	// Start is called before the first frame update
	void Start()
    {
		Initialize();
	}

	protected override void Initialize()
	{
		base.Initialize();
		activeSkill = null;
        forceEndTurn = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (forceEndTurn && !isSkillInProgress)
        {
            Debug.Log("Turn is over");
            EndTurn();
        }
    }

	private void LateUpdate()
	{
		SetRotation();
	}

	protected override void OnTurnEnd()
	{
        stats.Reload();
		activeSkill = null;
        forceEndTurn = false;
        Debug.Log("OnTurnEnd called");
	}

	public override void OnSuccessfulSkill(int consumedMana, int consumedMovement)
	{
        base.OnSuccessfulSkill(consumedMana, consumedMovement);
        Mana -= consumedMana;
        RemainingMoveCount -= consumedMovement;
		activeSkill = null;
        Debug.Log("Mana: " + Mana + ", Move: " + RemainingMoveCount);

        bool hasEnoughMana = false;
        for(int i = 0; i < skills.Capacity; i++)
        {
            if (skills[i].HasEnoughMana())
            {
                hasEnoughMana = true;
                break;
            }
        }
        if (!hasEnoughMana)
        {
            EndTurn();
        }
	}

	public override void OnCancelledSkill()
	{
		activeSkill = null;
	}

	private void OnDrawGizmos()
	{
		if (HasControll)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + Vector3.up, 1);
		}
	}

    public override void ForceEndTurn()
    {
        Debug.Log("Force end turn");
        forceEndTurn = true;
    }

    public override void UseSkill(int skillIndex)
    {
        if (!isSkillInProgress)
        {
            if(activeSkill != null)
            {
                activeSkill.ResetState();
            }
            activeSkill = skills[skillIndex];
        }
        
    }

    public override void SetTarget(Vector3 target)
    {
        if(activeSkill != null) activeSkill.Target = target;
    }
}
