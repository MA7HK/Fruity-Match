using UnityEngine;

public class LevelSelectedManager : MonoBehaviour
{
	public GameObject[] _panels;
	public GameObject _currentPanel;
	public int _page;
	public int _currentLevel = 0;
	private GameData _gameData;

	// Start is called before the first frame update
	void Start()
	{
		_gameData = FindObjectOfType<GameData>();

		for (int i = 0; i < _panels.Length; i++)
		{
			_panels[i].SetActive(false);
		}
		if(_gameData != null)
		{
			for (int i = 0; i < _gameData._saveData._isActive.Length; i++)
			{
				if(_gameData._saveData._isActive[i]) { _currentLevel = i; }
			}
		}
		_page = (int)Mathf.Floor(_currentLevel / 12);
		_currentPanel = _panels[_page];
		_panels[_page].SetActive(true);
	}

	public void PageRight()
	{
		if(_page < _panels.Length - 1)
		{
			_currentPanel.SetActive(false);
			_page++;
			_currentPanel = _panels[_page];
			_currentPanel.SetActive(true);
		}
	}

	public void PageLeft()
	{
		if(_page > 0)
		{
			_currentPanel.SetActive(false);
			_page--;
			_currentPanel = _panels[_page];
			_currentPanel.SetActive(true);
		}
	}
}