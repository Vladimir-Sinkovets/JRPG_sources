using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Abilities.FX;
using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Effects.Factories;
using Assets.Game.Scripts.BattleSystem.Extensions;
using Assets.Game.Scripts.BattleSystem.Services.BattlePositions;
using Assets.Game.Scripts.BattleSystem.Tokens;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.Common.Services.Audio;
using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Game.Scripts.BattleSystem.Abilities
{
    public class UniversalAbility : BattleAbility
    {
        [ListDrawerSettings(Draggable = true, AlwaysExpanded = true)]
        [OnValueChanged(nameof(OnNumberOfTargetsChanged))]
        public List<TurnAction> TurnActions;

        public float ReturnToStartPositionTime;
        public Ease Ease;

        private Vector3 _startPosition;

        [Inject] private IAudioService _audioService;
        [Inject] private BattlePositionsData _battlePositionsData;

        public override string FullDescription
        {
            get
            {
                return _description + '\n' + GetDamageDescription() + '\n' + GetCostDescription(); ;
            }
        }

        private string GetDamageDescription()
        {
            var damageFormula = string.Empty;

            foreach (var action in TurnActions)
            {
                if (action.Type == TurnActionType.Attack)
                {
                    if (!string.IsNullOrEmpty(damageFormula))
                        damageFormula = "\n";

                    damageFormula = "{";

                    damageFormula += action.Attack.Dmg.ToString();
                    if (action.Attack.Coefs.Hp != 0) damageFormula += $" + {action.Attack.Coefs.Hp}% * HP";
                    if (action.Attack.Coefs.MagicalAttack != 0) damageFormula += $" + {action.Attack.Coefs.MagicalAttack}% * MATK";
                    if (action.Attack.Coefs.Attack != 0) damageFormula += $" + {action.Attack.Coefs.Attack}% * ATK";
                    if (action.Attack.Coefs.Defence != 0) damageFormula += $" + {action.Attack.Coefs.Defence} * DEF";
                    if (action.Attack.Coefs.MagicalDefence != 0)  damageFormula += $" + {action.Attack.Coefs.MagicalDefence}% * MDEF";
                    if (action.Attack.Coefs.Speed != 0)  damageFormula += $" + {action.Attack.Coefs.Speed}% * SPD";

                    damageFormula += $"}} - {action.Attack.DmgType.GetName()}";
                }
            }
            return damageFormula;
        }

        public override IEnumerator Execute(AbilityData data)
        {
            yield return HandleCost(data);

            _startPosition = data.Owner.transform.position;
            var wasMoved = false;

            foreach (var action in TurnActions)
            {
                yield return new WaitForSeconds(action.TimeBefore);

                if (action.Type == TurnActionType.Move)
                {
                    yield return HandleMove(data, action);
                    wasMoved = true;
                }
                else if (action.Type == TurnActionType.Attack)
                {
                    yield return HandleAttack(data, action);
                }
                else if (action.Type == TurnActionType.Heal)
                {
                    yield return HandleHeal(data, action);
                }
                else if (action.Type == TurnActionType.Neutral)
                {
                    yield return HandleNeutral(data, action);
                }

                yield return new WaitForSeconds(action.TimeAfter);
            }

            if (wasMoved)
            {
                yield return data.Owner.transform.DOMove(_startPosition, ReturnToStartPositionTime)
                    .SetEase(Ease)
                    .WaitForCompletion(); 
            }
        }

        private IEnumerator HandleNeutral(AbilityData data, TurnAction action)
        {
            var owner = data.Owner;
            var targets = data.Targets;

            if (!action.EffectsWithInterval)
            {
                yield return ApplyFX(owner, targets, action);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];

                if (action.EffectsWithInterval)
                {
                    yield return ApplyFX(owner, new() { target }, action);

                    yield return new WaitForSeconds(action.EffectsInterval);
                }

                if (action.SideEffects.HasEffect && action.SideEffects.Effect != null)
                    target.Effects.AddEffect(action.SideEffects.Effect.GetEffect(target, owner));

                if (action.SideEffects.HasToken)
                    target.Tokens.Add(action.SideEffects.Token);
            }
        }

        private IEnumerator HandleHeal(AbilityData data, TurnAction action)
        {
            var owner = data.Owner;
            var targets = data.Targets;

            if (!action.HealWithInterval)
            {
                yield return ApplyFX(owner, targets, action);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];

                if (action.HealWithInterval)
                {
                    yield return ApplyFX(owner, new() { target }, action);

                    yield return new WaitForSeconds(action.HealInterval);
                }

                var heal = GetHeal(owner, action.Heal);

                target.Stats.Heal(heal);

                if (action.Heal.SideEffects.HasEffect && action.Heal.SideEffects.Effect != null)
                    target.Effects.AddEffect(action.Heal.SideEffects.Effect.GetEffect(target, owner));

                if (action.Heal.SideEffects.HasToken)
                    target.Tokens.Add(action.Heal.SideEffects.Token);
            }
        }
        private IEnumerator HandleAttack(AbilityData data, TurnAction action)
        {
            var owner = data.Owner;
            var targets = data.Targets;

            if (!action.AttackWithInterval)
            {
                yield return ApplyFX(owner, targets, action);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];

                if (action.AttackWithInterval)
                {
                    yield return ApplyFX(owner, new() { target }, action);

                    yield return new WaitForSeconds(action.AttackInterval);
                }

                var damage = GetDamage(owner, action.Attack);

                target.Stats.TakeDamage(damage, action.Attack.DmgType);

                if (action.Attack.SideEffects.HasEffect && action.Attack.SideEffects.Effect != null)
                    target.Effects.AddEffect(action.Attack.SideEffects.Effect.GetEffect(target, owner));

                if (action.Attack.SideEffects.HasToken)
                    target.Tokens.Add(action.Attack.SideEffects.Token);
            }
        }

        private IEnumerator ApplyFX(BattleUnit owner, List<BattleUnit> targets, TurnAction action)
        {
            InstantiateParticles(owner, action.OwnerParticles);

            _audioService.PlaySFX(action.AttackAudioClip);

            if (action.AttackFeedback != null)
                action.AttackFeedback.PlayFeedbacks();

            if (action.OwnerParticles != null)
                yield return new WaitForSeconds(action.OwnerParticles.Delay);

            InstantiateParticles(targets, action.TargetParticles);

            if (action.TargetParticles != null)
                yield return new WaitForSeconds(action.TargetParticles.Delay);

            _audioService.PlaySFX(action.HitAudioClip);

            if (action.HitFeedback != null)
                action.HitFeedback.PlayFeedbacks();

            //_cameraShakeService.Shake(action.ScreenShakeProfile);
        }

        private IEnumerator HandleMove(AbilityData data, TurnAction action)
        {
            var middlePosition = data.BattlePositionsData.MiddlePosition.position;

            if (action.MoveType == TurnActionMoveType.None)
            {
            }
            else if (action.MoveType == TurnActionMoveType.MiddlePosition)
            {
                yield return data.Owner.transform.DOMove(middlePosition, action.MovingDuration).WaitForCompletion();
            }
            else if (action.MoveType == TurnActionMoveType.StartPosition)
            {
                yield return data.Owner.transform.DOMove(_startPosition, action.MovingDuration).WaitForCompletion();
            }
            else if (action.MoveType == TurnActionMoveType.Custom)
            {
                var position = _battlePositionsData.MiddlePosition.position + action.Position;

                var tween = data.Owner.transform.DOMove(position, action.MovingDuration);

                if (action.UseCurve)
                    tween.SetEase(action.Curve);
                else
                    tween.SetEase(action.Ease);

                yield return tween.WaitForCompletion();
            }
        }

        private int GetHeal(BattleUnit owner, TurnActionHealSettings heal)
        {
            return heal.Heal + StatsCoefficients.CalculateDamage(heal.Coefs, owner.Stats);
        }

        private int GetDamage(Unit.BattleUnit owner, TurnActionAttackSettings attack)
        {
            return attack.Dmg + StatsCoefficients.CalculateDamage(attack.Coefs, owner.Stats);
        }

        private void InstantiateParticles(List<BattleUnit> targets, ParticlesFX particleSystemPrefab)
        {
            if (particleSystemPrefab == null)
                return;

            var particles = new List<ParticlesFX>();

            foreach (var target in targets)
            {
                if (target == null || target.IsDestroyed())
                    continue;

                var particle = CreateParticleInstance(target, particleSystemPrefab);
                particles.Add(particle);
            }
        }

        private void InstantiateParticles(BattleUnit target, ParticlesFX particleSystemPrefab)
        {
            var targets = new List<BattleUnit> { target };

            InstantiateParticles(targets, particleSystemPrefab);
        }

        private ParticlesFX CreateParticleInstance(BattleUnit target, ParticlesFX prefab)
        {
            var instance = Instantiate(prefab);
            instance.transform.position = target.SpriteRenderer.bounds.center + new Vector3(0, 0, -0.2f);
            instance.Play();
            return instance;
        }
    }

    [Serializable]
    [DeclareBoxGroup("Action")]
    [DeclareBoxGroup("Action/Attack", Title = "Attack")]
    [DeclareBoxGroup("Action/Heal", Title = "Heal")]
    [DeclareHorizontalGroup("Action/Time")]
    [DeclareHorizontalGroup("Action/AttackInterval")]
    [DeclareHorizontalGroup("Action/HealInterval")] 
    [DeclareHorizontalGroup("Action/SideEffectsInterval")]
    [DeclareBoxGroup("Action/SideEffects", HideTitle = true)]
    [DeclareBoxGroup("Action/fx", Title = "FX")]
    public class TurnAction
    {
        [Group("Action")]
        [EnumToggleButtons]
        [HideLabel]
        public TurnActionType Type;

        [Group("Action/Time")]
        public float TimeBefore;
        [Group("Action/Time")]
        public float TimeAfter;

        #region -------------------------------- Move --------------------------------

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        public TurnActionMoveType MoveType;

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        [ShowIf(nameof(MoveType), TurnActionMoveType.Custom)]
        public Vector3 Position;

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        [ShowIf(nameof(MoveType), TurnActionMoveType.Custom)]
        public bool UseCurve = false;

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        [ShowIf(nameof(MoveType), TurnActionMoveType.Custom)]
        [ShowIf(nameof(UseCurve), true)]
        public AnimationCurve Curve;

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        [ShowIf(nameof(MoveType), TurnActionMoveType.Custom)]
        [ShowIf(nameof(UseCurve), false)]
        public Ease Ease;

        [Group("Action")]
        [ShowIf(nameof(Type), TurnActionType.Move)]
        public float MovingDuration;

        #endregion


        #region -------------------------------- Attack --------------------------------

        [Group("Action/AttackInterval")]
        [ShowIf(nameof(Type), TurnActionType.Attack)]
        [LabelWidth(120)]
        public bool AttackWithInterval;

        [Group("Action/AttackInterval")]
        [ShowIf(nameof(Type), TurnActionType.Attack)]
        [ShowIf(nameof(AttackWithInterval))]
        [LabelWidth(120)]
        public float AttackInterval;

        [InlineProperty]
        [Group("Action/Attack")]
        [ShowIf(nameof(Type), TurnActionType.Attack)]
        [HideLabel]
        public TurnActionAttackSettings Attack;

        #endregion


        #region -------------------------------- Neutral --------------------------------

        [Group("Action/SideEffectsInterval")]
        [ShowIf(nameof(Type), TurnActionType.Neutral)]
        [LabelWidth(120)]
        public bool EffectsWithInterval;

        [Group("Action/SideEffectsInterval")]
        [ShowIf(nameof(Type), TurnActionType.Neutral)]
        [ShowIf(nameof(EffectsWithInterval), true)]
        [LabelWidth(120)]
        public float EffectsInterval;

        [Group("Action/SideEffects")]
        [InlineProperty]
        [ShowIf(nameof(Type), TurnActionType.Neutral)]
        [HideLabel]
        public SideEffects SideEffects;

        #endregion


        #region -------------------------------- Heal --------------------------------

        [Group("Action/HealInterval")]
        [ShowIf(nameof(Type), TurnActionType.Heal)]
        [LabelWidth(120)]
        public bool HealWithInterval;

        [Group("Action/HealInterval")]
        [ShowIf(nameof(Type), TurnActionType.Heal)]
        [ShowIf(nameof(HealWithInterval))]
        [LabelWidth(120)]
        public float HealInterval;

        [InlineProperty]
        [Group("Action/Heal")]
        [ShowIf(nameof(Type), TurnActionType.Heal)]
        [HideLabel]
        public TurnActionHealSettings Heal;

        #endregion


        #region -------------------------------- Effects (common) --------------------------------

        //[Group("Action/fx")]
        //[HideIf(nameof(Type), TurnActionType.Move)]
        //public ScreenShakeProfile ScreenShakeProfile;

        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public ParticlesFX OwnerParticles;

        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public AudioClip AttackAudioClip;

        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public MMF_Player AttackFeedback;

        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public ParticlesFX TargetParticles;


        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public AudioClip HitAudioClip;

        [Group("Action/fx")]
        [HideIf(nameof(Type), TurnActionType.Move)]
        public MMF_Player HitFeedback;
        #endregion
    }


    [Serializable]
    [DeclareBoxGroup("SideEffects", HideTitle = true)]
    public class TurnActionAttackSettings
    {
        public bool Random = false;

        [Range(0, 100)]
        [HideLabel]
        [HideIf(nameof(Random), false)]
        [Unit("%")]
        public int RandomDeviation = 10;

        [LabelWidth(70)]
        public int Dmg;

        [LabelWidth(70)]
        public DamageType DmgType;

        [LabelWidth(70)]
        [InlineProperty]
        public StatsCoefficients Coefs;

        [Group("SideEffects")]
        [InlineProperty]
        [HideLabel]
        public SideEffects SideEffects;
    }

    [Serializable]
    [DeclareBoxGroup("SideEffects", HideTitle = true)]
    public class TurnActionHealSettings
    {
        public bool Random = false;

        [Range(0, 100)]
        [HideLabel]
        [HideIf(nameof(Random), false)]
        [Unit("%")]
        public int RandomDeviation = 10;

        [LabelWidth(70)]
        public int Heal;

        [LabelWidth(70)]
        [InlineProperty]
        public StatsCoefficients Coefs;

        [Group("SideEffects")]
        [InlineProperty]
        [HideLabel]
        public SideEffects SideEffects;
    }

    [Serializable]
    [DeclareHorizontalGroup("Token")]
    [DeclareHorizontalGroup("Effect")]
    public class SideEffects
    {

        [Group("Token")]
        [LabelWidth(70)]
        public bool HasToken = true;

        [Group("Token")]
        [HideLabel]
        [ShowIf(nameof(HasToken))]
        public Token Token;

        [Group("Effect")]
        [LabelWidth(70)]
        public bool HasEffect = false;

        [Group("Effect")]
        [HideLabel]
        [ShowIf(nameof(HasEffect))]
        public BattleEffectFactory Effect;
    }

    public enum TurnActionMoveType
    {
        None,
        MiddlePosition,
        StartPosition,
        Custom,
    }

    public enum TurnActionType
    {
        Move,
        Attack,
        Heal,
        Neutral,
    }
}
