using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class story : MonoBehaviour
{
    public Camera cam;
    public GameObject storys;
    public Image last;
    public int zoom = 20, smooth = 5;
    public static float speed = 0.004f;
    public bool isPointer;
    public Text loading;
    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        storys.gameObject.SetActive(true);
        last.gameObject.SetActive(false);
        StartCoroutine(play());

    }

    // Update is called once per frame
    void Update()
    {
        if (isPointer)
        {
            speed = 0.0001f;
        }
        else
        {
            speed = 0.005f;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
    IEnumerator play()
    {
        yield return new WaitForSeconds(1f);
        float i = 0;
        cam.transform.position = new Vector3(-19, 0, -1);

        while (cam.transform.position.x < 19.5)
        {
            i += 0.03f; //다해 - 속도 높였음
            cam.transform.position = new Vector3(-19 + i, 0, -1);
            yield return new WaitForSeconds(speed);
        }
        for (int j = 0; j < 10; j++)
        {
            bgm.volume -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);

        storys.gameObject.SetActive(false);
        last.gameObject.SetActive(true);
        string sentence = "Loading...";
        for (int j = 0; j < sentence.Length; j++)
        {
            loading.text = sentence.Substring(0, j + 1);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("farm");

    }
    public void enter()
    {
        isPointer = true;
    }
    public void exit()
    {
        isPointer = false;
    }

    public void skip()
    {
        SceneManager.LoadScene("farm");
    }
}
