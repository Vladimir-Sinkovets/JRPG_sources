using Assets.Game.Scripts.Common.SaveData;
using Assets.Game.Scripts.Common.UI.Panels;
using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Game.Scripts.Common.Services.SaveLoadPanels
{
    public class LoadPanel : MonoBehaviour
    {
        [SerializeField] private List<SaveSlot> _slots;

        [SerializeField] private PanelBase _panel;
        [Space]
        [SerializeField] private GameObject _loadingPanel;

        private void Start()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].OnClicked += SaveSlotClicked;
            }
        }

        public void Open() => UpdateSlots();

        public void Close() { }

        private void UpdateSlots()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SetSaveSlot(i);
            }
        }

        private void SaveSlotClicked(int index)
        {
            if (SaveSystem.HasSavedGameInSlot(index))
            {
                StartCoroutine(SaveCoroutine(index));
            }
        }

        private IEnumerator SaveCoroutine(int index)
        {
            _loadingPanel.SetActive(true);

            _panel.UnSelect();

            EventSystem.current.SetSelectedGameObject(null);

            yield return null;

            SaveSystem.LoadFromSlot(index);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].OnClicked -= SaveSlotClicked;
            }
        }
    }
}
