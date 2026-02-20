using Assets.Game.Scripts.Locations.Player;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Game.Scripts.Locations.Enemies
{
    public class EnemyStateMachineData
    {
        // ------------------ Links ------------------
        public NavMeshAgent NavMeshAgent { get; set; }
        public PlayerController PlayerController { get; set; }

        // ------------------ Patrol ------------------
        public Vector3 OriginalPosition { get; set; }
        public float PatrolRange { get; set; } = 10f;
        public float MinWaitingTime { get; set; } = 1f;
        public float MaxWaitingTime { get; set; } = 3f;
        public float VisionRange { get; set; }
        public float PatrolSpeed { get; set; }
        public float PatrolAcceleration { get; set; }
        public float AlertDelay { get; set; }
        public float ActivationRange { get; internal set; } = 20.0f;

        // ------------------ Chasing ------------------
        public float ChasingRange { get; set; } = 15f;
        public float ChasingSpeed { get; set; }
        public float ChasingAcceleration { get; set; }
        public float StoppingDistance { get; set; } = 0.3f;
        public Action OnCollision {  get; set; }
    }
}