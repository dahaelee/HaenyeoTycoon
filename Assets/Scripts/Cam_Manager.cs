using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cam_Manager : MonoBehaviour
{
    public Camera mainCam;
    public Scrollbar Switch;
    private CamPos camPos = CamPos.farm;
    bool isMoving = false;
    Vector3 farm_position = new Vector3(-640,0,-1000), beach_position = new Vector3(640,0,-1000);

    private Vector2 startPos, endPos;
    public float sensitivity = 10;
    
    enum CamPos
    {
        farm,
        beach
    }

    void Start()
    {
        dataLoad();
        if(camPos == CamPos.beach)
        {
            Switch.value = 1;
            mainCam.transform.localPosition = beach_position;
        }
        else
        {
            Switch.value = 0;
            mainCam.transform.localPosition = farm_position;
        }
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            float distance = Vector2.Distance(startPos, endPos);
            if(distance > sensitivity) {
                if (startPos.x < endPos.x)
                {
                    go_beach_func();
                }
                else
                {
                    go_farm_func();
                }
            }
        }
        */
    }

    public void switcher()
    {
        if (!isMoving)
        {
            if (camPos == CamPos.farm)
            {
                Switch.value = 1;
                go_beach_func();
            }
            else
            {
                Switch.value = 0;
                go_farm_func();
            }
        }
        dataSave();
    }



    public void go_farm_func()
    {
        camPos = CamPos.farm;
        StartCoroutine(go_farm());
    }
    public void go_beach_func()
    {
        camPos = CamPos.beach;
        StartCoroutine(go_beach());
    }

    public IEnumerator go_farm()
    {
        if (!isMoving)
        {
            isMoving = true;
            float LerpT = 0;
            float speed = 3;

            while (LerpT <= 1)
            {
                mainCam.transform.localPosition = Vector3.Lerp(beach_position, farm_position, LerpT);
                Switch.value = 1- LerpT;
                LerpT += Time.deltaTime * speed;
                yield return null;
            }
            mainCam.transform.localPosition = farm_position;
            Switch.value = 0;
            yield return new WaitForSeconds(0.3f);
        }
        isMoving = false;
    }

    public IEnumerator go_beach()
    {
        if (!isMoving)
        {
            isMoving = true;
            float LerpT = 0;
            float speed = 3;

            while (LerpT <= 1)
            {
                mainCam.transform.localPosition = Vector3.Lerp(farm_position, beach_position, LerpT);
                Switch.value = LerpT;
                LerpT += Time.deltaTime * speed;
                
                yield return null;
            }
            Switch.value = 1;
            mainCam.transform.localPosition = beach_position;
            yield return new WaitForSeconds(0.3f);
        }
        isMoving = false;
    }

    void dataSave()
    {
        PlayerPrefs.SetInt("camPos", (int)camPos);
    }

    void dataLoad()
    {
        camPos = (CamPos)PlayerPrefs.GetInt("camPos", 0);
    }
}
