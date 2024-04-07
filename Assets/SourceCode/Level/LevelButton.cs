using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
	public bool _isActive;
	public GameObject _activeObject;
	public GameObject _LockedObject;
	private int _starsActive;

	[Header("UI")]
	public Image[] _stars;
	public Text _levelText;
	public int _level;
	public SurePanel surePanel;

	[Header("Buttons")]
	public Button _confirmButton;

	private GameData _gamedata;

	void Start()
	{
		//		get component
		_gamedata = FindObjectOfType<GameData>();

		//		Button Evenets
		if(_gamedata._saveData._isActive[_level -1])
		{
			_confirmButton.onClick.AddListener(ConfirmPanel);
		}

		//		Functions
		LoadData();
		ActivateStars();
		ShowLevel();
		DecideSprite();
	}

	void LoadData()
	{
		// 	is gamedata present
		if(_gamedata != null)
		{
			//	decide if the level is active
			if(_gamedata._saveData._isActive[_level -1])
			{
				_isActive = true;
			}else _isActive = false;

			// 	decide how many stars to activate
			_starsActive = _gamedata._saveData._stars[_level - 1];
		}
	}

	void ActivateStars()
	{
		for (int i = 0; i < _starsActive; i++)
		{

			_stars[i].enabled = true;
		}
	}

	void DecideSprite()
	{
		if(_isActive)
		{
			_activeObject.SetActive(true);
			_LockedObject.SetActive(false);
			_levelText.enabled = true;
		}
		else
		{
			_activeObject.SetActive(false);
			_LockedObject.SetActive(true);
			_levelText.enabled = false;
		}
	}

	void ShowLevel()
	{
		_levelText.text = _level.ToString();
	}

	public void ConfirmPanel()
	{	
		//_confirmPanel.GetComponent<SurePanel>()._level = _level;
		//_confirmPanel.SetActive(true);

		//SurePanel surePanel = FindObjectOfType<SurePanel>();
		surePanel.SelectLevel(_level);
		surePanel.gameObject.SetActive(true);
	}
}