using SpaceShooter;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// “рансофрм объекта за которым следит камера
    /// </summary>
    [SerializeField] private Transform m_Target;
    /// <summary>
    /// —корость движени€ камеры
    /// </summary>
    [SerializeField] private float m_InterpolationLinear;
    /// <summary>
    /// —корость угловой интерпол€ции (поворота) камеры
    /// </summary>
    [SerializeField] private float m_InterpolationAngular;
    /// <summary>
    /// —мещение камеры по оси Z
    /// </summary>
    [SerializeField] private float m_CameraZOffset;
    /// <summary>
    /// —мещение по направлению движени€
    /// </summary>
    [SerializeField] private float m_ForwardOffset;

    private void FixedUpdate()
    {
        if (m_Target == null) return;

        Vector2 camPos = transform.position;
        Vector2 targetPos = m_Target.position + (m_Target.transform.up * m_ForwardOffset);
        Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);

        transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

        if (m_InterpolationAngular > 0)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                m_Target.rotation,
                m_InterpolationAngular * Time.deltaTime
            );
        }
    }

    public void SetTarget(SpaceShip target)
    {
        m_Target = target.transform;
    }
}
