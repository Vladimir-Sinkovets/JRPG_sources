using System;
using UnityEngine;

namespace Assets.Game.Scripts.Common.UI.Panels
{
    public abstract class PanelAnimation : MonoBehaviour
    {
        public abstract void Show(PanelBase panel, Action onPanelShowed);
        public abstract void Hide(PanelBase panel, Action onPanelHided);
    }
}
