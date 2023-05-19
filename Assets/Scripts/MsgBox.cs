using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class MsgBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameTmp;
        [SerializeField] private TMP_Text msgTmp;

        public string senderName
        {
            set => playerNameTmp.text = value;
        }

        public string msgText
        {
            set => msgTmp.text = value;
        }
    }
}