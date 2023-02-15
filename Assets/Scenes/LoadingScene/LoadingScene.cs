using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    static int sceneID;
    public static void LoadTo(int ID)
    {
        sceneID = ID;
        SceneManager.LoadScene( (int)SceneLoader.Scenes.Loading );
    }


    [SerializeField] Slider loadSlider;
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.25f);

        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneID);
        while (!oper.isDone)
        {
            float progress = oper.progress / 1;
            loadSlider.value = progress;
            yield return null;
        }
    }
}
