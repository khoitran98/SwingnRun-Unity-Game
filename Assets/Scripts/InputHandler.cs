using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class InputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private int score;
    private string name;
    public Text Username_field;
    public GameObject warningText, successText, longText, shortText;
    void Start ()
    {
        // var input = gameObject.GetComponent<InputField>();
        // input.onEndEdit.AddListener(SaveGame);  
    }
    public void SaveGame()
    {
        score = GameObject.Find("Main Camera").GetComponent<GameManager>().score;
        name = Username_field.text.ToString();
        StartCoroutine("Save");
    }
    IEnumerator Save ()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("?name=" + name + "&score=" + score,"dummy"))
        {
            if (name.Length > 8)
            {
                longText.SetActive(true);
                yield return new WaitForSecondsRealtime(1);
                longText.SetActive(false);
            }
            else if (name.Length < 3)
            {
                shortText.SetActive(true);
                yield return new WaitForSecondsRealtime(1);
                shortText.SetActive(false);
            }
            else {
                yield return www.Send();
                if (www.downloadHandler.text.ToString().Contains("unique"))
                {
                    warningText.SetActive(true);
                    yield return new WaitForSecondsRealtime(1);
                    warningText.SetActive(false);
                }
                else {
                    successText.SetActive(true);
                    yield return new WaitForSecondsRealtime(1);
                    successText.SetActive(false);
                }
            }
        }
    }

}
