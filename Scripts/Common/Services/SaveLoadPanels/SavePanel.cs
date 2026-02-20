using Assets.Game.Scripts.Common.SaveData;
using Assets.Game.Scripts.Common.UI.Panels;
using PixelCrushers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Assets.Game.Scripts.Common.Services.SaveLoadPanels
{
    public class SavePanel : MonoBehaviour
    {
        [SerializeField] private List<SaveSlot> _slots;
        [SerializeField] private PanelBase _panel;
        [Space]
        [SerializeField] private GameObject _savingPanel;

        [Inject] private GameMetaDataSaver _gameMetaDataSaver;

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
            StartCoroutine(SaveCoroutine(index));
        }

        private IEnumerator SaveCoroutine(int index)
        {
            _savingPanel.SetActive(true);

            _panel.UnSelect();

            _panel.LastSelected = EventSystem.current.currentSelectedGameObject;

            EventSystem.current.SetSelectedGameObject(null);

            yield return null;
            
            yield return new WaitForEndOfFrame();

            _gameMetaDataSaver.TakeScreenshot();
            
            SaveSystem.SaveToSlot(index);
            
            yield return null;

            _savingPanel.SetActive(false);

            _panel.Select();

            UpdateSlots();
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
