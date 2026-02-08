using UnityEngine;

namespace Common
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_Lifetime;

        private float m_Timer = 0.0f;

        private void Update()
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}