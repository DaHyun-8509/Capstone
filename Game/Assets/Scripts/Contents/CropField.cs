using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropField : MonoBehaviour
{
    public float generateTime = 15f;

    private GameObject crop = null;
    private GameObject lv1 = null;
    private GameObject lv2 = null;
    private GameObject lv3 = null;

    public GameObject Crop { get { return crop; } set { crop = value; } }


    private bool isGrown;
    public bool IsGrown
    {
        get { return isGrown; }
        set { isGrown = value; }
    }

    private void Start()
    {
        isGrown = false;
    }

    public void PlantCrop(string name)
    {
        GameObject prefab = Resources.Load<GameObject>(name);
        crop = GameObject.Instantiate(prefab);
        crop.transform.SetParent(transform);

        //Lv별 작물 찾아두기 
        Transform[] children = crop.GetComponentsInChildren<Transform>();

        foreach(Transform child in children)
        {
            if (child.name == "Lv1")
                lv1 = child.gameObject;
            else if (child.name == "Lv2")
                lv2 = child.gameObject;
            else if (child.name == "Lv3")
                lv3 = child.gameObject;
        }

    }


    public IEnumerator GrowToLv2AfterDelay()
    { //작물을 Lv2로 
        yield return new WaitForSeconds(generateTime / 2);

        //일정 시간 후 보이게
        lv1.SetActive(false);
        lv2.SetActive(true);
        
    }

    public IEnumerator GrowToLv3AfterDelay()
    { //작물을 Lv3으로
        yield return new WaitForSeconds(generateTime / 2);

        //일정 시간 후 보이게
        lv2.SetActive(false);
        lv3.SetActive(true);

        isGrown = true;
    }

}
