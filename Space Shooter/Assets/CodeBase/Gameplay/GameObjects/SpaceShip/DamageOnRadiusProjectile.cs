using UnityEngine;

namespace SpaceShooter
{
    public class DamageOnRadiusProjectile : Projectile
    {
        [SerializeField] private float m_Radius;

        protected override void ApplyDamage(RaycastHit2D hit)
        {
            Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

            if (destructible != null && destructible != m_Parent)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_Radius);
                ApplyDamageOnRadius(colliders);
            }
        }

        private void ApplyDamageOnRadius(Collider2D[] colliders)
        {
            foreach (var hit in colliders)
            {
                Destructible destructible = hit.transform.root.GetComponent<Destructible>();
                if (destructible != null && destructible != m_Parent)
                {
                    destructible.ApplyDamage(m_Damage);

                    OnHit(destructible);
                }
            }
        }
    }
}