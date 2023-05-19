using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchPaidKeyboard : MonoBehaviour
{
    [SerializeField] private Transform keyboardTrans;

    private TMP_InputField[] _inputFields;
    private TMP_InputField _activeInputField;

    private void Start()
    {
        _inputFields = transform.GetComponentsInChildren<TMP_InputField>(true);
        foreach (var inputField in _inputFields)
        {
            inputField.onSelect.AddListener(arg0 =>
            {
                _activeInputField = inputField;
                keyboardTrans.gameObject.SetActive(true);
            });

            // inputField.onDeselect.AddListener(arg0 =>
            // {
            //     _activeInputField = null;
            //     keyboardTrans.gameObject.SetActive(false);
            // });
        }

        var keys = keyboardTrans.GetComponentsInChildren<Button>();
        for (var i = 0; i < keys.Length; i++)
        {
            var text = keys[i].GetComponentInChildren<TMP_Text>();
            keys[i].onClick.AddListener(() =>
            {
                if (_activeInputField == null) return;
                switch (text.text)
                {
                    case "delete":
                        _activeInputField.text = _activeInputField.text.Substring(0, _activeInputField.text.Length - 1);
                        break;
                    case "space":
                        _activeInputField.text += " ";
                        break;
                    default:
                        _activeInputField.text += text.text;
                        break;
                }
            });
        }
    }
}