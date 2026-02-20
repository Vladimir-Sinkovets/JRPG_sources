using Assets.Game.Scripts.Common.Characters;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Locations.Services.PauseManagers
{
    public class PlayerCharacterDataItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        [SerializeField] private TMP_Text _stats;

        [SerializeField] private GameObject _container;

        public void Show(Stats character)
        {
            _container.SetActive(true);

            _stats.text = string.Empty;

            _stats.text += $"Hp - {character.Hp}\n";
            _stats.text += $"Attack - {character.Attack}\n";
            _stats.text += $"Magic attack - {character.MagicalAttack}\n";
            _stats.text += $"Defence - {character.Defence}\n";
            _stats.text += $"Magic defence - {character.MagicalDefence}\n";
        }

        public void Hide()
        {
            _container.SetActive(false);
        }
    }
}