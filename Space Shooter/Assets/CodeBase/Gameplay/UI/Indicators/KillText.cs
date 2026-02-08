using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class KillText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private int lastNumKills;

        private void Update()
        {
            var kills = Player.Instance.NumKills;
            if (kills != lastNumKills)
            {
                m_Text.text = "Kills : " + kills.ToString();
                lastNumKills = kills;
            }
        }
    }
}