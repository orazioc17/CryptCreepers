using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int addedTime = 10;
    [SerializeField] AudioClip itemClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(itemClip, transform.position);
            GameManager.sharedInstance.time += addedTime;
            Destroy(gameObject, 0.1f);
        }
    }
}
