using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer parentRenderer;
    // Start is called before the first frame update
    private List<Obstacle> obstacles = new List<Obstacle>();
    void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle obs = collision.GetComponent<Obstacle>();
            transform.parent.GetComponent<SpriteRenderer>().sortingOrder = obs.MySpriteRenderer.sortingOrder - 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parentRenderer.sortingOrder = 200;
    }
}
