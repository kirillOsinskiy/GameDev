using TMPro;
using UnityEngine;

namespace SpaceShooter
{

    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Kills;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private TextMeshProUGUI m_ButtonNextText;

        private bool m_LevelPassed = false;

        // Start is called before the first frame update
        void Start()
        {
            m_LevelPassed = false;

            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
            gameObject.SetActive(true);
            m_LevelPassed = true;

            FillLevelStatistics();

            m_Result.text = "Passed!";

            if (LevelSequenceController.Instance.IsCurrentLevelLast())
            {
                m_ButtonNextText.text = "Main menu";
            }
            else
            {
                m_ButtonNextText.text = "Next";
            }
        }

        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            FillLevelStatistics();

            m_Result.text = "Lose";

            m_ButtonNextText.text = "Restart";
        }

        private void FillLevelStatistics()
        {
            m_Kills.text = "Kills : " + Player.Instance.NumKills.ToString();
            m_Score.text = "Score : " + Player.Instance.Score.ToString();
            m_Time.text = "Time : " + LevelController.Instance.LevelTime.ToString("F0");
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if (m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }
            else
            {
                LevelController.Instance.RestartLevel();
            }
        }
    }
}