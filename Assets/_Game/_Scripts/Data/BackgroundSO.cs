using System;
using UnityEngine;

namespace _Game._Scripts.Data
{
    [CreateAssetMenu(menuName = "BackgroundSO",fileName = "BackgroundSO",order = 0)]
    public class BackgroundSO : ScriptableObject
    {
        public BackgroundInfo[] backgrounds;
    }

    [Serializable]
    public struct BackgroundInfo
    {
        public Sprite  icon;
        public Sprite  sprite;
        public Vector2 resolution;
    }
}