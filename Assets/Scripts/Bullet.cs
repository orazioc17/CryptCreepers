using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] int health = 3;
    public bool powerShot;

    private void Start()
    {
        Destroy(gameObject, 5); // El 5 como segundo parametro, indicara que la bala sera destruida a los 5 segundos
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(); // Ejecutando el metodo TakeDamage del enemigo
            Destroy(gameObject);

            if (!powerShot)
            {
                Destroy(gameObject);
            }

            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
