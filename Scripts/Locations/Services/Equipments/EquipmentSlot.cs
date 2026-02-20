using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using Assets.Game.Scripts.Locations.Services.InventoryPanel;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.Equipments
{
    public class EquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentSlot> OnClicked;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Image _icon;
        [SerializeField] private StatsPanel _statsPanel;
        [SerializeField] private AbilityListView _abilityListView;
        [Space]
        [SerializeField] private Image _borders;
        [SerializeField] private GameObject _slotGameObject;
        [SerializeField] private float _scaleAmount = 1.2f;
        [SerializeField] private float _animationDuration = 0.25f;

        private Vector3 _originalScale;
        private Color _originalColor;

        private InventorySlot _slot;

        public EquipmentSlotType EquipmentSlotType { get; set; }
        public InventorySlot InventorySlot { get => _slot; }

        private void Awake()
        {
            _originalScale = _slotGameObject.transform.localScale;

            _originalColor = _borders.color;
        }

        public void SetSlot(InventorySlot slot)
        {
            if (slot == null)
            {
                Clear();
                return;
            }

            _slot = slot;

            _title.text = slot.Item.Name;
            _icon.sprite = slot.Item.Sprite;
            _icon.color = Color.white;

            _statsPanel.SetStats(slot.Item.Stats);
            _abilityListView.SetAbilities(slot.Item.Abilities);
        }

        public void Clear()
        {
            _slot = null;

            _title.text = string.Empty;
            _icon.sprite = null;
            _icon.color = new Color(0, 0, 0, 0);

            _statsPanel.Clear();
            _abilityListView.Clear();

            _slotGameObject.transform.localScale = _originalScale;
            _borders.color = _originalColor;
        }

        public void OnSlotClicked() => OnClicked?.Invoke(this);

        public void OnSelectedHandler()
        {
            DOTween.Kill(_slotGameObject.transform);
            DOTween.Kill(_borders);

            _slotGameObject.transform.DOScale(_originalScale * _scaleAmount, _animationDuration)
                .SetEase(Ease.OutBack);

            _borders.DOColor(Color.white, _animationDuration);
        }

        public void OnDeselectedHandler()
        {
            DOTween.Kill(_slotGameObject.transform);
            DOTween.Kill(_borders);

            Sequence deselectSequence = DOTween.Sequence();

            deselectSequence.Append(_slotGameObject.transform.DOScale(_originalScale, _animationDuration)
                            .SetEase(Ease.InBack));

            deselectSequence.Join(_borders.DOColor(_originalColor, _animationDuration));
        }
    }
}
