using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircleObject : MonoBehaviour
{
    public Rigidbody2D rb;
    public float wallX_Dissipation = 0.7f;
    public float wallY_Dissipation = 0.7f;
    public bool gravity = false;
    public float forceGravity = 9.8f;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "WallY")
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -wallY_Dissipation);
        }
        if (col.tag == "WallX")
        {
            rb.velocity = new Vector2(rb.velocity.x * -wallX_Dissipation, rb.velocity.y);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (rb.velocity.y >= -0.5f  && rb.velocity.y <= 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            gravity = false;
        }
    }

    private void FixedUpdate()
    {
        if (gravity)
        {
            rb.velocity -= new Vector2(0, forceGravity * Time.deltaTime);
        }
    }

    
    public void ResetObjectCall() { StartCoroutine(ResetObject()); }
    private IEnumerator ResetObject()
    {
        yield return new WaitForSeconds(5f);
        gravity = false;
        rb.velocity = new Vector2(0, 0);
        gameObject.SetActive(false);
    }
    
}

