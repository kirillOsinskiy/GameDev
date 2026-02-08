using Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        [SerializeField] private LevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted = false;
        private float m_LevelTime = 0;
        private LevelSequenceController m_levelSequenceController;
        private LevelProperties m_currentLevelProperties;

        public float LevelTime => m_LevelTime;

        private void Start()
        {
            Time.timeScale = 1.0f;
            m_LevelTime = 0;
            m_levelSequenceController = LevelSequenceController.Instance;
            m_currentLevelProperties = m_levelSequenceController.GetCurrentLevel();
        }

        private void Update()
        {
            if (!m_IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelComplete();
            }

            if (Player.Instance.NumLives == 0)
            {
                Lose();
            }
        }

        private bool CheckLevelComplete()
        {
            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted) numCompleted++;
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                Debug.Log("Level Complete!");

                Pass();
            }

            return true;
        }

        private void Lose()
        {
            LevelLost?.Invoke();
            Time.timeScale = 0f;
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            Time.timeScale = 0f;
        }

        public void LoadNextLevel()
        {
            if (m_levelSequenceController.IsCurrentLevelLast() == false)
            {
                SceneManager.LoadScene(m_levelSequenceController.GetNextLevelProperties().SceneName);
            }
            else
            {
                SceneManager.LoadScene("main_menu");
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_currentLevelProperties.SceneName);
        }
    }
}