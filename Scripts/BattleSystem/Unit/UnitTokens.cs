using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Tokens;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Unit
{
    public class UnitTokens : MonoBehaviour
    {
        [SerializeField] private TokenIcon _prefab;
        [SerializeField] private RectTransform _tokensContainer;

        [Inject] private TokensConfig _tokensConfig;

        private List<TokenIcon> _icons = new();

        private BattleUnit _unit;

        public BattleUnit Unit { get => _unit; }
        public IEnumerable<TokenIcon> Collection { get => _icons; }

        public void SetUnit(BattleUnit unit) => _unit = unit;

        public void Add(Token token)
        {
            var icon = Instantiate(_prefab, _tokensContainer);

            _icons.Add(icon);

            var data = _tokensConfig.Data.FirstOrDefault(x => x.Token == token);

            icon.SetToken(token);
            icon.SetImage(data.Icon);
        }

        private void RemoveTokens(int index, int length)
        {
            for (int i = index; i < index + length; i++)
            {
                Destroy(_icons[i].gameObject);
            }

            _icons.RemoveRange(index, length);
        }

        public void RemoveTokens(IEnumerable<TokenIcon> tokens)
        {
            foreach (var token in tokens)
            {
                _icons.Remove(token);

                Destroy(token.gameObject);
            }
        }
    }
}
