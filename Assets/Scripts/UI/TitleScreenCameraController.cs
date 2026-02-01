using UnityEngine;

public class TitleScreenCameraController : MonoBehaviour {
    [SerializeField] float _rotationSpeed;

    void Update() {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }

}
