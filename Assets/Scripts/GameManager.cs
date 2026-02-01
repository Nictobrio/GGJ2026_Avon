using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float timer = 60f;
    public float accTime = 0f;
    public GameObject guard = null;
    public void startLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void exit()
    {
        Application.Quit();
        Debug.Log("salio");
    }
    

    // Update is called once per frame
    void Update()
    {
        accTime += Time.deltaTime;

        if (accTime > timer && guard != null)
        {
            guard.gameObject.SetActive(true);

        }
    }
}