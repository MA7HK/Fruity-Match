using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalManagerOne : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject _startPage;
    public GameObject _settingPage;
    
    [Header("Buttons")]
    public Button _musicButton;
    public Button _sfxButton;
    public Button _quitButton;
    public Button _infoButton;

    [Header("Images")]
    public Sprite[] _musicOnOffImage;
    public Sprite[] _sfxOnOffImage;

    void Start() 
    {
        _settingPage.SetActive(false);
        Time.timeScale = 1.0f;
        SetEventButtons();
        SetMusicState();
        SetSfxState();
    }

    void SetEventButtons()
    {
        _musicButton.onClick.AddListener(OnPressedMusicButton);
        _sfxButton.onClick.AddListener(OnPressedSfxButton);
        _quitButton.onClick.AddListener(OnPressedQuitButton);
    }

    void SetMusicState()
    {
        int musicState = PlayerPrefs.GetInt("Music", 1);
        _musicButton.image.sprite = _musicOnOffImage[musicState == 0 ? 1 : 0];
    }

    void OnPressedMusicButton()
    {
        int musicState = PlayerPrefs.GetInt("Music", 1);
        PlayerPrefs.SetInt("Music", musicState == 0 ? 1 : 0);
        _musicButton.image.sprite = _musicOnOffImage[musicState == 0 ? 0 : 1];
    }

    void SetSfxState()
    {
        int sfxState = PlayerPrefs.GetInt("Sound", 1);
        _sfxButton.image.sprite = _sfxOnOffImage[sfxState == 0 ? 1 : 0];
    }

    void OnPressedSfxButton()
    {
        int sfxState = PlayerPrefs.GetInt("Sound", 1);
        PlayerPrefs.SetInt("Sound", sfxState == 0 ? 1 : 0);
        _sfxButton.image.sprite = _sfxOnOffImage[sfxState == 0 ? 0 : 1];
    }

    public void OnPressedPlayButton() => SceneManager.LoadScene(2);
    public void OnPressedSettingButton() => _settingPage.SetActive(true);
    public void OnPressedBackButton() => _settingPage.SetActive(false);
    public void OnPressedQuitButton() => Application.Quit(0);

}
