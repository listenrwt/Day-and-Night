using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public SceneLoadingManager SLM;
    public float stayTime;
    private float Timer = 0;

    private void Update()
    {
        if(Timer < stayTime)
        {
            Timer += Time.deltaTime;
        }else
        {
            SLM.NextScene();
        }
    }

}
