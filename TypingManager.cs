using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class TypingManager : MonoBehaviour
{
    public List<Word> words; //words displayed in inspector

    public TextMeshProUGUI display;

    [SerializeField] private Animator anim;
    [SerializeField] private Animator textAnim;

    public int wordCount;
    public TextMeshProUGUI wordText;

    public bool isDone;

    //public TextMeshProUGUI greyDisplay;

    public void Awake()
    {
        display.text = null;
        isDone = false;
        //greyDisplay.text = "bunnies";
    }
    private void Update()
    {
            InputText();
            //GreyText(); 
    }
void InputText()
    {
        string input = Input.inputString; 
        if (input.Equals("")) //if not typing
            return; //stops above function
        
        char c = input[0];
        string typing = "";
        for (int i = 0; i < words.Count; i++)
        {
            Word w = words[i];
            if (w.ContinueText(c))
            {
                SoundManager1.instance.PlayText();
                string typed = w.GetTyped();
                typing += typed + "|";
                if (typed.Equals(w.text))
                {
                    words.Remove(w);
                    DecreaseWordCount();
                    break;
                }
            }
        }
        display.text = typing;
        if (words.Count == 0) //when all words done
        {
           isDone = true;
           anim.Play("FadeEnd");
           SoundManager1.instance.PlayAchievementSound();
           StartCoroutine(EndText());
        }
        if (isDone)
        {
            StartCoroutine(Loader());
        }
    }

    IEnumerator Loader()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("end"); //this works to load scene after x amount of time
    }

    IEnumerator EndText()
    {
        yield return new WaitForSeconds(2);
        textAnim.Play("TextFade");

    }

    public void DecreaseWordCount()
    {
        wordCount -= 1;
        wordText.text = $"{wordCount} left";
        SoundManager1.instance.PlayCount();
    }
    /*void GreyText()
    {
        string greyer = display.text;
        greyDisplay.text = greyer; //displays words in grey but after typing :((((


    }*/
}


[System.Serializable]
public class Word
{
    public string text;
    public UnityEvent onTyped; //can add other events this way too
    public UnityEvent onTyper;
    string hasTyped = "";
    int curChar = 0;

    public Word(string t)
    {
        text = t;
        hasTyped = "";
        curChar = 0;
    }

    public bool ContinueText(char c)
    {
        if (c.Equals(text[curChar]))
        {
            {
                onTyper.Invoke();
            }
        }
        if (c.Equals(text[curChar]))
        {
            curChar++;
            hasTyped = text.Substring(0, curChar);
            if (curChar >= text.Length) //has typed whole word
            {
                onTyped.Invoke(); //invokes function below of resetting the current character 
                curChar = 0;
            }

            return true; //restarts the loop
        }
        else
        {
            curChar = 0;
            hasTyped = "";
            return false;
        }
    }

    public string GetTyped()
    {
        return hasTyped;
    }
}