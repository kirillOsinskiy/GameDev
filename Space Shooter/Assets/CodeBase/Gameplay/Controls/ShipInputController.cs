using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class ShipInputController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Joystick
        }

        [SerializeField] private ControlMode m_ControlMode;

        private SpaceShip m_TargetShip;
        private VirtualGamepad m_VirtualGamepad;

        public void Construct(VirtualGamepad virtualGamepad)
        {
            m_VirtualGamepad = virtualGamepad;
        }

        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
                m_VirtualGamepad.VirtualJoystick.gameObject.SetActive(false);
            else
                m_VirtualGamepad.VirtualJoystick.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Keyboard)
            {
                ControlKeyboard();

                m_VirtualGamepad.MobileFirePrimary.gameObject.SetActive(false);
                m_VirtualGamepad.MobileFireSecondary.gameObject.SetActive(false);
            }

            if (m_ControlMode == ControlMode.Joystick)
            {
                ControlJoystick();

                m_VirtualGamepad.MobileFirePrimary.gameObject.SetActive(true);
                m_VirtualGamepad.MobileFireSecondary.gameObject.SetActive(true);
            }
        }

        private void ControlJoystick()
        {
            var dir = m_VirtualGamepad.VirtualJoystick.Value;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_VirtualGamepad.MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (m_VirtualGamepad.MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }
        }

        private void ControlKeyboard()
        {
            float thrust = 0.0f; // т€га
            float torque = 0.0f; // вращение

            if (Input.GetKey(KeyCode.UpArrow))
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow))
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                torque = 1.0f;

            if (Input.GetKey(KeyCode.RightArrow))
                torque = -1.0f;

            if (Input.GetKey(KeyCode.Space))
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }

        public void SetTarget(SpaceShip ship)
        {
            m_TargetShip = ship;
        }
    }
}