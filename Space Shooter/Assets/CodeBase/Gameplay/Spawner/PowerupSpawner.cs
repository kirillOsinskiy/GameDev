using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class PowerupSpawner : AbstractSpawner
    {
        protected override void SpawnSpecialities(GameObject obj)
        {
            Debug.Log("Powerup spawned!");
        }
    }
}