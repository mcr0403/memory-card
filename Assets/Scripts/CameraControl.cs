using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    public GameObject uiStartGame;
    private bool isStart = false;
    private float xAngle;
    public GameObject time;
    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        xAngle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (transform.position.z < -2.1f)
            {
                transform.position += new Vector3(0f, 0f, 2f);
            }
            else if (xAngle < 75f)
            {
                transform.rotation = Quaternion.Euler(xAngle += 2, 0, 0);
            }
            else
            {
                time.SetActive(true);
            }
        }
    }

}
