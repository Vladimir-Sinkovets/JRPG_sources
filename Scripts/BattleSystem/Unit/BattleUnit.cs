using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Configs.Behaviours;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Services.FormulasServices;
using Assets.Game.Scripts.BattleSystem.Services.UIPrefabs;
using Assets.Game.Scripts.Common.Characters;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Unit
{
    public class BattleUnit : MonoBehaviour
    {
        private UnitTokens _tokens;
        private GameObject _view;
        private GameObject _canvas;
        private RectTransform _effectsUIContainer;
        private SpriteRenderer _renderer;

        private UnitStats _stats = new();
        private UnitEffects _effects = new();
        private UnitAbilities _abilities = new(); // should be created here. if not it will break hpbar

        private DiContainer _container;

        private bool _isEnemy;

        private EnemyBehaviour _behaviour;

        [Inject] private readonly BattlePositionsData _battlePositionsData;

        #region props
        public UnitTokens Tokens { get => _tokens; }
        public UnitStats Stats { get => _stats; }
        public UnitEffects Effects { get => _effects; }
        public UnitAbilities Abilities { get => _abilities; }

        public bool IsEnemy { get => _isEnemy; }
        public EnemyBehaviour Behaviour { get => _behaviour; }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                _renderer = _renderer != null ? _renderer : _view.GetComponent<SpriteRenderer>();

                return _renderer;
            }
        }

        #endregion

        public void Init(BattleUnitInitArgs args)
        {
            _container = args.Container;
            _isEnemy = args.IsEnemy;
            _behaviour = args.IsEnemy ? args.Behaviour : null;
            _view = args.View;
            _canvas = args.Canvas;
            _effectsUIContainer = args.EffectsUIContainer;
            _tokens = args.Tokens;

            _stats.Init(
                unit: this,
                args.Stats,
                formulasService: _container.Resolve<FormulasService>());

            _effects.Init(
                battleUIPrefabs: _container.Resolve<BattleUIPrefabs>(),
                _effectsUIContainer);

            _abilities.Init(
                battlePositionsData: _container.Resolve<BattlePositionsData>(),
                args.Abilities,
                unit: this);

            _stats.OnDied += OnDiedHandler;
        }
        
        public void UpdateTurnWaitingProgress(float time)
        {
            _stats.UpdateTurnProgress(time);
        }

        private void OnDiedHandler(UnitStats _)
        {
            _view.SetActive(false);
            _canvas.SetActive(false);

            _battlePositionsData.RemovePositionHolder(this);

            _stats.OnDied -= OnDiedHandler;
        }
    }

    public class BattleUnitInitArgs
    {
        public Stats Stats;
        public DiContainer Container;
        public bool IsEnemy;
        public IEnumerable<BattleAbility> Abilities;
        public EnemyBehaviour Behaviour;

        public GameObject View;
        public GameObject Canvas;
        public RectTransform EffectsUIContainer;
        public UnitTokens Tokens;
    }
}