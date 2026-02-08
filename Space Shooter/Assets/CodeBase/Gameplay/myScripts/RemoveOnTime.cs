using UnityEngine;

namespace SpaceShooter
{
    public class RemoveOnTime : MonoBehaviour
    {
        [SerializeField] private float m_TimeUntilRemove;

        private float timer = 0.0f;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > m_TimeUntilRemove)
            {
                Destroy(gameObject);
            }
        }
    }
}