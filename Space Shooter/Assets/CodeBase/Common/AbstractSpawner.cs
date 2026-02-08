using UnityEngine;

namespace Common
{
    /// <summary>
    /// Абстрактный класс спавнера
    /// </summary>
    [RequireComponent(typeof(CircleArea))]
    public abstract class AbstractSpawner : MonoBehaviour
    {
        /// <summary>
        /// Тип спавна объектов
        /// </summary>
        public enum SpawnMode
        {
            Start, // спавн только при старте
            Loop, // спавн по таймеру во время игры
            StartAndLoop // спавн при старте и дальше по таймеру
        }

        /// <summary>
        /// Префабы для спавна
        /// </summary>
        [SerializeField] private GameObject[] m_EntityPrefabs;
        /// <summary>
        /// Область спавна
        /// </summary>
        [SerializeField] private CircleArea m_Area;
        /// <summary>
        /// Тип спавна объектов
        /// </summary>
        [SerializeField] private SpawnMode m_SpawnMode;
        /// <summary>
        /// Количество объектов, которые будут спавниться
        /// </summary>
        [SerializeField] protected int m_NumSpawns;
        /// <summary>
        /// Время между респавнами (для типа спавна Loop)
        /// </summary>
        [SerializeField] private float m_RespawnTime;

        private float m_Timer;

        private void Start()
        {
            switch (m_SpawnMode)
            {
                case SpawnMode.Start:
                case SpawnMode.StartAndLoop:
                    SpawnEntities();
                    break;
                default:
                    break;
            }

            m_Timer = m_RespawnTime;
        }

        private void Update()
        {
            switch (m_SpawnMode)
            {
                case SpawnMode.Loop:
                case SpawnMode.StartAndLoop:
                    SpawnOnTimer();
                    break;
                default:
                    break;
            }
        }

        private void SpawnOnTimer()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
            }
            else
            {
                SpawnEntities();
                m_Timer = m_RespawnTime;
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                SpawnEntity();
            }
        }

        protected void SpawnEntity()
        {
            int index = Random.Range(0, m_EntityPrefabs.Length);
            GameObject obj = Instantiate(m_EntityPrefabs[index].gameObject);
            obj.transform.position = m_Area.GetRandomInsideZone();

            SpawnSpecialities(obj);
        }

        /// <summary>
        /// Метод для переопределения в дочерних классах. Позволяет задать особую логику спавна для сущностей.
        /// </summary>
        /// <param name="obj"></param>
        protected abstract void SpawnSpecialities(GameObject obj);
    }
}