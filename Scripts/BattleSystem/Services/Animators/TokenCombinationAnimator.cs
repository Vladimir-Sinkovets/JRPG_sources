using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Services.CameraFocusServices;
using Assets.Game.Scripts.BattleSystem.Unit;
using Assets.Game.Scripts.BattleSystem.Unit.UI;
using Assets.Game.Scripts.Common.Services.Audio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.BattleSystem.Services.Animators
{
    public class TokenCombinationAnimator : ITokenCombinationAnimator
    {
        [Inject] private TokensConfig _tokensConfig;
        //[Inject] private IVFXService _vfxService;
        [Inject] private IAudioService _audioService;
        [Inject] private ICameraFocusService _cameraFocusService;

        public IEnumerator AnimateCombination(IEnumerable<TokenIcon> comboIcons, BattleUnit targetUnit)
        {
            if (!comboIcons.Any())
                yield break;

            _cameraFocusService.SetTarget(targetUnit.transform);

            yield return new WaitForSeconds(1.0f);

            yield return HighlightComboTokens(comboIcons);

            yield return MergeTokensAnimation(comboIcons);

            yield return CreateMergeEffect(comboIcons);

            yield return PlayTargetEffect(targetUnit);

            yield return HideComboTokens(comboIcons);
        }

        private IEnumerator HighlightComboTokens(IEnumerable<TokenIcon> comboIcons)
        {
            var sequences = new List<Sequence>();

            foreach (var icon in comboIcons)
            {
                var seq = DOTween.Sequence();
                seq.Join(icon.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack));
                seq.Join(icon.transform.DOShakePosition(0.3f, strength: 5f, vibrato: 10));
                sequences.Add(seq);
            }

            foreach (var icon in comboIcons)
            {
                icon.EnableGlow(true);
            }

            _audioService.PlaySFX(_tokensConfig.HighlightComboSfx);

            yield return new WaitForSeconds(0.4f);
        }

        private IEnumerator MergeTokensAnimation(IEnumerable<TokenIcon> comboIcons)
        {
            Vector3 centerPoint = Vector3.zero;
            foreach (var icon in comboIcons)
            {
                centerPoint += icon.transform.position;
            }
            centerPoint /= comboIcons.Count();

            var moveSequences = new List<Sequence>();

            foreach (var icon in comboIcons)
            {
                var seq = DOTween.Sequence();
                seq.Append(icon.transform.DOMove(centerPoint, 0.5f).SetEase(Ease.InQuad));
                seq.Join(icon.transform.DOScale(0.7f, 0.5f));
                moveSequences.Add(seq);
            }

            yield return new WaitForSeconds(0.5f);

            _audioService.PlaySFX(_tokensConfig.MergeComboSfx);
        }

        private IEnumerator CreateMergeEffect(IEnumerable<TokenIcon> comboIcons)
        {
            Vector3 centerPoint = comboIcons.ElementAt(0).transform.position;

            //_vfxService.PlayVFX("token_merge", centerPoint);

            //_vfxService.PlayVFX(combination.VFXName, centerPoint);

            foreach (var icon in comboIcons)
            {
                icon.GetComponent<CanvasGroup>().DOFade(0, 0.2f).SetLoops(2, LoopType.Yoyo);
            }

            yield return new WaitForSeconds(0.4f);
        }

        private IEnumerator PlayTargetEffect(BattleUnit targetUnit)
        {
            //_vfxService.ShowFloatingText(combination.Name, targetUnit.transform.position + Vector3.up * 2, Color.yellow);

            //_vfxService.PlayVFX(combination.TargetVFXName, targetUnit.transform.position);

            //if (combination.ShakePower > 0)
            //{
            //    _vfxService.ShakeCamera(0.3f, 0.2f);
            //}

            yield return new WaitForSeconds(0.3f);
        }

        private IEnumerator HideComboTokens(IEnumerable<TokenIcon> comboIcons)
        {
            foreach (var icon in comboIcons)
            {
                var seq = DOTween.Sequence();
                seq.Append(icon.transform.DOScale(0f, 0.2f));
                seq.Join(icon.GetComponent<CanvasGroup>().DOFade(0f, 0.2f));
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

}
