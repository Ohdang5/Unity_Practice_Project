using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    //Suitable For Instant Input
    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("is jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("is jump", true);
        }

        //Stop Move
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.x * 0.0001f, rigid.velocity.y);
        }

        //Flip Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.7)
        {
            anim.SetBool("is walk", false);
            rigid.velocity = new Vector2(rigid.velocity.x * 0.00001f, rigid.velocity.y);//°¨¼Ó
        }
        else
            anim.SetBool("is walk", true);

        if ((rigid.velocity.y) <= 0)
        {
            //Ray
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

            //Detect Ray
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("is jump", false);
            }
            else anim.SetBool("is jump", true);
        }

        //Detect Fall
        if ((rigid.velocity.y) < -1)
            anim.SetBool("is land", true);
        else
            anim.SetBool("is land", false);
    }
    void FixedUpdate()
    {
        //Move
        
        float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision With Enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                JumpAttack(collision.transform);
                OnTheEnemy();
                anim.SetBool("is jump", true);
            }
            else
                OnDamaged(collision.transform.position);
        }
        else if (collision.gameObject.CompareTag("Spike"))
        {
            OnDamaged(collision.transform.position);
        }
    }
    void JumpAttack(Transform enemy)
    {
        //Road EnemyMove.cs
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnTheEnemy()
    {
        //On The Enemy, Little Jump
        rigid.AddForce(0.5f * jumpPower * Vector2.up, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Item"))
        {
            return;
        }
        collision.gameObject.SetActive(false);
    }

    void OnDamaged(Vector2 targetPos)
    {
        anim.SetTrigger("is damaged");
        //Change Layer (Immortal Active)
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Knock Back
        int dirc = targetPos.x-transform.position.x < 0 ? 1 : -1;
        float opsdirc = rigid.velocity.x * -1f;
        rigid.AddForce(new Vector2(opsdirc, 0) * 7, ForceMode2D.Impulse);
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);
        Invoke(nameof(OffImmortal), 3);
    }
    void OffImmortal()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }
}
