using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class EntitySpawnerDebris : AbstractSpawner
    {
        public enum DebrisSize
        {
            Small,
            Big
        }

        /// <summary>
        /// Скорость мусора
        /// </summary>
        [SerializeField] private float m_RandomSpeed;

        /// <summary>
        /// Поддерживаем заданное количество мусора
        /// </summary>
        private void OnDebrisDie(DestructibleBase destructible)
        {
            Debris debris = destructible as Debris;

            SpawnEntity();
        }

        protected override void SpawnSpecialities(GameObject obj)
        {
            obj.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDie);            

            // назначаем скорость мусору
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            if (rigidbody != null && m_RandomSpeed > 0)
            {
                rigidbody.velocity = Random.insideUnitCircle * m_RandomSpeed;
            }
        }
    }
}