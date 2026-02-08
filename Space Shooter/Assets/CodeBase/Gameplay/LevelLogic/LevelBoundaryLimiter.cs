using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Ограничитель позиции. Работает вместе со скриптом LevelBoundary, если таковой имеется на сцене.
    /// Кидается на объект, который нужно ограничить.
    /// </summary>
    public class LevelBoundaryLimiter : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var levelBoundary = LevelBoundary.Instance;
            var radius = levelBoundary.Radius;

            if (transform.position.magnitude > radius)
            {
                if (levelBoundary.LimitMode == LevelBoundary.BoundaryMode.Limit)
                {
                    transform.position = transform.position.normalized * radius;
                }
                
                if (levelBoundary.LimitMode == LevelBoundary.BoundaryMode.Teleport)
                {
                    transform.position = -transform.position.normalized * radius;
                }
            }
        }
    }
}