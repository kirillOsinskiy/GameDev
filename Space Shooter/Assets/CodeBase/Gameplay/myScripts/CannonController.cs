using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace SpaceShooter
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private Cannon m_Cannon;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                m_Cannon.Shoot();
            }
        }

        public void SetTarget(SpaceShip target)
        {
            m_Cannon = target.GetComponent<Cannon>();
        }
    }
}