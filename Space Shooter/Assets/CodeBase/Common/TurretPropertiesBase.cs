using UnityEngine;

namespace Common
{
    [CreateAssetMenu]
    public abstract class TurretPropertiesBase : ScriptableObject
    {
        /// <summary>
        /// Ссылка на префаб снаряда
        /// </summary>
        [SerializeField] private ProjectileBase m_ProjectilePrefab;
        public ProjectileBase ProjectilePrefab => m_ProjectilePrefab;

        /// <summary>
        /// Скорострельность турели. Чем меньше, тем быстрее стреляет.
        /// Задержка в секундах между выстрелами.
        /// </summary>
        [SerializeField] private float m_RateOfFire;
        public float RateOfFire => m_RateOfFire;

        /// <summary>
        /// Сколько потребляет энергии
        /// </summary>
        [SerializeField] private int m_EnergyUsage;
        public int EnergyUsage => m_EnergyUsage;

        /// <summary>
        /// Сколько потребляет патронов
        /// </summary>
        [SerializeField] private int m_AmmoUsage;
        public int AmmoUsage => m_AmmoUsage;

        /// <summary>
        /// Ссылка на звук, который воспроизводится, при запуске снаряда
        /// </summary>
        [SerializeField] private AudioClip m_LaunchSFX;
        public AudioClip LaunchSFX => m_LaunchSFX;
    }
}