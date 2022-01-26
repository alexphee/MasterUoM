using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;
    public static SpellBook MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }
            return instance;
        }
    }
    [SerializeField]
    private Image castingBar;
    [SerializeField]
    private Text spellName;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Spell[] spells;
    [SerializeField]
    private Text castTime;
    [SerializeField]
    private CanvasGroup canvasGroup;
    private Coroutine spellRoutine;
    private Coroutine fadeRoutine;

    public Spell CastSpelll(int index)
    {
        
        Color tmp = castingBar.color;
        //tmp.a = 1;
        tmp = spells[index].MyBarColor;
        
        castingBar.color = tmp;
        castingBar.fillAmount = 0;
        spellName.text = spells[index].MyName;
        icon.sprite = spells[index].MyIcon;

        spellRoutine = StartCoroutine(Progress(index)); //i have to do this so i can cancell casting if moved or sth
        fadeRoutine = StartCoroutine(FadeBar());
        return spells[index];
    }

    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime; //time left for casting
        float rate = 1.0f / spells[index].MyCastTime; //the rate the bar fills, based on the cast time of the spell //divide the max with the casttime
        float progress = 0.0f; //how far this is filled, when its 1 its casted
        while (progress <= 1.0)//as long as bar is not maxed out
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress); //move from 0 (min) to 1 (max) [the bar fill values] 
            progress += rate * Time.deltaTime;
            timePassed += Time.deltaTime; //increase over time passed
            castTime.text = (spells[index].MyCastTime - timePassed).ToString("f2"); ; //the cast time of the spell - the time pased //with 2 decimal
            if (spells[index].MyCastTime - timePassed < 0)
            {
                castTime.text = "0.0"; //BUGFIX so it doesnt end at -0.0
            }
            yield return null; //dont wait for any sec
        }
        StopCasting(); //reset routine
    }

    public void StopCasting()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null; //remove ref
        }
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    private IEnumerator FadeBar()
    {

        float rate = 1.0f / 0.3f;
        float progress = 0.0f;
        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime; 
            yield return null;
        }

       
    }
}
