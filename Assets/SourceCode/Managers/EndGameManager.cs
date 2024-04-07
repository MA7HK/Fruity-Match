using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{	Moves,
	Time
}

[System.Serializable]
public class EndGameRequirement
{
	public GameType _gameType;
	public int _value;
}

public class EndGameManager : MonoBehaviour
{
	public GameObject _youWinPanel;
	public GameObject _youLosePanel;
	public GameObject _moveLabel;
	public GameObject _timeLable;
	public Text _counter;
	
	public EndGameRequirement _requirements;
	private Board _board;
	private float _timeSecond;
	public int _currentCounterValue;

	public bool isTimerActive = false;
	// Start is called before the first frame update
	void Start()
	{
		_board = FindObjectOfType<Board>();
		SetGameType();
		SetupGame();
	}

	void SetGameType()
	{
		if(_board._world != null)
		{
			if(_board._level < _board._world.levels.Length)
			{
				if(_board._world.levels[_board._level] != null)
				{
					_requirements = _board._world.levels[_board._level].endGameRequirement;
				}
			}
		}
	}

	void SetupGame()
	{
		_currentCounterValue = _requirements._value;
		if(_requirements._gameType == GameType.Moves)
		{
			_moveLabel.SetActive(true);
			_timeLable.SetActive(false);
		}
		else
		{
			_timeSecond = 1;
			_moveLabel.SetActive(false);
			_timeLable.SetActive(true);
		}
		_counter.text = _currentCounterValue.ToString();

		_youLosePanel.SetActive(false);
		_youWinPanel.SetActive(false);
	}
	public void DecreaseCounterValue()
	{	
		_currentCounterValue--;
		_counter.text = _currentCounterValue.ToString();
		if(_currentCounterValue == 0) { StartCoroutine(YouLose()); }	
	}

	public void WinGame() 
	{ 
		_youWinPanel.SetActive(true); 
		_board._currentState = GameState.win;
		_currentCounterValue = 0;
		_counter.text = _currentCounterValue.ToString();
	}

	public IEnumerator YouLose()
	{
		yield return new WaitForSeconds(1.5f);
		_youLosePanel.SetActive(true);
		_board._currentState = GameState.lose;
		print("You Lose !!");
		_currentCounterValue = 0;
		_counter.text = _currentCounterValue.ToString();
	}

	public void StartTimer()
	{
		if(!isTimerActive) isTimerActive = true; 
	}

	// Update is called once per frame
	void Update()
	{
		if(_requirements._gameType == GameType.Time && _currentCounterValue > 0) 
		{ 
			if(isTimerActive)
			{
				_timeSecond -= Time.deltaTime;
				if(_timeSecond <= 0) 
				{
					DecreaseCounterValue();
					_timeSecond = 1;
				}
			}
		}
	}
}