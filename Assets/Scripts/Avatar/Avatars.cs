using UnityEngine;

namespace Avatar
{
    [CreateAssetMenu(fileName = "Avatars", menuName = "Avatar/AvatarScript")]
    public class Avatars : ScriptableObject
    {
        public GameObject[] avatarsNetworkPrefab;
    }
}