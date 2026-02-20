using Assets.Game.Scripts.BattleSystem.Abilities.Base;
using Assets.Game.Scripts.BattleSystem.Configs;
using Assets.Game.Scripts.BattleSystem.Data;
using Assets.Game.Scripts.Common.Configs.PlayerAssetsAccesser;
using Assets.Game.Scripts.Common.Services.SceneLoading;
using Assets.Game.Scripts.Locations.Extensions;
using Assets.Game.Scripts.Locations.PlayerData;
using Assets.Game.Scripts.Locations.PlayerData.InventoryData;
using Assets.Game.Scripts.Locations.PlayerData.PartyData;
using PixelCrushers;
using System;
using System.Linq;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

public class BattleStarter : MonoBehaviour
{
    [Inject]
    private IBattleSceneLoader _battleSceneLoader;

    [SerializeField] private string _battleId;

    [SerializeField] private CharacterConfig[] _enemyConfigs;

    [SerializeField] private UnityEvent OnBattleLost;

    [Inject] private Inventory _inventory;
    [Inject] private Party _party;
    [Inject] private PlayerStats _playerStats;
    [Inject] private BattleAssets _battleAssets;

    [Inject(Optional = true)] private BattleResults _battleResults;

    [Button("Generate id")]
    private void GenerateId()
    {
        _battleId = Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (_battleResults == null)
            return;

        var s = SaveSystem.currentSavedGameData.Dict
            .Select(x => $"{x.Key}: {x.Value.data}")
            .Aggregate((x, y) => x + y + "\n");

        if (_battleResults.Id == _battleId)
        {
            OnBattleLost?.Invoke();
        }
    }

    public void StartBattle()
    {
        var abilities = _inventory.GetEquipmentAbilities()
            .ToList();

        var summonAbilities = _party.GetSummonAbilities();

        abilities.AddRange(summonAbilities);

        var stats = _playerStats.Stats + _inventory.GetEquipmentStatsBonus();

        _battleSceneLoader.LoadBattle(new BattleArgs()
        {
            EnemyTeamConfigs = _enemyConfigs.Select(x => new CharacterArgs()
            {
                Abilities = x.Abilities,
                Prefab = x.Prefab,
                Stats = x.Stats.Copy(),
                BehaviourConfig = x.Behaviour,
            }).ToArray(),

            PlayerTeamConfigs = new CharacterArgs[]
            {
                new CharacterArgs()
                {
                    Abilities = abilities,
                    Prefab = _battleAssets.PlayerPrefab,
                    Stats = stats.Copy(),
                }
            },

            ReturnSceneName = SceneManager.GetActiveScene().name,
            Id = _battleId,
        });
    }
}
