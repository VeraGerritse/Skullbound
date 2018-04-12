using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
    public Image bar;
    public List<string> loadText = new List<string>();
    public Text text;
    public static LoadingScreen instance;
    public Canvas canvas;
    float max;
    float current;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        max = loadText.Count;
    }



    public void UpdateLoad()
    {
        if(current == max)
        {
            canvas.enabled = false;
            return;
        }
        bar.fillAmount = current / max;
        if (current != max)
        {
            current++;
        }
        if(current != 0)
        {
            int curr = Mathf.RoundToInt( current);
            text.text = loadText[curr -1];
        }
        bar.fillAmount = current / max;
    }
}
