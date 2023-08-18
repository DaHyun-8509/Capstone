using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    DataManager _data  = new DataManager();
    FieldManager _field = new FieldManager();
    InputManager _input = new InputManager();
    InventoryManager _inventory = new InventoryManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    TimeManager _time = new TimeManager();
    UIManager _ui = new UIManager();
    EnergyManager _energy = new EnergyManager();

    public static DataManager Data { get { return Instance._data; } }
    public static FieldManager Field { get { return Instance._field; } }
    public static InputManager Input { get { return Instance._input; } }
    public static InventoryManager Inventory { get { return Instance._inventory; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static EnergyManager Energy { get { return Instance._energy; } }

    void Start()
    {
        Init();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UI.Start();
        Energy.Start();
    }

    void Update()
    {
        Input.OnUpdate();
        Time.Update();
        UI.Update();
        Energy.Update();
    }

    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers"); 
            if(go == null) //@Managers 오브젝트가 없으면 추가
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);  //사라지지 않게 함
            s_instance = go.GetComponent<Managers>();   
        }
    }
}
