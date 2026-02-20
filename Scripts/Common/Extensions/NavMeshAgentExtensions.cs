using UnityEngine;
using UnityEngine.AI;

namespace Assets.Game.Scripts.Common.Extensions
{
    public static class NavMeshAgentExtensions
    {
        public static Vector3 GetMovementDirection(this NavMeshAgent agent)
        {
            var velocity = agent.velocity;

            if (velocity.magnitude > 0.1f)
            {
                var normalizedVelocity = velocity.normalized;

                var localVelocity = agent.transform.InverseTransformDirection(normalizedVelocity);

                return localVelocity;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}