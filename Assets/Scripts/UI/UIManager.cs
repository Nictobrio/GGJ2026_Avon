using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private Image _currentMaskImage;
    [SerializeField] private ChangeMaskButton[] _changeMaskButtons;

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
        //Game Over
        Debug.Log("Game Over");
    }

}
