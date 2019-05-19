using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TurnBehaviorDelegate();
public delegate void ActiceCharactersChangedDelegate(List<GameObject> newCharacters);

public class TurnController {

	private Dictionary<int, List<GameObject>> teams;
	private Dictionary<string, int> teamIDs;

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
		teams = new Dictionary<int, List<GameObject>>();
		teamIDs = new Dictionary<string, int>();
	}

	private void AddCharacter(GameObject character)
	{
		TurnBehavior behavior = character.GetComponent<TurnBehavior>();
		if(behavior == null)
		{
			Debug.LogError("All characters you register must have a subclass of TurnBehavior on it!");
		}
		string team = behavior.GetTeam();
		if (teamIDs.ContainsKey(team))
		{
			teams[teamIDs[team]].Add(character);
		}
		else
		{
			if (teams.Count == 0)
			{
				CurrentCharacter = character;
				Debug.Log("First player is set");
			}
			int newTeamID = teamIDs.Count;
			teamIDs.Add(team, newTeamID);
			List<GameObject> newList = new List<GameObject>();
			newList.Add(character);
			teams.Add(newTeamID, newList);
		}
	}

	private void RemoveCharacter(GameObject character)
	{
		string team = character.GetComponent<TurnBehavior>().GetTeam();
		int teamID = teamIDs[team];
		List<GameObject> teamMembers = teams[teamID];
		teamMembers.Remove(character);
		if(teamMembers.Count == 0)
		{
			teams.Remove(teamID);
			teamIDs.Remove(team);
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
		if (currentTeamMemberID > teams[currentTeamID].Count - 1)
		{
            currentTeamMemberID = 0;
            do
            {
                currentTeamID++;
                if (currentTeamID >= teams.Count - 1)
                {
                    currentTeamID = 0;
                }
            }
            while (!teams.TryGetValue(currentTeamID, out team));
		}
		CurrentCharacter = team[currentTeamMemberID];
	}

    public List<GameObject> GetActiveCharacters()
	{
		List<GameObject> re = new List<GameObject>();
		foreach(List<GameObject> team in teams.Values)
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
