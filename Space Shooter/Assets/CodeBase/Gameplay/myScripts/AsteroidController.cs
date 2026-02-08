using SpaceShooter;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{

    /// <summary>
    /// —сылка на префаб астероида
    /// </summary>
    [SerializeField] private GameObject m_Asteroid;

    /// <summary>
    /// ¬рем€ между спавнами новых астероидов
    /// </summary>
    [SerializeField] private float timeToSpawn;

    [SerializeField] private float m_MinAsteroidTTL;
    [SerializeField] private float m_MaxAsteroidTTL;

    private float timer = 0.0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToSpawn)
        {
            timer = 0.0f;
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        var newAsteroid = Instantiate(m_Asteroid);

        newAsteroid.transform.position = new Vector3 (Random.Range(-30, 30), 35.0f, newAsteroid.transform.position.z);

        var asteroid = newAsteroid.GetComponent<Asteroid>();
        asteroid.TimeToLive = Random.Range(m_MinAsteroidTTL, m_MaxAsteroidTTL);
    }
}
