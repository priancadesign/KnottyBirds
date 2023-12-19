using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public bool isOverlap;
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Line")
        isOverlap = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Line")
        isOverlap = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Line")
        isOverlap = false;
    }
}
