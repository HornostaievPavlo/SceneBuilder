using UnityEngine;

public class LoadingIconRotator : MonoBehaviour
{
    public float speed;

    private RectTransform image;

    private void Awake() => image = GetComponent<RectTransform>();

    private void Update() => RotateImage();

    private void RotateImage()
    {
        image.eulerAngles -= new Vector3(0, 0, speed * Time.deltaTime);
    }

    private void OnDisable() => image.eulerAngles = Vector3.zero;
}
