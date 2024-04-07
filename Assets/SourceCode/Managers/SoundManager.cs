using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager soundfx;

	[Header("Audio Clips")]
	public AudioClip[] _destroyAudioClip;
    public AudioClip _collapsedClip;
    public AudioClip _buttonClickClip;

    [Header("Audio Source")]
	public AudioSource _audioSource;
    public AudioSource _backgroundAudioSource;
    public AudioSource _woodAudioSource;
    public AudioSource _collapseAudioSource;
    public AudioSource _rockAudioSource;


    void Awake()
    {
        if (soundfx == null)
        {
            soundfx = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start() 
    {
        if (!_backgroundAudioSource.isPlaying)
        {
            int musicState = PlayerPrefs.GetInt("Music", 1);
            _backgroundAudioSource.Play();
            _backgroundAudioSource.volume = musicState == 0 ? 0 : 0.5f;
        }
    } 

    void Update()
    {
        if(!_backgroundAudioSource.isPlaying)
        {
            _backgroundAudioSource.Play();
        }
        ControlVolume();
    }

    public void ControlVolume()
    {
        int musicState = PlayerPrefs.GetInt("Music", 1);
        _backgroundAudioSource.volume = musicState == 0 ? 0 : 0.5f;
    }

	public void PlayRandomDestorySound()
	{
        int sfxState = PlayerPrefs.GetInt("Sound", 1);
        if (sfxState == 1)
        {
            int randNumber = Random.Range(0, _destroyAudioClip.Length);
            _audioSource.PlayOneShot(_destroyAudioClip[randNumber]);
        }
	}

    public void PlayButtonClickSound() => PlaySound(_audioSource, _buttonClickClip);
    public void PlayCollapsedSound() => PlaySound(_collapseAudioSource, _collapsedClip, volume: 0.1f);
    public void PlayWoodBreakingSound(AudioClip audioClip) => PlaySound(_woodAudioSource, audioClip, volume: 0.5f);
    public void PlayRockBreakingSound(AudioClip audioClip) => PlaySound(_rockAudioSource, audioClip);

    private void PlaySound(AudioSource audioSource, AudioClip audioClip, float volume = 1.0f)
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            int soundState = PlayerPrefs.GetInt("Sound");
            if (soundState == 1)
            {
                audioSource.PlayOneShot(audioClip);
                audioSource.volume = volume;
            }
        }
        else
        {
            audioSource.PlayOneShot(audioClip);
            audioSource.volume = volume;
        }
    }

}