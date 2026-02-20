using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Effects.Factories;
using Assets.Game.Scripts.BattleSystem.Tokens;
using Assets.Game.Scripts.BattleSystem.Unit;
using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Game.Scripts.BattleSystem.Abilities
{
    public class Hit : BattleAbility
    {
        [SerializeField] private Token _token;
        [Header("Time intervals")]
        [SerializeField] private float MovingForwardDuration = 0.9f;
        [SerializeField] private float PauseBeforeDamage = 0.6f;
        [SerializeField] private float PauseAfterDamage = 0.5f;
        [SerializeField] private float MovingBackwardDuration = 0.9f;
        [SerializeField] private float PauseBeforeEnd = 0.6f;

        [Space]
        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _baseDamage;

        [Space]
        [SerializeField] private bool _hasEffect;
        [SerializeField] private BattleEffectFactory _effectFactory;

        [Space]
        [SerializeField] private ParticleSystem _particles;

        private Guid _id;
        public Token Token { get => _token; }
        public override IEnumerator Execute(AbilityData data)
        {
            if (AreTargetsEmpty(data.Targets))
            {
                End(data);
                yield break;
            }

            var owner = data.Owner;
            var middlePosition = data.BattlePositionsData.MiddlePosition.position;
            var startPosition = owner.transform.position;

            yield return owner.transform.DOMove(middlePosition, MovingForwardDuration).WaitForCompletion();

            yield return new WaitForSeconds(PauseBeforeDamage);

            var damage = _baseDamage + data.Owner.Stats.Attack;

            MakeDamage(data.Targets, data.Owner, damage, _damageType);

            yield return new WaitForSeconds(PauseAfterDamage);

            yield return owner.transform.DOMove(startPosition, MovingBackwardDuration).WaitForCompletion();

            yield return new WaitForSeconds(PauseBeforeEnd);

            //DestroyParticles(list);

            End(data);

            yield break;
        }

        private bool AreTargetsEmpty(List<BattleUnit> targets)
        {
            foreach (var unit in targets)
            {
                if (unit != null && unit.IsDestroyed() == false)
                    return false;
            }

            return true;
        }

        private void MakeDamage(IEnumerable<BattleUnit> targets, BattleUnit owner, int damage, DamageType damageType)
        {
            foreach (var target in targets)
            {
                if (target == null || target.IsDestroyed())
                    continue;

                target.Stats.TakeDamage(damage, damageType);

                if (_hasEffect)
                    target.Effects.AddEffect(_effectFactory.GetEffect(target, owner, _id));

                target.Tokens.Add(Token);
            }
        }
    }
}
