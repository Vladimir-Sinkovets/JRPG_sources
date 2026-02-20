using DG.Tweening;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.DialogueEffectsServices
{
    public class DialogueSoundEffects : MonoBehaviour
    {
        [SerializeField] private AudioSource _backgroundSound;

        [SerializeField] private SoundPreset[] _soundPresets;

        private AudioSource _audioSource;

        private void Awake() => _audioSource = GetComponent<AudioSource>();

        public void SetBackgroundPreset(double index)
        {
            _backgroundSound.volume = _soundPresets[(int)index].Volume;

            _backgroundSound.DOPitch(_soundPresets[(int)index].Pitch, 1);
        }

        #region Lua registration
        void OnEnable()
        {
            Lua.RegisterFunction(nameof(SetBackgroundPreset), this, SymbolExtensions.GetMethodInfo(() => SetBackgroundPreset(0)));
        }

        void OnDisable()
        {
            Lua.UnregisterFunction(nameof(SetBackgroundPreset));
        }
        #endregion
    }

    [Serializable]
    public class SoundPreset
    {
        public float Volume;
        public float Pitch;
    }
}
