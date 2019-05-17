using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTempObjectSkill : Skill
{
	public GameObject DemonstrationObject;

	private GameObject spawnedObject;

	public float lifetime;

	private float remainingLifetime;

	public float range;

    private bool isTargetInRange;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        remainingLifetime = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Execute();
        }
    }

    protected void Execute()
	{
		if(!casted && TargetEnabled)
		{
            if(!HasEnoughMana())
            {
                Cancel();
                return;
            }
			if(isTargetInRange)
			{
				spawnedObject = Instantiate(DemonstrationObject, Target, Quaternion.identity);
				casted = true;
				Debug.Log("Skill casted");
			}
			else
			{
				Debug.Log("Out of range!");
			}
		}
		if (casted)
		{
			remainingLifetime -= Time.deltaTime;
			if (remainingLifetime <= 0)
			{
				Destroy(spawnedObject);
				behavior.OnSuccessfulSkill(manaCost, movementCost);
				Finish();
			}
		}
	}

	public override void ResetState()
	{
		base.ResetState();
		remainingLifetime = lifetime;
	}

	private void OnDrawGizmos()
	{
        if (isActive && !casted)
        {
            if (MouseToWorld(out RaycastHit hit))
            {
                if (IsTargetInRange(hit.point))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                if (useVisuals) Gizmos.DrawWireSphere(hit.point, 0.5f);
            }
            
            Gizmos.color = Color.blue;
            if (useVisuals) Gizmos.DrawWireSphere(transform.position, range);
            
        }
	}

    private bool IsTargetInRange(Vector3 target)
    {
        if((transform.position - target).magnitude < range)
        {
            isTargetInRange = true;
        }
        else
        {
            isTargetInRange = false;
        }
        return isTargetInRange;
    }
}
