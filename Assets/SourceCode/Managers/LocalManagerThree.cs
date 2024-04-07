using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocalManagerThree : MonoBehaviour
{
	public Button _homeButton;
    public Button _rightPageButton;
    public Button _leftPageButton;
    GameData gamedata;

    void Start() => SetEventOnButtons();

    void SetEventOnButtons() 
    {
        _homeButton.onClick.AddListener(OnPressedHomeButton);
        _rightPageButton.onClick.AddListener(OnPressedRightPageButton);
        _leftPageButton.onClick.AddListener(OnPressedLeftPageButton);
    }

    private void OnPressedHomeButton() => SceneManager.LoadScene("Main_Menu");
    private void OnPressedRightPageButton()
    {
        LevelSelectedManager level_manager = FindObjectOfType<LevelSelectedManager>();
        level_manager.PageRight();
    }
    private void OnPressedLeftPageButton() => FindObjectOfType<LevelSelectedManager>().PageLeft();

}