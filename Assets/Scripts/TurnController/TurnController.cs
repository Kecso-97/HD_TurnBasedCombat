using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TurnBehaviorDelegate();
public delegate void ActiceCharactersChangedDelegate(List<GameObject> newCharacters);

public class TurnController {

    //private Dictionary<int, List<GameObject>> teams;
    private List<List<GameObject>> teams;
	//private Dictionary<string, int> teamIDs;

	private static TurnController instance;

	public int currentTeamMemberID;
	public int currentTeamID;

    public event TurnBehaviorDelegate NotifyTurnOver;

    public event ActiceCharactersChangedDelegate NotifyCharactersChange;

    private GameObject currentCharacter;
    private TurnBehavior currentCharacterBehavior;
    private CharacterStats currentCharacterStats;

    public GameObject CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }

        set
        {
            currentCharacter = value;
            currentCharacterBehavior = currentCharacter.GetComponent<TurnBehavior>();
            if (currentCharacterBehavior == null) Debug.LogError("No Behavior is atached to the character!");
            currentCharacterStats = currentCharacter.GetComponent<CharacterStats>();
            if (currentCharacterStats == null) Debug.LogError("No CharacterStats component is atached to the character!");
        }
    }

    public TurnBehavior CurrentCharacterBehavior
    {
        get
        {
            return currentCharacterBehavior;
        }
    }

    public CharacterStats CurrentCharacterStats
    {
        get
        {
            return currentCharacterStats;
        }
    }

    private TurnController()
	{
		teams = new List<List<GameObject>>();
		//teamIDs = new Dictionary<string, int>();
	}

	private void AddCharacter(GameObject character)
	{
		TurnBehavior behavior = character.GetComponent<TurnBehavior>();
		if(behavior == null)
		{
			Debug.LogError("All characters you register must have a subclass of TurnBehavior on it!");
		}
		string team = behavior.GetTeam();

        bool newTeam = true;
        foreach(var teamList in teams)
        {
            if(teamList[0].GetComponent<TurnBehavior>().team == team)
            {
                teamList.Add(character);
                newTeam = false;
            }
        }
        if (newTeam)
        {
            var newList = new List<GameObject>();
            newList.Add(character);
            if(teams.Count == 0)
            {
                CurrentCharacter = character;
            }
            teams.Add(newList);

        }
	}

	private void RemoveCharacter(GameObject character)
	{
		string team = character.GetComponent<TurnBehavior>().GetTeam();
        //int teamID = teamIDs[team];
        List<GameObject> emptyTeam = null;
        foreach(var teamList in teams)
        {
            if(teamList[0].GetComponent<TurnBehavior>().team == team)
            {
                teamList.Remove(character);
                if (teamList.Count == 0)
                {
                    emptyTeam = teamList;
                }
                break;
            }
        }
        if (emptyTeam != null)
        {
            teams.Remove(emptyTeam);
        }
	}

	public static TurnController GetInstance()
	{
		if(instance == null)
		{
			instance = new TurnController();
		}
		return instance;
	}

	public void EndTurn()
	{
        IncrementTurnVariables();
		if (NotifyTurnOver != null) NotifyTurnOver();
	}
    
	public bool HasControll(GameObject subject)
	{
		return CurrentCharacter == subject;
	}
    
	public void IncrementTurnVariables()
	{
        List<GameObject> team = teams[currentTeamID];
		currentTeamMemberID++;
		if (currentTeamMemberID >= teams[currentTeamID].Count)
		{
            currentTeamMemberID = 0;
            currentTeamID++;
            if (currentTeamID >= teams.Count)
            {
                currentTeamID = 0;
            }
		}
		CurrentCharacter = team[currentTeamMemberID];
	}

    public List<GameObject> GetActiveCharacters()
	{
		List<GameObject> re = new List<GameObject>();
		foreach(List<GameObject> team in teams)
		{
			re.AddRange(team);
		}
		return re;
	}

	public void OnCharacterEnter(GameObject newCharacter)
	{
		AddCharacter(newCharacter);
		if(NotifyCharactersChange != null) NotifyCharactersChange(GetActiveCharacters());
		Debug.Log("A character have been added");
	}

	public void OnCharactreExit(GameObject oldCharacter)
	{
		RemoveCharacter(oldCharacter);
		if (NotifyCharactersChange != null) NotifyCharactersChange(GetActiveCharacters());
		Debug.Log("A character have been removed");
	}
}
