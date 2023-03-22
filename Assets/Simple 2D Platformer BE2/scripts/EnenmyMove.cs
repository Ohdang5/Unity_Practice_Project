using UnityEngine;

public class EnenmyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public int nextMove;
    public float nextThinktime;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
            Turn();
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
}
