using UnityEngine;
using UnityEngine.UI;

public class SaveFilePanel : MonoBehaviour
{
    public int currentNumber;

    public RawImage preview => GetComponentInChildren<RawImage>();

    public Button loadButton => GetComponentsInChildren<Button>()[0];
    public Button deleteButton => GetComponentsInChildren<Button>()[1];
}
