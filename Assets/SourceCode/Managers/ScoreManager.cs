using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text _scoreText;
    public int _score;
    public Image _fillerBar;
    private Board _board;
    private GameData gameData;
    private int _numberofStars;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        _board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update() => _scoreText.text = _score.ToString();

    public void IncreaseScore(int _amountToIncrease)
    {
        _score += _amountToIncrease;
        for (int i = 0; i < _board._scoreGoals.Length; i++)
        {
            if(_score > _board._scoreGoals[i] && _numberofStars < i + 1) { _numberofStars++; }
        }

        if(gameData != null)
        {
            int highScore = gameData._saveData._highScores[_board._level];
            if(_score > highScore)
            {
                gameData._saveData._highScores[_board._level] =_score;
            }
            int _currentStars = gameData._saveData._stars[_board._level];
            if(_numberofStars > _currentStars)
            {
                gameData._saveData._stars[_board._level] = _numberofStars;
            }
			if(_score > 500)
            {
                if(_board._level > 36 )
                {
                    //if(_board._level < 36) gameData._saveData._isActive[_board._level + 1] = true;
                    //else 
                    gameData._saveData._isActive[_board._level + 1] = true;
                }
            } 
            gameData.Save();
        }

        UpdateFillBar();
    }

    void UpdateFillBar()
    {
        //  update bar
        if (_board != null && _fillerBar != null)
        {
            int _length = _board._scoreGoals.Length;
            _fillerBar.fillAmount = (float)_score / _board._scoreGoals[_length - 1];
        }
    }
    
}
