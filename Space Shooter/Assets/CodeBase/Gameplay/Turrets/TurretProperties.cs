using Common;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Вид турели: главная или вторичная.
    /// </summary>
    public enum TurretMode
    {
        Primary,
        Secondary
    }

    [CreateAssetMenu]
    public sealed class TurretProperties : TurretPropertiesBase
    {
        /// <summary>
        /// Вид турели
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;        

    }
}