using UnityEngine;
using UnityEngine.SceneManagement;
// SceneLoader - ��� �������� ���� � ��������
// ��������������� ����� ����

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
    /// ������� �� ������� ���� � ����������� �������
    /// </summary>
    public static void LoadMenu_WithLoading() => LoadWithLoadingWindow(Scenes.Menu);

    /// <summary>
    /// ������� �� ������� ����
    /// </summary>
    public static void LoadGame() => Load(Scenes.Game);

    /// <summary>
    /// ������� �� ������� ���� � ����������� �������
    /// </summary>
    public static void LoadGame_WithLoading() => LoadWithLoadingWindow(Scenes.Game);

    /// <summary>
    /// ������� �� ���� ���������
    /// </summary>
    public static void LoadRestartWindow() => Load(Scenes.Restart);
}
