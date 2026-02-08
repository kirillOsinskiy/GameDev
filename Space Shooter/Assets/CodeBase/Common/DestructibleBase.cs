using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То, что может иметь хитпоинты.
    /// </summary>
    public abstract class DestructibleBase : Entity
    {
        #region Properties

        [Header("Destructible")]
        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое количество хитпоинтов.
        /// </summary>
        [SerializeField] private int m_HitPointsStart;
        public int MaxHitPoints => m_HitPointsStart;

        /// <summary>
        /// Визуальный эффект при уничтожении
        /// </summary>
        [SerializeField] private GameObject m_ExplosionPrefab;

        /// <summary>
        /// Звук при уничтожении
        /// </summary>
        [SerializeField] private AudioClip m_DestructionSFX;

        /// <summary>
        /// Текущее количество хитпоинтов.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPointsStart;

            transform.SetParent(null);
        }

        #endregion

        #region Public API

        /// <summary>
        /// Применение урона к объекту.
        /// </summary>
        /// <param name="damage">Урон наносимый объекту</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();
            }
        }

        public void DestroyIt()
        {
            ApplyDamage(m_CurrentHitPoints);
        }

        /// <summary>
        /// Воспроизвести какой-либо звук (например, подбираемого бонуса)
        /// </summary>
        /// <param name="clip"></param>
        public void PlaySFXSound(AudioClip clip)
        {
            if (clip == null) return;

            AudioSource source = gameObject.GetComponent<AudioSource>();

            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.rolloffMode = AudioRolloffMode.Logarithmic;
                source.maxDistance = 10;
                source.volume = 0.25f;
            }

            source.PlayOneShot(clip);
        }

        public void SetIndestructible(bool indestructible)
        {
            m_Indestructible = indestructible;
        }

        #endregion

        /// <summary>
        /// Событие уничтожения объекта, когда хитпоинты ниже нуля.
        /// </summary>
        private void OnDeath()
        {
            OnDeathSpecialities();
            DestroyEffects();
            Destroy(gameObject);
            m_EventOnDeath?.Invoke(this);
        }

        /// <summary>
        /// Метод для переопределения. Задаёт особое поведение при уничтожении. 
        /// Для астероидов "расаклывает" их на несколько новых астероидов
        /// </summary>
        protected virtual void OnDeathSpecialities()
        {
        }

        private void DestroyEffects()
        {
            if (m_ExplosionPrefab != null)
            {
                // взрыв корабля
                var explosion = Instantiate(m_ExplosionPrefab);
                explosion.transform.position = transform.position;

                if (m_DestructionSFX != null)
                {
                    Debug.Log("Play sound explosion destructible");
                    AudioSource source = (AudioSource)explosion.gameObject.AddComponent(typeof(AudioSource));
                    source.clip = m_DestructionSFX;
                    source.spatialBlend = 0.0f;
                    source.volume = 0.3f;
                    source.Play();
                }
            }
        }

        private static HashSet<DestructibleBase> m_AllDesctructibles;
        public static IReadOnlyCollection<DestructibleBase> AllDestructibles => m_AllDesctructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDesctructibles == null)
            {
                m_AllDesctructibles = new HashSet<DestructibleBase>();
            }

            m_AllDesctructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDesctructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent<DestructibleBase> m_EventOnDeath;
        public UnityEvent<DestructibleBase> EventOnDeath => m_EventOnDeath;

    }

}
