using UnityEngine;

public class RestartScene : MonoBehaviour
{
    public void onButtonRestartPressed()
    {
        SceneLoader.LoadGame();
    }

    public void onButtonExitPressed()
    {
        SceneLoader.LoadMenu_WithLoading();
    }
}
