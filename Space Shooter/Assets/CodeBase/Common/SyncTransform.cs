using UnityEngine;

namespace Common
{
    public class SyncTransform : MonoBehaviour
    {
        public enum UpdateType
        {
            UPDATE, FIXED_UPDATE
        }

        [SerializeField] private Transform m_Target;
        [SerializeField] private UpdateType m_UpdateType;

        public void SetTarget(Transform target)
        {
            m_Target = target;
        }

        private void Update()
        {
            if (m_UpdateType == UpdateType.UPDATE)
            {
                SyncPosition();
            }
        }

        private void FixedUpdate()
        {
            if (m_UpdateType == UpdateType.FIXED_UPDATE)
            {
                SyncPosition();
            }
        }

        private void SyncPosition()
        {
            transform.position = new Vector3(m_Target.position.x, m_Target.position.y, transform.position.z);
        }
    }
}