using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileSkill : Skill
{
    public GameObject Effect;

    private GameObject spawnedObject;

    public float lifetime;

    private float remainingLifetime;

    public int damage;

    public float speed;

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
        if (!casted && TargetEnabled)
        {
            if (!HasEnoughMana())
            {
                Cancel();
                return;
            }
            
            Projectile projectile = Effect.GetComponent<Projectile>();
            projectile.damage = damage;
            projectile.direction = Target - transform.position;
            projectile.direction.y = 0;
            projectile.speed = speed;
            spawnedObject = Instantiate(Effect, transform.position + (Target - transform.position).normalized + Vector3.up, Quaternion.Euler(90, 0, 0));
            casted = true;
            Debug.Log("Skill casted");
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
                Gizmos.color = Color.green;
                if (useVisuals)
                {
                    Gizmos.DrawWireSphere(hit.point, 0.75f);
                    Gizmos.DrawLine(transform.position, hit.point);
                }
            }
        }
    }
}
