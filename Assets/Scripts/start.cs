using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public Image bg, seagall1, seagall2, seagall3, seagall4, seagall5;
    public Text press_to_start;
    public GameObject moving1, moving2;
    public AudioSource bgm;

    void Start()
    {
        bg.gameObject.SetActive(true);
        press_to_start.gameObject.SetActive(false);
        StartCoroutine("press");
        StartCoroutine("seagall");
        StartCoroutine(move(moving1));
        StartCoroutine(move(moving2));
        StartCoroutine(another_seagall());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    IEnumerator press()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            press_to_start.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            press_to_start.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            press_to_start.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            press_to_start.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            press_to_start.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);



        }
    }

    IEnumerator seagall()
    {
        while (true)
        {
            
            seagall1.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            seagall1.gameObject.SetActive(false);
            seagall2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            seagall2.gameObject.SetActive(false);
            seagall3.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            seagall3.gameObject.SetActive(false);
            seagall2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            seagall2.gameObject.SetActive(false);
            seagall1.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            seagall1.gameObject.SetActive(false);
            seagall2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            seagall2.gameObject.SetActive(false);
            seagall3.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            seagall3.gameObject.SetActive(false);
            seagall2.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            seagall2.gameObject.SetActive(false);
   

        }
    }

    IEnumerator another_seagall()
    {
        while (true)
        {
            seagall5.gameObject.SetActive(false);
            seagall4.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            seagall4.gameObject.SetActive(false);
            seagall5.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
    
    IEnumerator move(GameObject posit)
    {
        float a = Random.Range(0, 100);
        yield return new WaitForSeconds(a/100);
        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                posit.transform.localPosition = new Vector3(304, (float)(176+i*0.06+a/100), 0);
                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < 100; i++)
            {
                posit.transform.localPosition = new Vector3(304, (float)(182 - i * 0.06+a/100), 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void next_scene()
    {
        int isNew =PlayerPrefs.GetInt("isNew",1);
        if (isNew == 0)     //처음이 아니면 바로 양식장 씬
        {
            SceneManager.LoadScene("farm");
        }
        else
        {                           //처음이면 스토리 씬
            SceneManager.LoadScene("story");
            PlayerPrefs.DeleteAll();
        }
    }



}
