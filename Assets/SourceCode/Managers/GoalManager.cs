using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BlankGoal
{
	public int _numberNeeded;
	public int _numberCollected;
	public Sprite _goalSprite;
	public string _matchValue;
}


public class GoalManager : MonoBehaviour
{
	public BlankGoal[] _levelGoal;
	public List<GoalPanel> _currentGoals = new List<GoalPanel>(); 
	public GameObject _goalPrefab;
	public GameObject _goalIntroParent;
	public GameObject _goalGameParent;
	private Board _board;

    // Start is called before the first frame update
    void Start() 
	{ 
		_board = FindObjectOfType<Board>(); 
		GetGoals();
		SetupGoals();
	}

	void GetGoals()
	{
		if(_board != null)
		{
			if(_board._world != null)
			{
				if(_board._level < _board._world.levels.Length)
				{
					if(_board._world.levels[_board._level] != null)
					{
						_levelGoal = _board._world.levels[_board._level].levelGoals;
						for (int i = 0; i < _levelGoal.Length; i++)
						{
							_levelGoal[i]._numberCollected = 0;
						}
					}
				}
			}
		}
	}

    void SetupGoals()
	{
		for (int i = 0; i < _levelGoal.Length; i++)
		{
			GameObject _goal = Instantiate(_goalPrefab, _goalIntroParent.transform.position, Quaternion.identity);
			_goal.transform.SetParent(_goalIntroParent.transform);

			GoalPanel _panel = _goal.GetComponent<GoalPanel>();
			_panel._thisSprite = _levelGoal[i]._goalSprite;
			_panel._thisString =  "0/" + _levelGoal[i]._numberNeeded;

			GameObject _gameGoal = Instantiate(_goalPrefab, _goalGameParent.transform.position, Quaternion.identity);
			_gameGoal.transform.SetParent(_goalGameParent.transform);

			_panel = _gameGoal.GetComponent<GoalPanel>();
			_currentGoals.Add(_panel);
			_panel._thisSprite = _levelGoal[i]._goalSprite;
			_panel._thisString =  "0/" + _levelGoal[i]._numberNeeded;
		}

		switch (_levelGoal.Length)
		{
			case 1: _goalIntroParent.GetComponent<HorizontalLayoutGroup>().spacing = 0;				//	horizontal
					_goalIntroParent.GetComponent<HorizontalLayoutGroup>().padding.left = 1000;		//	horizontal
					_goalGameParent.GetComponent<VerticalLayoutGroup>().spacing = 7777;				//	vertical
					_goalGameParent.GetComponent<VerticalLayoutGroup>().padding.left = 25910;		//	vertical
					break;
			case 2: _goalIntroParent.GetComponent<HorizontalLayoutGroup>().spacing = 22222; 		//	horizontal
					_goalIntroParent.GetComponent<HorizontalLayoutGroup>().padding.left = -8650;	//	horizontal
					_goalGameParent.GetComponent<VerticalLayoutGroup>().spacing = 7777;				//	vertical
					_goalGameParent.GetComponent<VerticalLayoutGroup>().padding.left = 25910;		//	vertical
					break;
			case 3: _goalIntroParent.GetComponent<HorizontalLayoutGroup>().spacing = 22113;			//	horizontal
					_goalIntroParent.GetComponent<HorizontalLayoutGroup>().padding.left = -20450;	//	horizontal
					_goalIntroParent.GetComponent<HorizontalLayoutGroup>().padding.top = -2020;		//	horizontal
					_goalGameParent.GetComponent<VerticalLayoutGroup>().spacing = 7777;				//	vertical
					_goalGameParent.GetComponent<VerticalLayoutGroup>().padding.left = 25910;		//	Vertical
					break;
		}
		_goalIntroParent.GetComponent<Transform>().localScale = Vector3.one * 0.009f;
		_goalGameParent.GetComponent<Transform>().localScale = Vector3.one * 0.009f;
	}

	// Update is called once per frame
	public void UpdateGoals()
	{
		int _goalsCompleted = 0;
		for (int i = 0; i < _levelGoal.Length; i++)
		{
			_currentGoals[i]._thisText.text = "" +_levelGoal[i]._numberCollected + "/" + _levelGoal[i]._numberNeeded;
			if(_levelGoal[i]._numberCollected >= _levelGoal[i]._numberNeeded)
			{
				_goalsCompleted++;
				_currentGoals[i]._thisText.text = "" + _levelGoal[i]._numberNeeded + "/" + _levelGoal[i]._numberNeeded;
			}
		}

		if(_goalsCompleted >= _levelGoal.Length) 
		{ 
			print("You win !!!!");
			GameObject.Find("End Game Manager").GetComponent<EndGameManager>().WinGame();
		}
	}

	public void CompareGoal(string _goalToCompare)
	{
		for (int i = 0; i < _levelGoal.Length; i++)
		{
			if(_goalToCompare == _levelGoal[i]._matchValue) 
			{ 
				_levelGoal[i]._numberCollected++; 
			}
		}
	}
}