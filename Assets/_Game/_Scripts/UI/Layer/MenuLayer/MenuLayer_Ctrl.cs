using UnityEngine;
using VInspector;

namespace _Game._Scripts.UI.Layer.MenuLayer
{
    public class MenuLayer_Ctrl : LayerBase
    {
        [Tab("Menu Layer")]
        [Foldout("Reference")]
        [SerializeField]
        private Transform _parentCardSelection;

        [EndFoldout]
        [Foldout("Asset")]
        [SerializeField]
        private CardSelectionItem _cardPrefab;
    }
}