using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MovebleWithTTL
    {
        /// <summary>
        /// ћетод добавлени€ сил астероиду дл€ движени€
        /// </summary>
        protected override void UpdateRigidBody()
        {
            // т€га вперЄд
            m_Rigid.AddForce(m_Thrust * -transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            // ограничение максимальной скорости
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

    }
}