using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData; // i make this into an array so i can have layers of things. Tiles bottom and on top trees and enemies etc // this method is helpfull to add more things on top of tiles, like snow eg
    [SerializeField]
    private MapElement[] mapElements; // the map elements, tiles, stones, grass etc
    [SerializeField]
    private Sprite defaultTile; //used as a meassure for space between tiles

    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();
    private Vector3 WorldStartPosition
    {
        get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GenerateMap()
    {
        int h = mapData[0].height;
        int w = mapData[0].width;
        for (int i = 0; i < mapData.Length; i++)// run the length of mapdata. all layers
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color theColor = mapData[i].GetPixel(x, y); //starting bot left going upwards and then 2nd column the same 3d same etc. Get color of current pixel

                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == theColor); //look through all premade tiles that we can spawn and if one of those has the same color as the color on the bitmap then i return it to newElement
                    if (newElement != null) //if i manage to find a color that matches bitmap
                    {
                        float xPosition = WorldStartPosition.x + (defaultTile.bounds.size.x * x); //calculate x position of tile
                        float yPosition = WorldStartPosition.y + (defaultTile.bounds.size.y * y); //calculate y position of tile

                        GameObject obj = Instantiate(newElement.MyElementPrefab); //create tile

                        if(newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), obj);
                        }


                        obj.transform.position = new Vector2(xPosition, yPosition); //set tile position
                        if (newElement.MyTileTag == "Tree01")
                        {
                            obj.GetComponent<SpriteRenderer>().sortingOrder = h * 2 - y * 2; //check tag, if it is a tree, set sorting order to 10
                        }


                        obj.transform.parent = map; //put genereted tiles in map folder of hierarchy


                    }
                }
            }
        }
    }
    private void WaterCheck()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles) //run through every tile in watertiles, everytime i find an object inside watertiles i refer to it eith a variable called tile and that on that tile i have a point which is a key and a value which is a gameobject
        {
            string compo = TileCheck(tile.Key);
        }
    }


    public string TileCheck(Point currentP)
{
        string comp = string.Empty;

        for(int x = -1; x <=1; x++){
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) //dont check myself, central tile
                {
                    if(waterTiles.ContainsKey(new Point(currentP.MyXpos+x, currentP.MyYpos + y)))
                    {
                        comp += "W";
                    }
                    else
                    {
                        comp += "E";
                    }
                }
            }
        }
        Debug.Log(comp);
        return comp;
}
}





[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;
    [SerializeField]
    private Color color;
    [SerializeField]
    private GameObject elementPrefab;

    public string MyTileTag { get => tileTag; }
    public Color MyColor { get => color; }
    public GameObject MyElementPrefab { get => elementPrefab; }
}

public struct Point
{
    public int MyXpos { get; set; }
    public int MyYpos { get; set; }
    
    public Point(int x, int y) //constructor --> each time i create a point, it sets the values
    {
        this.MyXpos = x;
        this.MyYpos = y;
    }

}
