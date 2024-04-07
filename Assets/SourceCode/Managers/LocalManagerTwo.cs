using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class LocalManagerTwo : MonoBehaviour
{
	public static LocalManagerTwo localManager;
	public GameObject _pausePanel;
	public GameObject _goalPanel;

	private GameData _gamedata;
	private Board _board;

	//		Buttons
	public Button _pauseButton;
	public Button _resumeButton;
	public Button[] _restartButton;
	public Button[] _mapButton;
	public Button[] _homeButton;
	public Button _nextButton;
	public Button _okButton;


	void Awake() { localManager ??= this; }  
	
	// Start is called before the first frame update
	void Start()
	{
		_gamedata = FindObjectOfType<GameData>();
		_board = FindObjectOfType<Board>();
		_pausePanel.active = false;
		SetEventOnButtons();

	}

	void SetEventOnButtons()
	{
		_pauseButton.onClick.AddListener(OnPressedPauseButton);

		_resumeButton.onClick.AddListener(OnPressedResumeButton);

		_restartButton[0].onClick.AddListener(OnPressedRestartButton);
		_restartButton[1].onClick.AddListener(OnPressedRestartButton);
		_restartButton[2].onClick.AddListener(OnPressedRestartButton);

		_homeButton[0].onClick.AddListener(OnPressedHomeButton);
		_homeButton[1].onClick.AddListener(OnPressedHomeButton);
		_homeButton[2].onClick.AddListener(OnPressedHomeButton);

		_mapButton[0].onClick.AddListener(OnPressedMapButton);
		_mapButton[1].onClick.AddListener(OnPressedMapButton);

		_nextButton.onClick.AddListener(OnPressedNextButton);
		_okButton.onClick.AddListener(OnPressedOkButton);
	}

	private void OnPressedPauseButton() { _pausePanel.SetActive(true);  Time.timeScale = 0.0f; }
	private void OnPressedResumeButton() { _pausePanel.SetActive(false); Time.timeScale = 1.0f; }
	private void OnPressedHomeButton() { SceneManager.LoadScene("Main_Menu"); }
	private void OnPressedRestartButton() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); Time.timeScale = 1.0f;}
	private void OnPressedMapButton() { SceneManager.LoadScene("Main_Map"); }
	private void OnPressedOkButton() 
	{ 
		GameObject.Find("Score Panel").GetComponent<Image>().raycastTarget = false;
		GameObject.Find("Board").GetComponent<Animator>().Play("GoSmall");
		GameObject.Find("Farmer").GetComponent<Animator>().Play("SideOut");
		StartCoroutine(DeactiveGoalPanel());
		FindObjectOfType<EndGameManager>().GetComponent<EndGameManager>().StartTimer();
	}

	private void OnPressedNextButton()
	{
		if(_gamedata != null)
		{
			_gamedata._saveData._isActive[_board._level + 1] = true;
			_gamedata.Save();
		}
		SceneManager.LoadScene("Main_Map");
	}

	IEnumerator DeactiveGoalPanel() 
	{ 
		yield return new WaitForSeconds(0.2f);
		FindObjectOfType<Board>()._currentState = GameState.move;
		_goalPanel.SetActive(false); 
	}

}