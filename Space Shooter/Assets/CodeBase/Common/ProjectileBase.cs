using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        /// <summary>
        /// Скорость снаряда
        /// </summary>
        [SerializeField] private float m_Velocity;
        /// <summary>
        /// Время жизни
        /// </summary>
        [SerializeField] private float m_Lifetime;
        /// <summary>
        /// Урон
        /// </summary>
        [SerializeField] protected int m_Damage;

        private float m_Timer;

        protected DestructibleBase m_Parent;

        /// <summary>
        /// Метод для определения нанесения урона целям
        /// </summary>
        /// <param name="hit"></param>
        protected virtual void ApplyDamage(RaycastHit2D hit)
        {
            DestructibleBase destructible = hit.collider.transform.root.GetComponent<DestructibleBase>();

            if (destructible != null && destructible != m_Parent)
            {
                destructible.ApplyDamage(m_Damage);

                OnHit(destructible);
            }
        }

        protected virtual void OnHit(DestructibleBase destructible) { }
        protected virtual void OnHit(Collider2D collider) { }
        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = GetDirection() * stepLength;

            ProjectileRotation();

            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirection(), stepLength);

            if (hit)
            {
                OnHit(hit.collider);
                ApplyDamage(hit);
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            transform.position += new Vector3(step.x, step.y, 0);

            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Lifetime)
            {
                OnProjectileLifeEnd(hit.collider, hit.point);
            }
        }

        public void SetParentShooter(DestructibleBase parent)
        {
            m_Parent = parent;
        }

        /// <summary>
        /// Метод определения поворота снаряда. Нужен, если у снаряда есть спрайт
        /// </summary>
        protected virtual void ProjectileRotation()
        {

        }

        /// <summary>
        /// Метод для определения направления полёта снаряда
        /// </summary>
        /// <returns></returns>
        protected virtual Vector2 GetDirection()
        {
            return transform.up;
        }
    }
}