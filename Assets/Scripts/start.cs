using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class start : MonoBehaviour
{
    public Image seagall1, seagall2, seagall3, seagall4, seagall5, start_button;
    public Text press_to_start;
    public GameObject title, human, rock;
    public GameObject[] cloud;
    public AudioSource bgm;


    void Start()
    {
        StartCoroutine(Image_active());
        start_button.gameObject.SetActive(false);
        seagall1.gameObject.SetActive(false);
        seagall2.gameObject.SetActive(false);
        seagall3.gameObject.SetActive(false);
        seagall4.gameObject.SetActive(false);
        seagall5.gameObject.SetActive(false);
        press_to_start.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        human.gameObject.SetActive(false);
        rock.gameObject.SetActive(false);
        cloud[0].gameObject.SetActive(false);
        cloud[1].gameObject.SetActive(false);
        cloud[2].gameObject.SetActive(false);
        cloud[3].gameObject.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator Image_active()
    {
        yield return new WaitForSeconds(0.2f);
        cloud[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cloud[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cloud[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        cloud[3].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        rock.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        human.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        title.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);
        StartCoroutine(seagall());
        StartCoroutine(another_seagall());

        yield return new WaitForSeconds(0.8f);
        StartCoroutine(press());

        

    }
    IEnumerator start_game()
    {
        yield return new WaitForSeconds(1f);
    }
    IEnumerator press()
    {
        start_button.gameObject.SetActive(true);
        while (true)
        {
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
        yield return new WaitForSeconds(a / 100);
        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                posit.transform.localPosition = new Vector3(304, (float)(176 + i * 0.06 + a / 100), 0);
                yield return new WaitForSeconds(0.01f);
            }
            for (int i = 0; i < 100; i++)
            {
                posit.transform.localPosition = new Vector3(304, (float)(182 - i * 0.06 + a / 100), 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void next_scene()
    {
        int isNew = PlayerPrefs.GetInt("isNew", 1);
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
