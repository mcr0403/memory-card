using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FLMenu : MonoBehaviour
{
    public AudioSource soundBG;

    // Start is called before the first frame update
    void Start()
    {
        soundBG.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartBtn()
    {
        SceneManager.LoadScene("FlipCard");
    }

    public void OnExitBtn()
    {
        Destroy(soundBG);
        Application.Quit();
    }

    
}
