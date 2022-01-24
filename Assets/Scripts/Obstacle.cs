using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer MySpriteRenderer{ get; set; }
    private Color defColor;
    private Color fadeColor;
    public int CompareTo(Obstacle other)
    {
        if(MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1; //CompareTo need an integer (1, -1 or 0 for equal)
        }else if(MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;
        }
        else
        {
            return 0;
        }
        //throw new NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        defColor = MySpriteRenderer.color;
        fadeColor = defColor;
        fadeColor.a = 0.5f; //50% color
    }

    public void FadeOut()
    {
        MySpriteRenderer.color = fadeColor;
    }

    public void FadeIn()
    {
        MySpriteRenderer.color = defColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "ObstacleCollider")
        {
            FadeOut();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "ObstacleCollider")
        {
            FadeIn();
        }
    }
}
