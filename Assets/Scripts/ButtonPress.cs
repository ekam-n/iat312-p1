using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonPress : MonoBehaviour
{
    public void startButton()
    {
        SceneManager.LoadScene(2);
    }

    public void backButton()
    {
        SceneManager.LoadScene(0);
    }

    public void HTPButton()
    {
        SceneManager.LoadScene(1);
    }
}
