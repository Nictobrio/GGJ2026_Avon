using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button {

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        PlayUIHighlightSound();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        base.OnPointerClick(eventData);
        EventSystem.current.SetSelectedGameObject(null);
        PlayUISubmitSound();
    }

    protected virtual void PlayUIHighlightSound() {
        AudioManager.Instance.PlayHighlightButtonSFX();
    }

    protected virtual void PlayUISubmitSound() {
        AudioManager.Instance.PlayPressButtonSFX();
    }

}
