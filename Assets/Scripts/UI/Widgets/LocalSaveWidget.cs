using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class LocalSaveWidget : MonoBehaviour
    {
        public int currentNumber;

        public RawImage preview => GetComponentInChildren<RawImage>();

        public Button loadButton => GetComponentsInChildren<Button>()[0];
        public Button deleteButton => GetComponentsInChildren<Button>()[1];
    }
}
