using UnityEngine;

public class ConstantRectRotation : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    [SerializeField] private float speed = 150f;

    private void OnEnable() => image.eulerAngles = Vector3.zero;
    
    private void Update() => image.eulerAngles -= new Vector3(0, 0, speed * Time.deltaTime);
}
