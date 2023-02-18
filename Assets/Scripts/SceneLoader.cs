using UnityEngine;
using UnityEngine.SceneManagement;
// SceneLoader - all loading scenes and transitions
// implemented through it

public class SceneLoader : MonoBehaviour 
{
    public enum Scenes{
        Menu = 0,
        Loading = 1,
        Restart = 2,
        Game = 3
    }

    static void Load(Scenes ID)
    {
        int Scene = (int)ID;
        SceneManager.LoadScene(Scene);
    }

    static void LoadWithLoadingWindow(Scenes ID)
    {
        int SceneID = (int)ID;
        LoadingScene.LoadTo(SceneID);
    }

    /// <summary>
    /// Переход на главное меню с загрузочным экраном
    /// </summary>
    public static void LoadMenu_WithLoading() => LoadWithLoadingWindow(Scenes.Menu);

    /// <summary>
    /// Transition to the playing field
    /// </summary>
    public static void LoadGame() => Load(Scenes.Game);

    /// <summary>
    /// Transition to the playing field with a loading screen
    /// </summary>
    public static void LoadGame_WithLoading() => LoadWithLoadingWindow(Scenes.Game);

    /// <summary>
    /// Transition to the defeat window
    /// </summary>
    public static void LoadDefeatWindow() => Load(Scenes.Restart);
}
