using UnityEngine;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] avatars;

    private int _index;

    private void Start()
    {
        foreach (var avatar in avatars)
        {
            avatar.SetActive(false);
        }

        _index = PlayerPrefs.HasKey("AvatarKey") ? PlayerPrefs.GetInt("AvatarKey") : 0;
        avatars[_index].SetActive(true);
    }

    public void ChangeNext(bool right)
    {
        avatars[_index].SetActive(false);
        if (right) _index++;
        else _index--;
        if (_index < 0) _index = avatars.Length - 1;
        if (_index >= avatars.Length) _index = 0;
        avatars[_index].SetActive(true);
        PlayerPrefs.SetInt("AvatarKey", _index);
        PlayerPrefs.Save();
    }
}
