using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class HighescoreText : MonoBehaviour
{
    Text highscore;
    // Start is called before the first frame update
    void OnEnable()
    {
        highscore = GetComponent<Text>();
        highscore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
