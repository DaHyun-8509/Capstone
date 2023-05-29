using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    private GameObject crop;
    public float genertateTime = 3f;

    private bool isGrown;
    public bool IsGrown
    {
        get { return isGrown; }
        set { isGrown = value; }
    }

    private void Start()
    {
        crop = transform.Find("Cabbage").gameObject;
        isGrown = true;
    }


    public IEnumerator GrowTreeAfterDelay()
    { //작물 다시 자란 상태로
        yield return new WaitForSeconds(genertateTime);

        //일정 시간 후 보이게
        crop.SetActive(true);
        isGrown = true;
    }

}
