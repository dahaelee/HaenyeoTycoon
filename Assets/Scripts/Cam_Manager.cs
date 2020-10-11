using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cam_Manager : MonoBehaviour
{
    public Camera mainCam;
    public Slider swtich;
    bool isMoving = false;
    Vector3 farm_position = new Vector3(-640,0,-1000), beach_position = new Vector3(640,0,-1000);

    private Vector2 startPos, endPos;
    public float sensitivity = 10;
    

    void Update()
    {/*
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
        if (swtich.value == 0)
        {
            
        }
    }



    public void go_farm_func()
    {
        StartCoroutine(go_farm());
    }
    public void go_beach_func()
    {
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

                LerpT += Time.deltaTime * speed;
                yield return null;
            }
            mainCam.transform.localPosition = farm_position;
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

                LerpT += Time.deltaTime * speed;
                yield return null;
            }
            mainCam.transform.localPosition = beach_position;
            yield return new WaitForSeconds(0.3f);
        }
        isMoving = false;
    }
}
