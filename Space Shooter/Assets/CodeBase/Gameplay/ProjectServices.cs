using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequenceController m_levelSequenceController;

        private void Awake()
        {
            m_levelSequenceController.Init();
        }
    }
}