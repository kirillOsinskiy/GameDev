using UnityEngine;

namespace SpaceShooter
{
    public class PowerupStats : Powerup
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            SetIndestructible,
            SpeedUp
        }

        [SerializeField] private EffectType m_EffectType;

        [SerializeField] private int m_Value;

        protected override void OnPickedUp(SpaceShip ship)
        {
            switch (m_EffectType)
            {
                case EffectType.AddAmmo:
                    ship.AddAmmo(m_Value);
                    break;
                case EffectType.AddEnergy:
                    ship.AddEnergy(m_Value);
                    break;
                case EffectType.SetIndestructible:
                    ship.SetIndestructible(m_Value);
                    break;
                case EffectType.SpeedUp:
                    ship.AddThrust(m_Value);
                    break;
                default:
                    Debug.Log("Unsupported effect type: " + m_EffectType);
                    break;
            }
        }
    }
}