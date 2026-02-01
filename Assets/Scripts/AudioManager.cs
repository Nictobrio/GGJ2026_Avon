using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    [Tooltip("The main Audio Mixer")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _highlightButtonSFX;
    [SerializeField] private AudioSource _pressButtonSFX;
    public const float VOLUME_MULTIPLIER = 20f;
    public const float DEFAULT_VOLUME = 0.6f;
    public const string MIXER_MASTER_VOLUME = "MasterVolume";
    public const string MIXER_MUSIC_VOLUME = "MusicVolume";
    public const string MIXER_SFX_VOLUME = "SFXVolume";

    #region Singleton
    public static AudioManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject); //If there is already an instance of the PersistentSingleton, destroys the new one on Awake
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public void PlayHighlightButtonSFX() {
        _highlightButtonSFX.Play();           //TO DO       //Uncomment when the SFX is added
    }

    public void PlayPressButtonSFX() {
        _pressButtonSFX.Play();               //TO DO       //Uncomment when the SFX is added
    }

    public void PlayMusic(AudioClip clip) {
        _musicSource.clip = clip;
        _musicSource.Play();
    }
    public void PlayMusic(AudioClip clip, float volume) {
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }

}
