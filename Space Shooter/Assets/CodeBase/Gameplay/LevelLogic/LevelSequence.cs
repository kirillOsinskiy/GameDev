using UnityEngine;

namespace SpaceShooter
{
    [CreateAssetMenu]
    public class LevelSequence : ScriptableObject
    {
        public LevelProperties[] Levels;
    }
}