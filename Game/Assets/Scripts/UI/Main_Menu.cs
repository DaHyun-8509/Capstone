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
        Debug.Log("�� ����");
    }
    public void OnClickLoad()
    {
        Debug.Log("�ҷ�����");
    }
    public void OnClickOption()
    {
        Debug.Log("ȯ�漳��");
    }
    public void OnClickQuit()
    {
        Debug.Log("��������");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
