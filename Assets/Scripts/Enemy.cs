using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Transform player;
    [SerializeField] int health = 1;
    [SerializeField] float speed = 1;
    [SerializeField] int scorePoints = 100;
    [SerializeField] AudioClip impactClip, deathClip;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint"); // Con esto tendremos una lista con todos los spawnpoints que creamos
        int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
        transform.position = spawnPoint[randomSpawnPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.position - transform.position;
        transform.position += (Vector3)direction.normalized * Time.deltaTime * speed;
    }

    public void TakeDamage()
    {
        health--;
        AudioSource.PlayClipAtPoint(impactClip, transform.position);
        if(health <= 0)
        {
            AudioSource.PlayClipAtPoint(deathClip, transform.position);
            GameManager.sharedInstance.Score += scorePoints;
            Destroy(gameObject, 0.1f);
        }
    }
}
