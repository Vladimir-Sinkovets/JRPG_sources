using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using Assets.Game.Scripts.Common.Configs.PlayerAssetsAccesser;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.UnitFactory
{
    public class BattleUnitFactory : IBattleUnitFactory
    {
        [Inject] private DiContainer _container;
        [Inject] private BattleAssets _battleAssets;

        public BattleUnit SpawnUnit(CharacterArgs config, Vector3 position, bool isEnemy)
        {
            var (unit, unitUI, view) = InstantiateUnit(config, position);

            if (config.BehaviourConfig != null)
            {
                var behaviour = config.BehaviourConfig.CreateBehavior(unit, _container);

                var behaviourAbilities = behaviour.InstantiateAbilities(unit, _container);

                unit.Init(new BattleUnitInitArgs()
                {
                    Stats = config.Stats,
                    Container = _container,
                    IsEnemy = isEnemy, 
                    Abilities = behaviourAbilities,
                    Behaviour = behaviour,
                    Canvas = unitUI.gameObject,
                    EffectsUIContainer = unitUI.EffectUIContainer,
                    View = view,
                    Tokens = unitUI.Tokens,
                });
            }
            else
            {
                var abilities = CreateAbilities(config, unit);

                unit.Init(new BattleUnitInitArgs()
                {
                    Stats = config.Stats,
                    Container = _container,
                    IsEnemy = isEnemy,
                    Abilities = abilities,
                    Behaviour = null,
                    Canvas = unitUI.gameObject,
                    EffectsUIContainer = unitUI.EffectUIContainer,
                    View = view,
                    Tokens = unitUI.Tokens,
                });
            }

            unit.transform.position = position;

            return unit;
        }

        private (BattleUnit unit, UnitUI unitUI, GameObject view) InstantiateUnit(CharacterArgs config, Vector3 position)
        {
            var unit = _container.InstantiateComponentOnNewGameObject<BattleUnit>(config.Prefab.name);

            var unitView = _container.InstantiatePrefab(config.Prefab, Vector3.zero, Quaternion.identity, unit.transform);

            var unitHeight = unitView.GetComponent<SpriteRenderer>().bounds.size.y;

            unitView.transform.position += new Vector3(0, unitHeight / 2, 0);

            var floatingDamage = _container
                .InstantiatePrefabForComponent<FloatingDamageMediator>(
                _battleAssets.FloatingDamage,
                new Vector3(0, unitHeight, 0),
                Quaternion.identity,
                unit.transform);

            var unitUI = _container
                .InstantiatePrefabForComponent<UnitUI>(
                _battleAssets.UnitUI,
                new Vector3(0, unitHeight, 0),
                Quaternion.identity,
                unit.transform);

            var canvasHeight = unitUI.Canvas.rect.height * unitUI.Canvas.localScale.y;

            unitUI.transform.position += new Vector3(0, canvasHeight / 2, 0);

            unitUI.Init(unit);
            floatingDamage.Init(unit);
            return (unit, unitUI, unitView);
        }

        private IEnumerable<BattleAbility> CreateAbilities(CharacterArgs config, BattleUnit unit)
        {
            var abilities = new List<BattleAbility>();

            foreach (var abilityPrefab in config.Abilities)
            {
                var ability = _container.InstantiatePrefab(abilityPrefab, unit.transform)
                    .GetComponent<BattleAbility>();

                abilities.Add(ability);
            }

            return abilities;
        }
    }
}
