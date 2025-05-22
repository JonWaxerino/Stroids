using UnityEngine;

public class StroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject LargeStroid, MediumStroid;

    [SerializeField] private float stroidTimer;
    [SerializeField] private float timeToSpawnStroid;
    [SerializeField, Range(0, 1.0f)] private float chanceToSpawnLargeStroid;

    void Update()
    {
        stroidTimer += Time.deltaTime;
        if (stroidTimer >= timeToSpawnStroid)
        {
            stroidTimer = 0;

            GameObject newStroid;
            StroidType type;
            float bigStroidSpawn = Random.Range(0, 1.0f);

            if (bigStroidSpawn >= chanceToSpawnLargeStroid)
            {
                newStroid = Instantiate(LargeStroid);
                type = StroidType.Large;
            }
            else
            {
                newStroid = Instantiate(MediumStroid);
                type = StroidType.Medium;
            }

            Vector3 normalizedStroidDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(1.0f, 1.0f)).normalized;
            newStroid.transform.position =
                normalizedStroidDirection * Camera.main.orthographicSize * Camera.main.aspect;
            newStroid.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));

            StroidBehaviour behaviour = newStroid.GetComponent<StroidBehaviour>();
            behaviour.type = type;
        }
    }
}
