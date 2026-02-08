using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovebleWithTTL : DestructibleWithTTL
    {
        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [SerializeField] protected float m_Mass;
        /// <summary>
        /// Толкающая вперёд сила.
        /// </summary>
        [SerializeField] protected float m_Thrust;
        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] protected float m_MaxLinearVelocity;

        /// <summary>
        /// Сохранённая ссылка на ригид.
        /// </summary>
        protected Rigidbody2D m_Rigid;

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();
        }
        #endregion

        protected virtual void UpdateRigidBody()
        {
        }
    }
}