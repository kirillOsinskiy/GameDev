using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Debris : Destructible
    {
        public enum DebrisSize
        {
            Small,
            Big
        }

        [SerializeField] private DebrisSize m_Size;
        public DebrisSize Size => m_Size;

        protected override void OnDeathSpecialities()
        {
            if (m_Size == DebrisSize.Big)
            {
                int randomDebrisCount = Random.Range(2, 4);
                for (int i = 0; i < randomDebrisCount; i++)
                {
                    SpawnSmallDebris();
                }
            }
        }

        private void SpawnSmallDebris()
        {
            Debris debris = Instantiate(this, transform.position, Quaternion.identity);

            debris.SetSize(DebrisSize.Small);

            // назначаем скорость мусору
            Rigidbody2D rigidBodyChild = debris.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidBodySelf = gameObject.GetComponent<Rigidbody2D>();

            if (rigidBodyChild != null && rigidBodySelf != null)
            {
                rigidBodyChild.velocity = Random.insideUnitCircle * rigidBodySelf.velocity.magnitude;
            }
        }

        private void SetSize(DebrisSize size)
        {
            transform.localScale = getVectorFromSize(size);
            m_Size = size;
        }

        private Vector3 getVectorFromSize(DebrisSize size)
        {
            switch (size)
            {
                case DebrisSize.Small: return new Vector3(0.6f, 0.6f, 0.6f);
                case DebrisSize.Big:
                default: return Vector3.one;
            }
        }
    }
}