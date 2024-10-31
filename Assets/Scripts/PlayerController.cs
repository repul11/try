using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    bool isJump = true;
    bool isDead = false;
    int idMove = 0;
    Animator anim;

    public GameObject Projectile; // object peluru
    public Vector2 projectileVelocity; // kecepatan peluru
    public Vector2 projectileOffset; // jarak posisi peluru dari posisi pla yer
    public float cooldown = 0.5f; // jeda waktu untuk menembak
    bool isCanShoot = true; // memastikan untuk kapan dapat menembak

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isCanShoot = false;
        EnemyController.EnemyKilled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Idle();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Idle();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }
        Move();
        Dead();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //nyentuh tanah
        if (isJump)
        {
            anim.ResetTrigger("jump");
            if (idMove == 0) anim.SetTrigger("idle");
            isJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Peluru"))
        {
            isCanShoot = true;
        }
        if (collision.transform.tag.Equals("Enemy"))
        {
            SceneManager.LoadScene("Game Over");
            isDead = true;
            
            //ngecek mati ato ngga
            Debug.Log("DED");
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // nyentuh tanah
        anim.SetTrigger("jump");
        anim.SetTrigger("run");
        anim.SetTrigger("idle");
        isJump = true;
    }

    public void MoveRight()
    {
        idMove = 1;
    }

    public void MoveLeft()
    {
        idMove = 2;
    }

    private void Move()
    {
        if (idMove == 1 && !isDead)
        {
            // Kondisi ketika bergerak ke kekanan 83.
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        if (idMove == 2 && !isDead)
        {
            // Kondisi ketika bergerak ke kiri
            if (!isJump) anim.SetTrigger("run");
            transform.Translate(-1 * Time.deltaTime * 5f, 0, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Jump()
    {
        if (!isJump)
        {
            // Kondisi ketika Loncat 
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 300f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Coin"))
        {
            Data.score += 1;
            Destroy(collision.gameObject);
        }
    }

    public void Idle()
    {
        // kondisi ketika idle/diam 
        if (!isJump)
        {
            anim.ResetTrigger("jump");
            anim.ResetTrigger("run");
            anim.SetTrigger("idle");
        }
        idMove = 0;
    }

    private void Dead()
    {
        if (!isDead)
        {
            if (transform.position.y < -6f)
            {
                // kondisi ketika jatuh 
                isDead = true;
                SceneManager.LoadScene("Game Over");
            }

        }
    }

    void Fire()
    {
        if (isCanShoot)
        {
            //buat projectile baru
            GameObject bullet = Instantiate(Projectile, 
                (Vector2)transform.position - projectileOffset * transform.localScale.x, 
                Quaternion.identity);

            //kecepatan projectile
            Vector2 velocity = new Vector2(projectileVelocity.x * transform.localScale.x, projectileVelocity.y);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity * -1;

            //scale projectile
            Vector3 scale = transform.localScale;
            bullet.transform.localScale = scale * -1;

            StartCoroutine(CanShoot());
            anim.ResetTrigger("shoot");

        }
    }

    IEnumerator CanShoot()
    {
        //anim.ResetTrigger("shoot");
        isCanShoot = false;
        yield return new WaitForSeconds(cooldown);
        anim.ResetTrigger("shoot");
        isCanShoot = true;

    }
}

