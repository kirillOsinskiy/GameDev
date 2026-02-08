using System;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        /// <summary>
        /// Тип поведения корабля
        /// </summary>
        public enum AIBehaviour
        {
            Null, // ничего не делает
            PatrolOnArea, // патрулирует в области выбирая случайные точки
            PatrolOnRoute // патрулирование по заданному маршруту
        }

        /// <summary>
        /// Хранит тип поведения
        /// </summary>
        [SerializeField] private AIBehaviour m_AIBehaviour;

        /// <summary>
        /// Область патрулирования (для поведения PatrolOnArea)
        /// </summary>
        [SerializeField] private AIPointPatrol m_PointPatrol;

        /// <summary>
        /// Точки для патрулирования по маршруту (для поведения PatrolOnRoute)
        /// </summary>
        [SerializeField] private AIPointPatrol[] m_PatrolRoute;

        /// <summary>
        /// Дистанция между кораблем и точкой на маршруте патрулирования при которой точка считается достигнутой
        /// </summary>
        [SerializeField] private float m_PatrolOnRoutePointDistance;

        /// <summary>
        /// Скорость перемещения
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        /// <summary>
        /// Скорость вращения
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        /// <summary>
        /// Таймер для изменения позиции
        /// </summary>
        [SerializeField] private float m_RandomSelectMovePointTime;

        /// <summary>
        /// Таймер для изменения цели
        /// </summary>
        [SerializeField] private float m_FindNewTargetTime;

        /// <summary>
        /// Таймер для стрельбы из основного оружия
        /// </summary>
        [SerializeField] private float m_ShootDelayPrimary;

        /// <summary>
        /// Таймер для стрельбы из дополнительного оружия
        /// </summary>
        [SerializeField] private float m_ShootDelaySecondary;

        /// <summary>
        /// Длина для raycast
        /// </summary>
        [SerializeField] private float m_EvadeRayLength;

        /// <summary>
        /// Длина дистанции до цели для атаки, дальше которой останавливаемся, чтобы избежать столкновения
        /// </summary>
        [SerializeField] private float m_DistanceFromTarget;

        /// <summary>
        /// Ссылка на свой корабль
        /// </summary>
        private SpaceShip m_SpaceShip;

        /// <summary>
        /// Точка назначения
        /// </summary>
        private Vector3 m_MovePosition;

        /// <summary>
        /// Выбранная цель для уничтожения
        /// </summary>
        private Destructible m_SelectedTarget;

        /// <summary>
        /// Таймер поиска новой точки патрулирования
        /// </summary>
        private Timer m_RandomizeDirectionTimer;

        private Timer m_FireTimerPrimary;

        private Timer m_FireTimerSecondary;

        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
            m_CurrentPatrolPointIndex = 0;
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.PatrolOnArea || m_AIBehaviour == AIBehaviour.PatrolOnRoute)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            // ищем целевую позицию для движения
            ActionFindNewPosition();
            // избегание столкновений
            ActionEvadeCollision();
            // двигаем корабль
            ActionControlShip();
            // ищем цель для атаки
            ActionFindNewAttackTarget();
            // атакуем цель
            ActionFire();
        }

        private void ActionFindNewPosition()
        {
            switch (m_AIBehaviour)
            {
                case AIBehaviour.PatrolOnArea:
                    PatrolOnArea();
                    break;
                case AIBehaviour.PatrolOnRoute:
                    PatrolOnRoute();
                    break;
            }
        }

        private void PatrolOnRoute()
        {
            //if (m_SelectedTarget != null)
            //{
            //    m_MovePosition = MakeLead();
            //}
            //else
            //{
                if (m_CurrentPatrolPoint == null)
                {
                    m_CurrentPatrolPoint = m_PatrolRoute[m_CurrentPatrolPointIndex];
                    m_MovePosition = m_CurrentPatrolPoint.transform.position;
                }

                if (Vector2.Distance(transform.position, m_CurrentPatrolPoint.transform.position) <= m_PatrolOnRoutePointDistance)
                {
                    m_CurrentPatrolPointIndex++;
                    if (m_CurrentPatrolPointIndex == m_PatrolRoute.Length)
                    {
                        m_CurrentPatrolPointIndex = 0;
                    }

                    m_CurrentPatrolPoint = m_PatrolRoute[m_CurrentPatrolPointIndex];
                    m_MovePosition = m_CurrentPatrolPoint.transform.position;
                }
                else if (Vector2.Distance(transform.position, m_MovePosition) <= m_PatrolOnRoutePointDistance)
                {
                    m_MovePosition = m_CurrentPatrolPoint.transform.position;
                }
            //}
        }

        private void PatrolOnArea()
        {
            if (m_SelectedTarget != null)
            {
                m_MovePosition = MakeLead();
            }
            else
            {
                if (m_PointPatrol != null)
                {
                    // проверяем, находится ли бот в зоне патрулирования
                    bool isInsidePatrolZone = (m_PointPatrol.transform.position - transform.position).sqrMagnitude < m_PointPatrol.Radius * m_PointPatrol.Radius;

                    if (isInsidePatrolZone)
                    {
                        if (m_RandomizeDirectionTimer.IsFinished)
                        {
                            Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PointPatrol.Radius + m_PointPatrol.transform.position;

                            m_MovePosition = newPoint;

                            m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                        }
                    }
                    else
                    {
                        m_MovePosition = m_PointPatrol.transform.position;
                    }
                }
            }
        }

        private void ActionEvadeCollision()
        {
            RaycastHit2D raycastCollision = Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength);

            if (raycastCollision)
            {
                Debris debris = raycastCollision.transform.root.GetComponent<Debris>();

                if (debris != null)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);
                }

                m_MovePosition = transform.position + transform.right * 5.0f;
            }
        }

        private void ActionControlShip()
        {
            // останавливаемся, если уже близко к цели, чтобы избежать беспорядочных метаний
            if (IsInsideMovePositionZone() && m_SelectedTarget != null)
            {
                m_SpaceShip.ThrustControl = 0;
            }
            else
            {
                m_SpaceShip.ThrustControl = m_NavigationLinear;
            }

            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(
                m_MovePosition,
                m_SpaceShip.transform
            ) * m_NavigationAngular;
        }

        /// <summary>
        /// Максимальный угол, при котором будет применена наибольшая сила для вращения
        /// </summary>
        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            // переводим позицию в локальные координаты
            // позиция своего корабля считается за 0,0,0
            // позиция цели будет иметь дургие значения в отличие от мировых координат
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            // получаем угол (со знаком) между вектором направления к цели и вектором наверх (относительно себя)
            // 3й параметр - ось относительн которой получаем угол (в нашем случае ось Z)
            // знак угла нужен, чтобы понимать в какую сторону поворачиваться к цели
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            // ограничивает величину угла поворота
            // определяет угол, при котором будет приложена максиамльная сила для вращения
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE);
            // переводим угол в нормальизованные координаты
            angle /= MAX_ANGLE;

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_SelectedTarget == null)
            {
                if (m_FindNewTargetTimer.IsFinished)
                {
                    m_FindNewTargetTimer.Start(m_FindNewTargetTime);

                    m_SelectedTarget = FindNearestTargetToAttack();
                }
            }
        }

        [SerializeField] private float m_MaxFireDistance;

        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                // стреляем, когда подлетим ближе к цели
                var distance = Vector2.Distance(m_SelectedTarget.transform.position, transform.position);
                if (distance <= m_MaxFireDistance)
                {
                    if (m_FireTimerPrimary.IsFinished)
                    {
                        m_SpaceShip.Fire(TurretMode.Primary);

                        m_FireTimerPrimary.Start(m_ShootDelayPrimary);
                    }

                    if (m_FireTimerSecondary.IsFinished)
                    {
                        m_SpaceShip.Fire(TurretMode.Secondary);

                        m_FireTimerSecondary.Start(m_ShootDelaySecondary);
                    }
                }
            }
        }

        private Destructible FindNearestTargetToAttack()
        {
            float minDist = float.MaxValue;
            Destructible target = null;

            foreach (var destructible in Destructible.AllDestructibles)
            {
                if (destructible.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (destructible.TeamId == Destructible.TeamIdNeutral) continue;

                if (destructible.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, destructible.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    target = destructible as Destructible;
                }
            }

            return target;
        }

        private Vector3 MakeLead()
        {
            Vector3 curTargetPosition = m_SelectedTarget.transform.position;
            Vector2 targetVelocity = m_SelectedTarget.GetComponent<Rigidbody2D>().velocity;

            var predictedPosition = curTargetPosition + new Vector3(targetVelocity.x, targetVelocity.y) * m_PredictionTime;

            Debug.DrawLine(transform.position, predictedPosition, Color.green);

            return predictedPosition;
        }
        
        [Range(0f, 5f)]
        [SerializeField] private float m_PredictionTime;

        /// <summary>
        /// Определяем, что подлетели достаточно близко к цели
        /// </summary>
        /// <returns></returns>
        private bool IsInsideMovePositionZone()
        {
            return (m_MovePosition - transform.position).sqrMagnitude < Math.Pow(m_DistanceFromTarget, 2);
        }

        public void SetAIPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.PatrolOnArea;
            m_PointPatrol = point;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimerPrimary = new Timer(m_ShootDelayPrimary);
            m_FireTimerSecondary = new Timer(m_ShootDelaySecondary);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.DistractTime(Time.deltaTime);
            m_FireTimerPrimary.DistractTime(Time.deltaTime);
            m_FireTimerSecondary.DistractTime(Time.deltaTime);
            m_FindNewTargetTimer.DistractTime(Time.deltaTime);
        }

        #endregion

        private AIPointPatrol m_CurrentPatrolPoint;
        private int m_CurrentPatrolPointIndex;

    }
}