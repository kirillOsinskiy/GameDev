using System;
using UnityEngine;


namespace SpaceShooter
{
    public class Cannon : MonoBehaviour
    {
        /// <summary>
        /// —сылка на префаб снар€да
        /// </summary>
        [SerializeField] private GameObject m_ProjectilePrefab;

        private float m_ForwardOffset = 0.7f;

        public void Shoot()
        {            
            var newProjectile = Instantiate(
                original: m_ProjectilePrefab,
                position: transform.position + transform.up * m_ForwardOffset,
                rotation: transform.rotation
            );            

            var projectile = newProjectile.GetComponent<MyProjectile>();            
            projectile.dir = transform.up;
         
        }
    }
}