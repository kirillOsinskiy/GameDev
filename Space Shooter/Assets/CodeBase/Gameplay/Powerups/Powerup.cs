using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Базовый класс подбираемых объектов
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] private AudioClip m_PickupSFX;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SpaceShip ship = collision.transform.root.GetComponent<SpaceShip>();

            if (ship != null 
                //&& Player.Instance.ActiveShip == ship /* чтобы бонусы мог подбирать только игрок */
                )
            {
                OnPickedUp(ship);

                ship.PlaySFXSound(m_PickupSFX);

                Destroy(gameObject);
            }
        }

        protected abstract void OnPickedUp(SpaceShip ship);
    }
}