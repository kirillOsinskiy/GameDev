using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class DestructibleWithTTL : Destructible
    {
        [SerializeField] private float m_DefaultTimeToLive;
        public float TimeToLive { get; set; }

        private float timer = 0.0f;

        protected override void Start()
        {
            base.Start();
            if (TimeToLive == 0.0f)
            {
                TimeToLive = m_DefaultTimeToLive;
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= TimeToLive)
            {
                DestroyIt();
            }
        }
    }
}