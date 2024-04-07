using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurePanel : MonoBehaviour
{
    [Header("Level Info")]
    public string _levelToLoad;
    public int _level;
    private GameData _gamedata;
	public int _starsActive;
    private int _highScore;

    [Header("UI")]
    public Image[] _stars;
    public Text _highScoreText;

    [Header("Buttons")]
    public Button _playButton;
    public Button _backButton;

    void OnEnable()
    {
        // Get the component
        _gamedata = FindObjectOfType<GameData>();

        // Button events
        _playButton.onClick.AddListener(OnPressPlayButton);
        _backButton.onClick.AddListener(OnPressCancelButton);

		for (int i = 0; i < 3; i++)
		{
			_stars[i].enabled = false;
		}

        // Functions
        LoadData();
        ActivateStars();
        SetText();
    }

    public void SelectLevel(int level)
    {
        _level = level;
        LoadData();
        ActivateStars();
        SetText();
    }

    void LoadData()
    {
        if(_gamedata != null)
        {
            _starsActive = _gamedata._saveData._stars[_level -1];
            _highScore = _gamedata._saveData._highScores[_level -1];
        }
    }

    void SetText()
    {
        _highScoreText.text = _highScore.ToString();
    }

    void ActivateStars()
    {	
		for (int i = 0; i < _starsActive; i++)
		{
			_stars[i].enabled = true;
		}
    }

    void OnPressCancelButton() => gameObject.SetActive(false);

    void OnPressPlayButton()
    {
        PlayerPrefs.SetInt("currentLevel", _level -1);
        SceneManager.LoadScene(_levelToLoad);
    }
}
