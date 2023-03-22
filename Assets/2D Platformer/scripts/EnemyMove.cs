using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;
    public float nextThinktime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        ThinkAI();
    }
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Ground Check Ray
        Vector2 frontVec = new(rigid.position.x + (nextMove * 0.5f), rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        //Turn If Edge
        if (rayHit.collider == null)
        {
            if (spriteRenderer.flipY != true)
            {
                Turn();
            }
            else
                Invoke(nameof(DeActive), 5);
        }

    }
    void Update()
    {

    }

    void ThinkAI()
    {
        //Think Move
        nextMove = Random.Range(-1, 2);
        nextThinktime = Random.Range(1f, 5f);

        //Animation
        if (Mathf.Abs(rigid.velocity.x) <= 0)
        {
            anim.SetInteger("walkSpeed", nextMove);
        }
        else
            anim.SetInteger("walkSpeed", nextMove);

        //Flip
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        //Recursive Call
        Invoke(nameof(ThinkAI), nextThinktime);//Delay Invoke(most bottom)
    }
    void Turn()
    {
        CancelInvoke();
        ThinkAI();
    }
    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}
