using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class FlipCardControl : MonoBehaviour
{

    public GameObject card;

    System.Random rdm = new System.Random();
    List<string> faceIndexes = new List<string>() { "T-0", "T-1", "T-2", "T-3", "T-4", "T-5", "T-6", "T-7", 
                                                    "I-0", "I-1", "I-2", "I-3", "I-4", "I-5", "I-6", "I-7"};

    private int randomIndex;

    private Card firstCard;
    private Card secondCard;
    private float timeRemaining;
    public TMP_Text timeValue;
    private bool isStart = false;
    private int correct = 0;
    private bool over = false;
    GameObject[] clear;
    public AudioSource flipSound;
    public GameObject star;
    private int stars;


    [Header("RESULT")]
    public GameObject resultPanel;
    public GameObject list;
    public GameObject vocabulary;
    private GameObject text;
    private GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        int[] randomNumbers = Enumerable.Range(0, 19).OrderBy(x => Guid.NewGuid()).Take(8).ToArray();

        card.GetComponent<Card>().indexGetWord = randomNumbers;

        float xPosition = -1.7925f;
        float zPosition = 2.25f;
        int count = 0;
        timeRemaining = 30f;

        for (int i = 0; i < 16; i++)
        {
            int totalIndex = faceIndexes.Count;
            randomIndex = rdm.Next(0, totalIndex);

            //spawn card at position Vector3
            var cardSpawn = Instantiate(card, new Vector3(xPosition, 0, zPosition),  
                       Quaternion.identity);
            cardSpawn.GetComponent<Card>().index = faceIndexes[randomIndex];   
            faceIndexes.RemoveAt(randomIndex);

            xPosition = xPosition + 1.195f;
            count++;

            //change position spawn
            if (count == 4)     
            {
                xPosition = -1.7925f;
                zPosition = zPosition - 1.5f;
                count= 0;
            }
        }
        resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timeRemaining -= Time.deltaTime;
            timeValue.text = Mathf.FloorToInt(timeRemaining % 60).ToString();
            if (timeRemaining <= 0 || correct == 8)
            {
                timeValue.text = "0";
                GameOver();
            }
        }  
    }
    
    //compare cards
    IEnumerator CheckMatch()
    {
        if (firstCard.indexNumber == secondCard.indexNumber)
        {
            correct++;
           
            //add to list result
            var vocab = Instantiate(vocabulary, list.transform);
            image = vocab.transform.GetChild(1).gameObject;
            text = vocab.transform.GetChild(2).gameObject;
            
            if (firstCard.indexText=="I")
            {
                image.GetComponent<Image>().sprite = firstCard.image.GetComponent<SpriteRenderer>().sprite;
                text.GetComponent<TMP_Text>().text = secondCard.text.text;
            } else
            {
                image.GetComponent<Image>().sprite = secondCard.image.GetComponent<SpriteRenderer>().sprite;                
                text.GetComponent<TMP_Text>().text = firstCard.text.text; 
            }

        } else
        {
            yield return new WaitForSeconds(.5f);

            firstCard.ReturnCard();
            secondCard.ReturnCard();
        }
        firstCard = null;
        secondCard = null;
    }

    public void CardsChoosing(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
            if (!isStart)
                isStart = true;
        } else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    public bool TwoCardUp()
    {
        if (!over)
        {
            bool twoCardUp = false;
            if (firstCard != null && secondCard != null)
            {
                twoCardUp= true;
            }
            return twoCardUp;
        }
        return true;
    }

    public void GameOver()
    {
        isStart = false;
        over = true;
        resultPanel.SetActive(true);

        if (correct > 0 && correct <= 5)
        {
            stars = 1;
        }
        if (correct > 5 && correct <= 7)
        {
            stars = 2;
        }
        if (correct > 7)
        {
            stars = 3;
        }

        for (int i = 0; i < stars; i++) {
            var starSpawn = Instantiate(star, GameObject.Find("Canvas/Panel/GameOver/Score").transform);
        }

        ////compare stars with highest stars
        //if (ChooseLesson.instance.stars < stars)
        //{
        //    StartCoroutine(PutRequest());
        //}
    }

    //IEnumerator PutRequest()
    //{
    //    int lessonId = ChooseLesson.instance.lessonId;
    //    string gameType = ChooseLesson.instance.gameType;

    //    string url = "https://localhost:7079/api/Starearn?lessonid=" + lessonId + "&gametype=" + gameType + "&userid=2";

    //    string jsonData = "{\"starEarnId\": 0," +
    //                       "\"lessonId\": 0," +
    //                       "\"gameType\": \"" + gameType + "\"," +
    //                       "\"stars\": \"" + stars + "\"," +
    //                       "\"userId\": 0 }";

    //    byte[] raw = Encoding.UTF8.GetBytes(jsonData);

    //    UnityWebRequest request = UnityWebRequest.Put(url, raw);

    //    request.SetRequestHeader("Content-type", "application/json");

    //    yield return request.SendWebRequest();

    //    if (request.isHttpError || request.isNetworkError)
    //    {
    //        Debug.Log(request.error);
    //    }
    //    else
    //    {
    //        Debug.Log(request.downloadHandler.text);
    //    }
    //}

    public void OnPlayAgainBtn()
    {
        SceneManager.LoadScene("FlipCard");
    }

    public void ExitBtn()
    {
        
        SceneManager.LoadScene("FCMenu");

    }


}
