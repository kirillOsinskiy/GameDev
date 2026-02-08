using System.Linq;
using Common;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceShooter
{
    public class SelfDirectedProjectile : Projectile
    {
        [SerializeField] private float m_SearchTargetRadius;
        [SerializeField] private float m_RotationSpeed;

        protected override Vector2 GetDirection()
        {
            var target = FindTarget();

            if (target == null)
            {
                return transform.up; // если целей нет, то по умолчанию летим вверх
            }
            else
            {
                // Если цель ещё существует, то просчитываем вектор направления до неё
                Vector2 projectilePos = transform.position;
                Vector2 targetPos = target.transform.position;
                Vector2 direction = targetPos - projectilePos;

                return new Vector2(direction.x, direction.y).normalized;
            }
        }

        protected override void ProjectileRotation()
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, GetDirection());
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Поиск цели для самонаводящейся ракеты
        /// </summary>
        private Destructible FindTarget()
        {
            var target = Physics2D.OverlapCircleAll(transform.position, m_SearchTargetRadius)
                .Select(collision => collision.transform.root.GetComponent<Destructible>())
                .Where(destructible => destructible is SpaceShip) // наводимся только на корабли
                .Where(destructible => destructible != m_Parent) // проверяем, что цель не выпуствший ракету корабль
                .Where(destructible => destructible.TeamId != m_Parent.TeamId) // проверяем, что цель не из нашей команды
                .Where(destructible => destructible.TeamId != Destructible.TeamIdNeutral) // проверяем, что цель не из нейтральной команды
                .NotNull()
                .OrderBy(destructible => Vector2.Distance(destructible.transform.position, transform.position)) // сортируем по дальности цели, чтобы выбрать ближайшую
                .DefaultIfEmpty(null)
                .First();

            if (target != null) return target;
            return null;
        }

        private string IgnoreTag = "WordlBoundary";

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var destructable = transform.root.GetComponent<Destructible>();

            if (collision.transform.tag == IgnoreTag) return;

            if (destructable != null && destructable != m_Parent)
            {
                destructable.ApplyDamage(m_Damage);
                OnHit(collision.collider);                
                OnProjectileLifeEnd(collision.collider, transform.position);
            }
        }
    }
}