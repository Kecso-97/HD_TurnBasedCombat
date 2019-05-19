using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterStats : MonoBehaviour
{
    private int health;
    public int maxHealth;

    private int mana;
    public int maxMana;

    private int movementCount;
    public int maxMovementCount;

    private EventController events;

    public Sprite fullDisplay;
    public Sprite iconDisplay;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            if(health <= 0)
            {
                health = 0;
                Die();
            }
            else if(health > maxHealth)
            {
                health = maxHealth;
            }
            events.OnStatsChanged(this);
        }
    }

    public int Mana
    {
        get
        {
            return mana;
        }

        set
        {
            mana = value;
            if(mana < 0)
            {
                mana = 0;
            }
            if(mana > maxMana)
            {
                mana = maxMana;
            }
            events.OnStatsChanged(this);
        }
    }

    public int MovementCount
    {
        get
        {
            return movementCount;
        }

        set
        {
            movementCount = value;
            if (movementCount <= 0)
            {
                movementCount = 0;
            }
            else if (movementCount > maxMovementCount)
            {
                movementCount = maxMovementCount;
            }
            events.OnStatsChanged(this);
        }
    }

    private void Die()
    {
        Debug.Log(name + " died");
        Destroy(gameObject);
    }

    public void Reload()
    {
        MovementCount = maxMovementCount;
        Mana = maxMana;
        Debug.Log("Stats reloaded!");
    }


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        mana = maxMana;
        movementCount = maxMovementCount;
        events = EventController.GetInstance();

        fullDisplay = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        if (iconDisplay == null) iconDisplay = fullDisplay;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }


    public void DisplayDetails()
    {
        events.OnCharacterSelected(this);
    }

}
