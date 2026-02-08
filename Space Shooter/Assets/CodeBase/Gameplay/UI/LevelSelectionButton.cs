using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_LevelTitle;
        [SerializeField] private Image m_PreviewImage;

        private LevelProperties m_LevelProperties;

        public void SetLevelProperties(LevelProperties levelProperties)
        {
            m_LevelProperties = levelProperties;
            m_PreviewImage.sprite = levelProperties.PreviewImage;
            m_LevelTitle.text = levelProperties.Title;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}