using SpaceShooter;
using UnityEngine;

public class MyProjectile : MovebleWithTTL
{
    public Vector3 dir { get; set; }

    private Vector2 initialForce;

    protected override void Start()
    {
        base.Start();
        m_Rigid.totalForce = initialForce;
    }

    /// <summary>
    /// ћетод добавлени€ сил астероиду дл€ движени€
    /// </summary>
    override protected void UpdateRigidBody()
    {        
        // т€га вперЄд
        m_Rigid.AddForce(m_Thrust * dir * Time.fixedDeltaTime, ForceMode2D.Impulse);

        // ограничение максимальной скорости
        m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    public void SetForce(Vector2 force)
    {
        initialForce = force;
    }
}
