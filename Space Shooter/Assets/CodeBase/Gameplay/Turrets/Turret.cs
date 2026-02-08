using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : TurretBase
    {
        /// <summary>
        /// Вид турели
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        private SpaceShip m_Ship;

        protected override void AssignParent()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        // Public API
        public override void Fire()
        {
            if (m_TurretProperties == null) return;
            if (m_RefireTimer > 0) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            // SFX
            m_Ship.PlaySFXSound(m_TurretProperties.LaunchSFX);
        }

        public override void AssignLoadout(TurretPropertiesBase turretProperties)
        {
            var props = turretProperties as TurretProperties;

            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;
        }
    }
}