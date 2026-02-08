using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Класс простой реализации спавнера Destructible сущностей
    /// </summary>
    public class EntitySpawner : AbstractSpawner
    {
        /// <summary>
        /// Список кораблей, которые заспавнились
        /// </summary>
        private List<DestructibleBase> m_SpawnedEntities = new List<DestructibleBase>();

        /// <summary>
        /// При спавне сущности добавляет её в список заспавненных сущностей
        /// </summary>
        /// <param name="obj">Заспавненная сущность</param>
        protected override void SpawnSpecialities(GameObject obj)
        {
            DestructibleBase spawned = obj.GetComponent<DestructibleBase>();
            spawned.EventOnDeath.AddListener(OnEntityDie);
            m_SpawnedEntities.Add(spawned);
        }

        /// <summary>
        /// При уничтожении сущности убираемего из списка заспавненных сущностей
        /// </summary>
        /// <param name="entity">уничтоженная сущность</param>
        private void OnEntityDie(DestructibleBase entity)
        {
            m_SpawnedEntities.Remove(entity);
        }
    }
}