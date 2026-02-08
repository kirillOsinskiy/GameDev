using UnityEngine;

namespace Common
{
    public abstract class TurretBase : MonoBehaviour
    {
        /// <summary>
        /// Ссылка на свойства турели
        /// </summary>
        [SerializeField] protected TurretPropertiesBase m_TurretProperties;

        /// <summary>
        /// Таймер повторного выстрела
        /// </summary>
        protected float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        #region UNITY_EVENT
        private void Start()
        {
            AssignParent();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
        }
        #endregion

        public abstract void Fire();
        public abstract void AssignLoadout(TurretPropertiesBase turretProperties);

        protected abstract void AssignParent();
    }
}