using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementSkill : Skill
{
	public float maxMovementDistance;

	private NavMeshAgent agent;

	protected override void Initialize()
	{
		base.Initialize();
		agent = GetComponent<NavMeshAgent>();
	}

	// Start is called before the first frame update
	void Start()
    {
		Initialize();
    }

    // Update is called once per frame
    void Update()
    {
		if (isActive)
		{
			Execute();
		}
	}

	private void Execute()
	{
		if (!casted && TargetEnabled)
		{
            if (!HasEnoughMana())
            {
                Cancel();
                return;
            }
            agent.destination = Target;
			agent.isStopped = true;
			if(DestinationDistance() > maxMovementDistance)
			{
				Debug.Log("Destination is out of range!");
			}
			else
			{
				agent.isStopped = false;
				casted = true;
				Debug.Log("Movement");
			}
		}
		if (casted)
		{
			if (DestinationDistance() <= 0.01)
			{
                Debug.Log("Successful movement");
				behavior.OnSuccessfulSkill(manaCost, movementCost);
				Finish();
			}
		}
	}

	protected float DestinationDistance()
	{
		//TODO calculate the length of the path

		float distance = (transform.position - agent.destination).magnitude;
		//Debug.Log("[DISTANCE]: " + distance);
		return distance;
	}

	public override void ResetState()
	{
		base.ResetState();
		agent.isStopped = true;
	}

	private void OnDrawGizmos()
	{
        if (!useVisuals) return;

        if (isActive && !casted)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, maxMovementDistance);
		}
	}
}
