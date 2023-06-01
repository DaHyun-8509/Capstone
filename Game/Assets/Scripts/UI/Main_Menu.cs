using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickNewGame()
    {
        Debug.Log("새 게임");
    }
    public void OnClickLoad()
    {
        Debug.Log("불러오기");
    }
    public void OnClickOption()
    {
        Debug.Log("환경설정");
    }
    public void OnClickQuit()
    {
        Debug.Log("게임종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
