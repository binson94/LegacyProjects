using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject UICanvas;
    public GameObject BackGround;

    public void Awake()
    {
        Screen.SetResolution(Screen.height * 1920 / 1080, Screen.height, true);
    }

    public void Btn_Start()
    {
        UICanvas.SetActive(false);
        StartCoroutine(RotateBackground());
    }

    IEnumerator RotateBackground()
    {
        for (float i = 0; i <= 90; i++)
        {
            Debug.Log(i);
            BackGround.transform.Rotate(Vector3.up);
            yield return new WaitForSeconds(0.03f);
        }
        BackGround.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        SceneManager.LoadScene(1);
    }

    public void Btn_Exit()
    {
        Application.Quit();
    }
}
