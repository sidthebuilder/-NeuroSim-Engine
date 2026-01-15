using UnityEngine;
using UnityEngine.AI;

namespace CharacterModel {

    /// <summary>
    /// Handles the physical movement of the character using Unity's NavMesh system.
    /// Acts as the "Legs" for the AI Brain.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMotor : MonoBehaviour {

        private NavMeshAgent agent;

        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Vector3 targetPosition) {
            if (agent != null && agent.isOnNavMesh) {
                agent.SetDestination(targetPosition);
                agent.isStopped = false;
            }
        }

        public void Stop() {
            if (agent != null && agent.isOnNavMesh) {
                agent.isStopped = true;
            }
        }

        public bool HasReachedDestination(float tolerance = 0.5f) {
            if (agent == null) return true;
            if (agent.pathPending) return false;
            
            return agent.remainingDistance <= tolerance;
        }

        public void SetSpeed(float speed) {
            if (agent != null) agent.speed = speed;
        }
    }
}
