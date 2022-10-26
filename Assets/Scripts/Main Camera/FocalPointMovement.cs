using UnityEngine;

public class FocalPointMovement : MonoBehaviour
{
    [Tooltip("Responsiveness of focal point movement")]
    [SerializeField]
    private float _movingSensitivity;

    private void Update()
    {
        MoveFocalPoint();
    }

    /// <summary>
    /// Moves camera focal point according to direction of mouse movement
    /// Works on both LCTRL and LMB are pressed
    /// </summary>
    private void MoveFocalPoint()
    {
        int leftMouseButton = 0;

        if (Input.GetMouseButton(leftMouseButton) && Input.GetKey(KeyCode.LeftControl))
        {
            float mousePositionX = Input.GetAxis("Mouse X") * _movingSensitivity * Time.deltaTime;
            float mousePositionY = Input.GetAxis("Mouse Y") * _movingSensitivity * Time.deltaTime;

            Vector3 movingVector = new Vector3(-mousePositionX, -mousePositionY, 0);

            transform.Translate(movingVector * _movingSensitivity * Time.deltaTime);
        }
    }
}