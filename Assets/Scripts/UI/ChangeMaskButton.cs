using UnityEngine;
using UnityEngine.UI;

public class ChangeMaskButton : CustomButton {
    [SerializeField] private Image _maskImage;
    public Sprite MaskSprite { get => _maskImage.sprite; }
    public bool MaskIsBroken { get; private set; }

    public void ChangeCurrentMask() {   //Called from a button
        UIManager.Instance.ChangeMask(this);
    }

    public void ShowButton() {
        gameObject.SetActive(true);
    }
    public void HideButton() {
        gameObject.SetActive(false);
    }
    public void DestroyMask() {
        MaskIsBroken = true;
        HideButton();
    }
    public void RestoreMask() {
        MaskIsBroken = false;
        ShowButton();
    }

}
