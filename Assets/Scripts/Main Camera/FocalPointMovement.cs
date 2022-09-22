using UnityEngine;

public class FocalPointMovement : MonoBehaviour
{
    [SerializeField] private float movingSensitivity;

    void Update()
    {
        MoveFocalPoint();
    }

    private void MoveFocalPoint() // on left button and ctrl pressed - moving of camera focal point in space
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            var mousePositionX = Input.GetAxis("Mouse X") * movingSensitivity * Time.deltaTime;
            var mousePositionY = Input.GetAxis("Mouse Y") * movingSensitivity * Time.deltaTime;

            transform.Translate(new Vector3(-mousePositionX, -mousePositionY, 0) * (Time.deltaTime * movingSensitivity));
        }
    }
}