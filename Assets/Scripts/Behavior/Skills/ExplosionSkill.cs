using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill : Skill
{
	public GameObject Effect;

	private GameObject spawnedObject;

	public float lifetime;

	private float remainingLifetime;

	public float range;

    public int damage;

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
                DealAreaDamage areaDamage = Effect.GetComponent<DealAreaDamage>();
				spawnedObject = Instantiate(Effect, Target + Vector3.up*0.001f, Quaternion.identity);
                areaDamage.damage = damage;
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
                if (useVisuals) Gizmos.DrawWireSphere(hit.point, 1.13f);
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
