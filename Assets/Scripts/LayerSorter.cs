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
        if (collision.CompareTag("Obstacle"))
        {
            Obstacle obs = collision.GetComponent<Obstacle>();
            obs.FadeOut();
            if(obstacles.Count == 0 || obs.MySpriteRenderer.sortingOrder - 1 <parentRenderer.sortingOrder) //6.3
            {
                parentRenderer.sortingOrder = obs.MySpriteRenderer.sortingOrder - 1;
            }
            obstacles.Add(obs);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle")) //if i stop colliding with obstacle
        {
            Obstacle obs = collision.GetComponent<Obstacle>(); //reference obstacle
            obs.FadeIn();
            obstacles.Remove(obs);
            if (obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 200;
            }
            else
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
