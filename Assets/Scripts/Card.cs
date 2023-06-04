using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool cardUp = false;
    public float zAxis = 0f;
    public TMP_Text text;
    public GameObject image;
    public string lessonName;

    public bool checkMatch = true;
    public List<Sprite> cardImages;
    public List<string> cardTexts;
    public Data[] vocabularies;
    public List<Data> vocabulariesGetByName;
    public int[] indexGetWord;
    public string index;
    public int indexNumber;
    public string indexText;
    private GameObject controller;
    public bool twoCardUp = false;
    private bool checkSpam = false;

    private Vector3 pickEnd;
    private Vector3 pickStart;
    private float pickDuration = .5f;
    private float pickTime = 0f;
    private bool picking = false;
    private bool putDown = false;
    private void Awake()
    {
        controller = GameObject.Find("Control");
    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (var vocab in vocabularies)
        {
            if (vocab.session == lessonName)
            {
                vocabulariesGetByName.Add(vocab);
            }
        }

        foreach(int i in indexGetWord)
        {
            cardImages.Add(vocabulariesGetByName[i].image);
            cardTexts.Add(vocabulariesGetByName[i].name);
        }
        
        string[] tempIndex = index.Split('-');
        indexNumber = Int32.Parse(tempIndex[1]);
        indexText = (string)tempIndex[0];
        if (tempIndex[0] == "T")
        {
            image.SetActive(false);
            text.text = cardTexts[indexNumber];
        } else
        {
            text.text = "";
            image.GetComponent<SpriteRenderer>().sprite = cardImages[indexNumber];
        }

        pickEnd = new Vector3(transform.position.x, transform.position.y + .7f, transform.position.z);
        pickStart = new Vector3(transform.position.x, 0f, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        //return back of card
        if (!checkMatch)
        {
            if (picking)
            {
                pickTime = 0f;
                while (pickTime < pickDuration)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pickEnd, pickTime / (pickDuration * 8));
                    pickTime += Time.deltaTime;
                }
                picking = false;
            }
            else
            {
                if (zAxis > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, zAxis -= 15);
                } else
                {
                    putDown = true;
                }
            }

            if (putDown)
            {
                pickTime = 0f;
                while (pickTime < pickDuration)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pickStart, pickTime / (pickDuration * 8));
                    pickTime += Time.deltaTime;
                }
                putDown = false;
                checkMatch = true;
                checkSpam = false;
            }
        }

        //show face of card
        if (cardUp)
        {
            if (picking)
            {
                pickTime = 0f;
                while (pickTime < pickDuration)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pickEnd, pickTime/ (pickDuration * 8));
                    pickTime += Time.deltaTime;
                }
                picking = false;
            } else
            {
                if (zAxis < 180)
                {
                    transform.rotation = Quaternion.Euler(0, 0, zAxis += 15);
                } else
                {
                    putDown = true;
                }
            }

            if (putDown)
            {
                pickTime = 0f;
                while (pickTime < pickDuration)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pickStart, pickTime / (pickDuration * 8));
                    pickTime += Time.deltaTime;
                }
                putDown= false;
                cardUp = false;
            }

        }
    }

    //click to flip and show face of card
    private void OnMouseDown()
    {
        if (!checkSpam && !controller.GetComponent<FlipCardControl>().TwoCardUp())
        {
            checkSpam = true;
            cardUp = true;
            picking = true;
            controller.GetComponent<FlipCardControl>().CardsChoosing(this);
            controller.GetComponent<FlipCardControl>().flipSound.Play();
        }
    }

    public void ReturnCard()
    {
        checkMatch = false;
        picking = true;
    }
}
