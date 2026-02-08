using UnityEngine;

namespace SpaceShooter
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
        private string IgnoreTag = "WordlBoundary";

        [SerializeField] private float m_VelocityDamageModifier;
        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var destructable = transform.root.GetComponent<Destructible>();

            if (collision.transform.tag == IgnoreTag) return;

            if (destructable != null)
            {
                var damage = m_DamageConstant + (collision.relativeVelocity.magnitude * m_VelocityDamageModifier);

                destructable.ApplyDamage((int)damage);
            }
        }
    }
}