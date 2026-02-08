using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private int lastScore;

        private void Update()
        {
            var score = Player.Instance.Score;
            if (score != lastScore)
            {
                m_Text.text = "Score : " + score.ToString();
                lastScore = score;
            }
        }
    }
}