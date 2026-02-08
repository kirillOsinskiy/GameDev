using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : ProjectileBase
    {

        /// <summary>
        /// Ёффект взрыва
        /// </summary>
        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        protected override void OnHit(DestructibleBase dest)
        {
            var destructible = dest as Destructible;

            if (m_Parent == Player.Instance.ActiveShip)
            {
                if (destructible.HitPoints > 0) return;

                Player.Instance.AddScore(destructible.ScoreValue);                

                if (destructible is SpaceShip)
                {
                    Player.Instance.AddKill();
                }
            }
        }

        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if (m_ImpactEffectPrefab != null)
            {
                Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}