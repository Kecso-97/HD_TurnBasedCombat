using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public delegate void CharacterStatsChangedDelegate(CharacterStats subject);
public delegate void CharacterActionDelegate(TurnBehavior subject);


public class EventController
{
    private static EventController instance;

    public static EventController GetInstance()
    {
        if(instance == null)
        {
            instance = new EventController();
        }
        return instance;
    }

    private EventController() { }

    

    public event CharacterStatsChangedDelegate NotifyStatsChaned;

    public event CharacterActionDelegate NotifySkillCasted;

    public event CharacterStatsChangedDelegate CharacterSelected;



    public void OnCharacterSelected(CharacterStats stats)
    {
        if (CharacterSelected != null) CharacterSelected(stats);
    }

    public void OnStatsChanged(CharacterStats subject)
    {
        if (NotifyStatsChaned != null) NotifyStatsChaned(subject);
    }

    internal void OnSkillCasted(TurnBehavior caster)
    {
        if (NotifySkillCasted != null) NotifySkillCasted(caster);
    }

}
