using Assets.Game.Scripts.Common.Characters;
using Assets.Game.Scripts.BattleSystem.Unit;
using System;
using Assets.Game.Scripts.BattleSystem.Services.FormulasServices;
using Zenject;
using Assets.Game.Scripts.BattleSystem.Abilities.Base;

namespace Assets.Game.Scripts.BattleSystem.Unit
{
    public class UnitStats
    {
        #region events
        public event Action<UnitStats> OnDied;
        public event Action<UnitStats, int> OnHpDecrease; // int: change
        public event Action<UnitStats, int> OnHpIncrease; // int: change
        public event Action<UnitStats, int, int> OnHpChanged; // int: oldHpValue, int: newHpValue
        public event Action<UnitStats> OnMaxHpChanged;
        public event Action<UnitStats> OnTurnProgressChanged;
        #endregion

        private Stats _characterStats;
        private int _currentHp;

        // _characterStats.Hp - MaxHp 
        // _currentHp - Hp 

        private float _turnWaitingProgress = 0;
        private bool _isDead;

        [Inject] private FormulasService _formulasService;

        #region props
        public int MaxHp
        {
            get => _characterStats.Hp;
            set
            {
                _characterStats.Hp = value;
                if (_currentHp > _characterStats.Hp)
                {
                    var hp = _currentHp;

                    _currentHp = _characterStats.Hp;

                    OnHpChanged?.Invoke(this, hp, _currentHp);
                }

                OnMaxHpChanged?.Invoke(this);
            }
        }
        public int Hp
        {
            get => _currentHp;
            set
            {
                var oldHp = _currentHp;
                _currentHp = value;

                if (oldHp - _currentHp < 0)
                    OnHpIncrease?.Invoke(this, _currentHp - oldHp);

                if (oldHp - _currentHp >= 0)
                    OnHpDecrease?.Invoke(this, oldHp - _currentHp);

                OnHpChanged?.Invoke(this, oldHp, _currentHp);
            }
        }

        public int Speed { get => _characterStats.Speed; set => _characterStats.Speed = value; }

        public int Attack { get => _characterStats.Attack; set => _characterStats.Attack = value; }
        public int MagicalAttack { get => _characterStats.MagicalAttack; set => _characterStats.MagicalAttack = value; }
        public int Defence { get => _characterStats.Defence; set => _characterStats.Defence = value; }
        public int MagicalDefence { get => _characterStats.MagicalDefence; set => _characterStats.MagicalDefence = value; }

        public bool IsDead { get => _isDead; }
        public float TurnProgress
        {
            get => _turnWaitingProgress;
            private set
            {
                if (value <= 0)
                    _turnWaitingProgress = 0;
                else
                    _turnWaitingProgress = value;

                OnTurnProgressChanged?.Invoke(this);
            }
        }
        public float TurnWaitingSpeed => 1 + _characterStats.Speed;

        public BattleUnit Unit { get; private set; }
        #endregion

        public void Init(BattleUnit unit, Stats stats, FormulasService formulasService)
        {
            _characterStats = new Stats();

            _formulasService = formulasService;

            MaxHp = stats.Hp;
            Hp = stats.Hp;
            Speed = stats.Speed;

            Attack = stats.Attack;
            MagicalAttack = stats.MagicalAttack;
            Defence = stats.Defence;
            MagicalDefence = stats.MagicalDefence;

            Unit = unit;

            _isDead = false;
        }

        public void TakeDamage(int damage, DamageType type)
        {
            if (_isDead) return;

            var reducedDamage = _formulasService.ReduceDamageByType(Unit, type, damage);

            Hp -= reducedDamage;

            if (Hp <= 0)
                Die();
        }

        public void Heal(int heal)
        {
            if (_isDead) return;

            Hp += heal;

            if (Hp <= 0)
                Die();
        }

        public void UpdateTurnProgress(float time)
        {
            TurnProgress += TurnWaitingSpeed * time;
        }

        public void ClearTurnProgress()
        {
            TurnProgress = 0;
        }

        private void Die()
        {
            _isDead = true;

            OnDied?.Invoke(this);
        }
    }
}
