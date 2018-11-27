using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // private void Update()
    // {
    //     if (Input.GetButtonDown("Escape"))
    //     {
    //         this.ChangeToMainMenu();
    //     }
    // }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ChangeSceneByID(int id)
    {
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    }
}
