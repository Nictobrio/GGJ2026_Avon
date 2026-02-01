using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Image _currentMaskImage;
    [SerializeField] private ChangeMaskButton[] _changeMaskButtons;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _titleScreenMenu;
    //[SerializeField] private GameObject _titleScreenCameraSystem;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _creditsMenu;
    [Header("Victory")]
    [SerializeField] private Image _victoryBackgroundImage;
    [SerializeField] private GameObject _victoryMenu;
    [Header("Defeat")]
    [SerializeField] private Image _defeatBackgroundImage;
    [SerializeField] private GameObject _defeatMenu;
    private static Color VictoryScreenFinalColor = new(0, 0, 0, 0.98f);
    private static Color DefeatScreenFinalColor = Color.black;

    #region Singleton
    public static UIManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject); //If there is already an instance of the PersistentSingleton, destroys the new one on Awake
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public void StartGame() {   //Called from a button
        _titleScreenMenu.SetActive(false);
        //if (_titleScreenCameraSystem != null)
        //    _titleScreenCameraSystem.SetActive(false);
        RestartGameplayUI();
        SceneManager.LoadScene("Gameplay");     //TO DO
    }

    private void RestartGameplayUI() {
        foreach (var button in _changeMaskButtons) {
            button.RestoreMask();
        }
        int randomMaskIndex = Random.Range(0, _changeMaskButtons.Length);
        ChangeMask(_changeMaskButtons[randomMaskIndex]);
        _gameplayUI.SetActive(true);
    }

    #region Masks
    public void ChangeMask(ChangeMaskButton changeMaskButton) {
        changeMaskButton.HideButton();
        for (int i = 0; i < _changeMaskButtons.Length; i++) {
            if (_changeMaskButtons[i].MaskSprite == _currentMaskImage.sprite) {
                if (!_changeMaskButtons[i].MaskIsBroken)
                    _changeMaskButtons[i].ShowButton();           //Show the button for the mask that is swapped out
                break;
            }
        }
        _currentMaskImage.sprite = changeMaskButton.MaskSprite;
    }

    public void DestroyCurrentMask() {
        foreach (var button in _changeMaskButtons) {
            if (button.MaskSprite == _currentMaskImage.sprite) {
                button.DestroyMask();
                break;
            }
        }

        //Set new mask
        foreach (var button in _changeMaskButtons) {
            if (!button.MaskIsBroken) {
                button.HideButton();
                _currentMaskImage.sprite = button.MaskSprite;
                return;
            }
        }

        //If the code reaches this part, it means there are no masks remaining
        Debug.Log("Defeat");
        ShowDefeatScreen();
    }
    #endregion

    #region VictoryAndDefeat

    public void ShowVictoryScreen() {
        _victoryBackgroundImage.color = Color.clear;
        _victoryBackgroundImage.gameObject.SetActive(true);
        StartCoroutine(ChangeImageColorOverTime(_victoryBackgroundImage, Color.clear, VictoryScreenFinalColor, 1.2f)); //Gradually turn the screen black
        StartCoroutine(ShowVictoryMenuCoroutine());
    }
    IEnumerator ShowVictoryMenuCoroutine() {
        yield return new WaitForSeconds(1.25f);
        _victoryMenu.SetActive(true);
    }

    public void ShowDefeatScreen() {
        _defeatBackgroundImage.color = Color.clear;
        _defeatBackgroundImage.gameObject.SetActive(true);
        StartCoroutine(ChangeImageColorOverTime(_defeatBackgroundImage, Color.clear, DefeatScreenFinalColor, 1.2f)); //Gradually turn the screen black
        StartCoroutine(ShowDefeatMenuCoroutine());
    }        
    IEnumerator ShowDefeatMenuCoroutine() {
        yield return new WaitForSeconds(1.25f);
        _defeatMenu.SetActive(true);
    }

    IEnumerator ChangeImageColorOverTime(Image image, Color startColor, Color endColor, float duration) {
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            float normalizedTime = t / duration;
            image.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
        image.color = endColor; //without this, the value could end at something very similar to the endColor, but not exactly it
    }

    public void RestartLevel() {    //Called from a button
        _victoryMenu.SetActive(false);
        _defeatMenu.SetActive(false);
        _victoryBackgroundImage.gameObject.SetActive(false);
        _defeatBackgroundImage.gameObject.SetActive(false);
        //TO DO     //Actually restart the level
        RestartGameplayUI();
    }

    #endregion

    public void OpenSettingsMenu() {    //Called from a button
        _settingsMenu.SetActive(true);
    }
    public void CloseSettingsMenu() {    //Called from a button
        _settingsMenu.SetActive(false);
    }

    public void ShowCredits() {    //Called from a button
        _creditsMenu.SetActive(true);
    }
    public void HideCreditsAndReturnToTitleScreen() {    //Called from a button
        _creditsMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");     //TO DO
    }

}
