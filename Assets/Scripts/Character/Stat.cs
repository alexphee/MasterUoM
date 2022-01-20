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

    private float overflow;
    public float MyOverflow
    {
        get
        {
            float tempOverflow = overflow;
            overflow = 0;
            return tempOverflow;
        }
    }
    public float MyCurrentValue {
        get { return currentValue; }
        set
        {
            if (value > MyMaxValue) // current value never exceeds max
            {
                overflow = value - MyMaxValue; //save the excess xp so i assign it to the xp bar at the new level, after reset
                currentValue = MyMaxValue;
            }
            else if (value < 0)  //i dont allow negative health values
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue; //result will be a value between 0-1

            if (statValue != null) // necessary because enemy healthbar has no script so i get nullreference exception
            {
                statValue.text = currentValue + "/" + MyMaxValue; //text in bar
            }
        }
    }
    public bool IsXPFull
    {
        get
        {
            return content.fillAmount == 1; //if 1 the bar is full
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
        //Debug.Log(MyCurrentValue);

        if (currentFill != content.fillAmount) //smoother
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed); //move equaly in every device (deltaTime) //Replaced Lerp with MoveTowards bc of the constant speed
        }
        //content.fillAmount = currentFill;
    }
    //initialize stats
    public void Initialize (float currentValue, float maxValue) 
    {
        if (content == null) { content = GetComponent<Image>(); } //if content doesn't exist i get it here, else i get NullRef exception
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue; //this is so the healthbar is already filled to max when i chose target, else it slowly fills from zero
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
