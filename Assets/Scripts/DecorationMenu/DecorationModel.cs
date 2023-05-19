using System;
using UnityEngine;

namespace DefaultNamespace.DecorationMenu
{
    [CreateAssetMenu(menuName = "Decoration Object")]
    public class DecorationModel : ScriptableObject
    {
        public DModel[] models;
    }

    [Serializable]
    public struct DModel
    {
        public int pointCost;
        public string modelName;
        public Sprite modelSprite;
        public GameObject modelPrefab;
    }
}