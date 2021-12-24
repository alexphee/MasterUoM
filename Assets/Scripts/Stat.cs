using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content; //the image i change the fill of
    [SerializeField]
    private Text statValue; //stat texts
    [SerializeField]
    private float lerpSpeed;
    private float currentFill;
    public float MyMaxValue { get; set; }
    private float currentValue;

    public float MyCurrentValue {
        get { return currentValue; }
        set { 
            if (value > MyMaxValue) // current value never exceeds max
            {
                currentValue = MyMaxValue;
            }else if (value < 0)  //i dont allow negative health values
            {
                currentValue = 0;
            }else
            {
                currentValue = value;
            }
            
            currentFill = currentValue / MyMaxValue; //result will be a value between 0-1

            statValue.text = currentValue + "/" + MyMaxValue; //text in bar
        } 
    }

     
    // Start is called before the first frame update
    void Start()
    {
       
            content = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(MyCurrentValue);

        if (currentFill != content.fillAmount) //smoother
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed); //move equaly in every device (deltaTime)
        }
        //content.fillAmount = currentFill;
    }
    //initialize stats
    public void Initialize (float currentValue, float maxValue) 
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
