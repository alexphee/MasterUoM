using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text price;
    [SerializeField]
    private Text quantity;

    //i will make a function to set everything up
    public void AddItem(VendorItem Vitem)
    {
        gameObject.SetActive(true);
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
