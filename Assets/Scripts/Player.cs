using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float h;
    float v;
    Vector3 moveDirection;
    [SerializeField] float speed = 3f;
    [SerializeField] Transform aim;
    [SerializeField] Camera playerCamera;

    [SerializeField] Transform bulletPrefab;

    bool gunLoaded = true;

    [SerializeField] float fireRate = 1;

    [SerializeField] int health = 10;

    bool playerInmune = false;

    Vector2 facingDirection;

    bool powerShotEnabled = false;

    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float blinkRate = 0.02f;
    CameraController camController;
    [SerializeField] AudioClip itemClip;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            UIManager.sharedInstance.UpdateUIHealth(health);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CameraController>();
        UIManager.sharedInstance.UpdateUIHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        // Con esto consigo el input de movimiento, sera -1 para la direccion negativa y 1 para la positiva
        ReadInput();

        //Movimiento del personaje
        transform.position += moveDirection * Time.deltaTime * speed; // Controlando la velocidad del jugador

        // Movimiento de la mira
        //aim.position = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        /* El metodo ScreenToWorldPoint traduce un  punto del espacio en la pantalla y lo convierte a un punto del espacio
           en el mundo, basicamente el puntero del mouse vive en el espacio de la pantalla y el jugador y los gameobjects viven en el
           espacio del mundo, entonces asi se puede conocere la posicion del mouse en la pantalla para poder usarlo como lo 
           necesitemos*/

        //Movimiento de la mira
        facingDirection = playerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Restando el transform.position podremos saber a que direccion esta apuntando el mouse en referencia con el jugador

        aim.position = transform.position + (Vector3)facingDirection.normalized; // Antes fallaba porque se usaba desde el inicio
        // Un Vector3, asi que se uso el normalized con vector3 y luego se convirtio en Vector3 para que funcione correctamente

        if (Input.GetMouseButton(0) && gunLoaded) // El mouse bullet 0 es el click izq
        {
            FireGun();
        }

        UpdatePlayerGraphics();

    }

    private void UpdatePlayerGraphics()
    {
        if (moveDirection.magnitude > 0.1)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (aim.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (aim.position.x < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void FireGun()
    {
        gunLoaded = false;
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Transform bulletClone = Instantiate(bulletPrefab, transform.position, targetRotation);
        if (powerShotEnabled)
        {
            bulletClone.GetComponent<Bullet>().powerShot = true;
        }

        /*Al lanzar una bala se va a disparar la co rutina, que lo que hace es esperar un segundo y recarga la bala*/
        StartCoroutine(ReloadGun());
    }

    private void ReadInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        moveDirection.x = h;
        moveDirection.y = v;
    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(1/fireRate);
        gunLoaded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !playerInmune)
        {
            TakeDamage();
        } 
        if (collision.CompareTag("PowerUp"))
        {
            AudioSource.PlayClipAtPoint(itemClip, transform.position);
            switch (collision.GetComponent<PowerUp>().powerUpType)
            {
                case PowerUp.PowerUpType.FireRateIncrease:
                    fireRate++;
                    break;
                case PowerUp.PowerUpType.PowerShot:
                    powerShotEnabled = true;
                    break;
                default:
                    break;
            }
            Destroy(collision.gameObject, 0.1f);
        }
    }

    void TakeDamage()
    {
        Health--;
        playerInmune = true;
        fireRate = 1;
        powerShotEnabled = false;
        camController.Shake();
        StartCoroutine(BlinkRoutine());
        Invoke("EndInmune", 3); // De esta forma el jugador permanece inmune un momento para volver a recibir danho
        if (Health <= 0)
        {
            Time.timeScale = 0;
            GameManager.sharedInstance.gameOver = true;
            UIManager.sharedInstance.ShowGameOverScreen();
        }
    }

    void EndInmune()
    {
        playerInmune = false;
    }

    IEnumerator BlinkRoutine()
    {
        int t = 10;
        while (t > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(t * blinkRate);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(t * blinkRate);
            t--;
        }
        
    }
}
