using UnityEngine;

namespace SpaceShooter
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперёд сила.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращательная скорость. В градусах/сек.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// Сохранённая ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite PreviewImage => m_PreviewImage;

        #region Public API

        /// <summary>
        /// Управление линейной тягой. От -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. От -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();
            UpdateEnergyRegen();
            CheckIndestructibleTimeOver();
            CheckSpeedUpTimeOver();
        }

        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения
        /// </summary>
        private void UpdateRigidBody()
        {
            // тяга вперёд
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            // ограничение максимальной скорости
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            // вращение
            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime);

            // ограничение скорости поворота
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime);
        }

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            foreach (var turret in m_Turrets)
            {
                if (turret.Mode == mode) turret.Fire();
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        #region Power Up methods
        public void AddEnergy(int e)
        {
            // ограничиваем значение энергии максимально допустимым
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            // ограничиваем значение боеприпасов максимально допустимым
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        /// <summary>
        /// Сделать корабль неуязвимым. Нужно для применения бонуса неуязвимости.
        /// Если корабль уже был неуязвимым, то прибавляем время бонуса к таймеру неуязвимости
        /// </summary>
        /// <param name="time">Время на которое делаем корабль неуязвимым</param>
        public void SetIndestructible(int time)
        {
            indestructibleTimer += time;
            SetIndestructible(true);
        }

        /// <summary>
        /// Время неуязвимости корабля
        /// </summary>
        private float indestructibleTimer = 0;

        /// <summary>
        /// Проверяем, что бонус неуязвимости ещё должен действовать или убираем его
        /// </summary>
        private void CheckIndestructibleTimeOver()
        {
            if (IsIndestructible)
            {
                indestructibleTimer -= Time.deltaTime;

                if (indestructibleTimer <= 0)
                {
                    indestructibleTimer = 0;
                    SetIndestructible(false);
                }
            }
        }

        /// <summary>
        /// Ускорить корабль на время на величину m_SpeedUpBonus
        /// </summary>
        /// <param name="time">Время ускорения</param>
        public void AddThrust(int time)
        {
            speedUpTimer += time;
            m_Thrust += m_SpeedUpBonus;
            IsSpeedUp = true;
        }

        /// <summary>
        /// Проверяем таймер бонуса ускорения корабля или убираем его
        /// </summary>
        private void CheckSpeedUpTimeOver()
        {
            if (IsSpeedUp)
            {
                speedUpTimer -= Time.deltaTime;

                if (speedUpTimer <= 0)
                {
                    speedUpTimer = 0;
                    IsSpeedUp = false;
                    m_Thrust -= m_SpeedUpBonus;
                }
            }
        }

        /// <summary>
        /// Величина бонуса ускорения
        /// </summary>
        [SerializeField] private int m_SpeedUpBonus;
        /// <summary>
        /// Время действия бонуса ускорнения корабля
        /// </summary>
        private float speedUpTimer = 0;

        /// <summary>
        /// Флаг активности бонуса ускорения
        /// </summary>
        private bool IsSpeedUp = false;

        #endregion

        /// <summary>
        /// Начинаем с максимальными запасами энергии и патронов
        /// </summary>
        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        /// <summary>
        /// Восстановление энергрии со временем
        /// </summary>
        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        /// <summary>
        /// Метод уменьшения энергии
        /// </summary>
        /// <param name="count"> Количество на которое нужно уменьшить энергию</param>
        /// <returns>true - успешно</returns>
        public bool DrawEnergy(int count)
        {
            // если пытаемся отнять 0 энергии, то всегда true
            if (count == 0) return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Метод уменьшения патронов
        /// </summary>
        /// <param name="count"> Количество на которое нужно уменьшить патроны</param>
        /// <returns>true - успешно</returns>
        public bool DrawAmmo(int count)
        {
            // если пытаемся отнять 0 патронов, то всегда true
            if (count == 0) return true;

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        public void AssignWeapon(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
    }
}