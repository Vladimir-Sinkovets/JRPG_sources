using Assets.Game.Scripts.Common.SaveData;
using PixelCrushers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Common.Services.SaveLoadPanels
{
    public class SaveSlot : MonoBehaviour
    {
        private const string EmptySlotText = "Empty";
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _sceneName;
        [SerializeField] private TMP_Text _time;
        
        private int _index;
        private Sprite _currentSprite;

        public event Action<int> OnClicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickHandler);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickHandler);
        }

        public void SetSaveSlot(int index)
        {
            _index = index;

            if (SaveSystem.HasSavedGameInSlot(index))
            {
                var savedGameData = SaveSystem.storer.RetrieveSavedGameData(index);
                var saveData = savedGameData.GetData("GameMetaData");
                var gameMetaData = SaveSystem.Deserialize<GameMetaData>(saveData);

                if (gameMetaData != null)
                {
                    SetSprite(gameMetaData.Picture);

                    _number.text = index.ToString();
                    _sceneName.text = gameMetaData.SceneName;
                    _time.text = gameMetaData.TimeStamp;
                }
                else
                {
                    _number.text = index.ToString();
                    _sceneName.text = EmptySlotText;
                }
            }
            else
            {
                _number.text = index.ToString();
                _sceneName.text = EmptySlotText;
            }
        }

        private void SetSprite(byte[] picture)
        {
            var texture = new Texture2D(2, 2);

            ClearPreviousSprite();

            if (texture.LoadImage(picture))
            {
                _currentSprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                _image.sprite = _currentSprite;
            }
            else
            {
                Debug.LogWarning($"Save {_index}, creating texture error");

                _image.sprite = null;
            }
        }

        private void ClearPreviousSprite()
        {
            if (_currentSprite != null)
            {
                Destroy(_currentSprite);
                _currentSprite = null;
            }
        }
        private void OnClickHandler()
        {
            OnClicked?.Invoke(_index);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}