using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSettings : MonoBehaviour {
    [Tooltip("The main Audio Mixer")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [Tooltip("The AudioSource that is played whenever the SFX volume or the Master volume is modified.")]
    [SerializeField] private AudioSource _sfxVolumeSample;
    [SerializeField] private float _volumeSampleCooldownTimeSeconds;
    private bool _enabledVolumeSample;
    private float _disableVolumeSampleUntilTime;

    private void Start() {
        _masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_MASTER_VOLUME, AudioManager.DEFAULT_VOLUME);
        _musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_MUSIC_VOLUME, AudioManager.DEFAULT_VOLUME);
        _sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_SFX_VOLUME, AudioManager.DEFAULT_VOLUME);
        ChangeMasterVolume(_masterSlider.value);
        ChangeMusicVolume(_musicSlider.value);
        ChangeSFXVolume(_sfxSlider.value);
        _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        _sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }
    private void Update() {
        if (!_enabledVolumeSample && Time.time > _disableVolumeSampleUntilTime) {
            _enabledVolumeSample = true;
        }
    }

    private void DisableVolumeSampleMomentarily() {
        _disableVolumeSampleUntilTime = Time.time + _volumeSampleCooldownTimeSeconds;
        _enabledVolumeSample = false;
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MASTER_VOLUME, _masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC_VOLUME, _musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_SFX_VOLUME, _sfxSlider.value);
        _masterSlider.onValueChanged.RemoveListener(ChangeMasterVolume);
        _musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        _sfxSlider.onValueChanged.RemoveListener(ChangeSFXVolume);
    }

    private void ChangeMasterVolume(float value) {
        _mixer.SetFloat(AudioManager.MIXER_MASTER_VOLUME, Mathf.Log10(value) * AudioManager.VOLUME_MULTIPLIER);
        TryToPlaySFXVolumeSample();
    }

    private void ChangeMusicVolume(float value) {
        _mixer.SetFloat(AudioManager.MIXER_MUSIC_VOLUME, Mathf.Log10(value) * AudioManager.VOLUME_MULTIPLIER);
    }
    private void ChangeSFXVolume(float value) {
        _mixer.SetFloat(AudioManager.MIXER_SFX_VOLUME, Mathf.Log10(value) * AudioManager.VOLUME_MULTIPLIER);
        TryToPlaySFXVolumeSample();
    }

    private void TryToPlaySFXVolumeSample() {
        if (!_enabledVolumeSample)
            return;
        if (_sfxVolumeSample != null && _sfxVolumeSample.isActiveAndEnabled) {
            DisableVolumeSampleMomentarily();
            //_sfxVolumeSample.Play();          //TO DO       //Uncomment when the SFX is added
        }
    }

}
