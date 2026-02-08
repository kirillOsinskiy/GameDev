using UnityEngine;

namespace SpaceShooter
{

    public abstract class LevelCondition : MonoBehaviour
    {
        public abstract bool IsCompleted { get; }
    }
}