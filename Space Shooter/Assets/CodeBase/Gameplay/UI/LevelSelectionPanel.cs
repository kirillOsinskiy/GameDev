using UnityEngine;

namespace SpaceShooter
{
    public class LevelSelectionPanel : MonoBehaviour
    {
        [SerializeField] private LevelSelectionButton m_LevelButtonPrefab;
        [SerializeField] private Transform m_Parent;

        private void Start()
        {
            LevelProperties[] allLevelProperties = LevelSequenceController.Instance.LevelSequence.Levels;

            for (int i = 0; i < allLevelProperties.Length; i++)
            {
                LevelSelectionButton btn = Instantiate(m_LevelButtonPrefab);
                btn.SetLevelProperties(allLevelProperties[i]);
                btn.transform.SetParent(m_Parent);
            }
        }
    }
}