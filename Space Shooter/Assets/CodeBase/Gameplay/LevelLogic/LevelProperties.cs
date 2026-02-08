using UnityEngine;

namespace SpaceShooter
{
    [System.Serializable]
    public class LevelProperties
    {
        [SerializeField] private string m_Tilte;
        [SerializeField] private string m_SceneName;
        [SerializeField] private Sprite m_PreviewImage;
        [SerializeField] private LevelProperties m_NextLevel;

        public string Title => m_Tilte;
        public string SceneName => m_SceneName;
        public Sprite PreviewImage => m_PreviewImage;
    }
}