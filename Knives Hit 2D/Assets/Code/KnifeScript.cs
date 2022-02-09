using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    [SerializeField]private Vector2 throwForce;

    private bool isActive = true;

    private Rigidbody2D rigidbody;
    private BoxCollider2D knifeCollider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        knifeCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            rigidbody.AddForce(throwForce, ForceMode2D.Impulse);
            rigidbody.gravityScale = 1;

            GameController.Instance.GameUI.DecrementDisplayKnifeCount();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        if (collision.collider.tag == "Log")
        {
            GetComponent<ParticleSystem>().Play();

            rigidbody.velocity = new Vector2(0, 0);
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.transform.SetParent(collision.collider.transform);

            knifeCollider.offset = new Vector2(knifeCollider.offset.x, -0.4f);
            knifeCollider.size = new Vector2(knifeCollider.size.x, 1.2f);

            GameController.Instance.OnSuccessfulKnifeHit();
        }

        else if (collision.collider.tag == "Knife")
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, -2);
            GameController.Instance.StartGameOverSequence(false);
        }
    }
}
