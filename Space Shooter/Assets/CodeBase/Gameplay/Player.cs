using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        /// <summary>
        /// Количество жизней игрока
        /// </summary>
        [SerializeField] private int m_NumLives;
        /// <summary>
        /// Ссылка на префаб корабля
        /// </summary>
        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        /// <summary>
        /// Контроллер камеры
        /// </summary>
        private FollowCamera m_FollowCamera;
        /// <summary>
        /// Контроллер управления кораблём
        /// </summary>
        private ShipInputController m_ShipInputController;
        /// <summary>
        /// Место спавна корабля игрока
        /// </summary>
        private Transform m_SpawnPoint;

        public FollowCamera FollowCamera => m_FollowCamera;

        public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
        {
            m_FollowCamera = followCamera;
            m_ShipInputController = shipInputController;
            m_SpawnPoint = spawnPoint;
        }

        /// <summary>
        /// Ссылка на корабль игрока
        /// </summary>
        private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        private int m_Score;
        private int m_NumKills;

        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives => m_NumLives;

        private void Start()
        {
            Respawn();
        }

        private void OnShipDeath(DestructibleBase ship)
        {
            m_NumLives--;

            if (m_NumLives > 0)
            {
                Debug.Log("Lives number=" + m_NumLives);
                Respawn();
            }
        }

        private void Respawn()
        {
            Debug.Log("Respawn ship");
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);

            m_FollowCamera.SetTarget(m_Ship);
            m_ShipInputController.SetTarget(m_Ship);
        }

        public void AddKill()
        {
            m_NumKills++;
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }
    }
}
