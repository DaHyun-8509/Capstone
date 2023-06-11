using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Saveplayer()
    {
        SaveData save = new SaveData();
        save.x = transform.position.x;
        save.y = transform.position.y;
        save.z = transform.position.z;

        SaveManager.Save(save);

    }

    public void LoadPlayer()
    {
        SaveData save = SaveManager.Load();
        transform.position = new Vector3(save.x, save.y, save.z);
    }
}
