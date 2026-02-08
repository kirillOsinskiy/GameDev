using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelBoundary : SingletonBase<LevelBoundary>
    {
        /// <summary>
        /// –адиус игрового мира
        /// </summary>
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        public enum BoundaryMode
        {
            Limit, // ограничение движени€
            Teleport // перенос игрока на противоположный край пол€
        }

        [SerializeField] private BoundaryMode m_LimitMode;
        public BoundaryMode LimitMode => m_LimitMode;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}