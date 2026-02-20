using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.Locations.Services.InventoryPanel
{
    public class AbilityView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;

        public void SetAbility(BattleAbility ability)
        {
            _icon.sprite = ability.Sprite;
            _name.text = ability.Name;
            _description.text = ability.FullDescription;
        }
    }
}