using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private PlayerInput.ActionEvent onSceneStart;

    [Header("Profile Setup")] [SerializeField]
    private TMP_InputField nameInput;
    

    private void Start()
    {
        onSceneStart.Invoke(default);
        if (PlayerPrefs.HasKey("UserName"))
        {
            var userName = PlayerPrefs.GetString("UserName");
            nameInput.text = userName;
        }
    }

    public void StartClass()
    {
        SceneManager.LoadScene("ClassRoom");
    }

    public void SaveNameInput()
    {
        PlayerPrefs.SetString("UserName", nameInput.text);
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}