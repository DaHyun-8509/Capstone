using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
    public float generateTime = 3f;

    private bool isGrown;
    public bool IsGrown
    {
        get { return isGrown; }
        set { isGrown = value; }
    }

    private void Start()
    {
        isGrown = true;
    }


    public IEnumerator GrowCropAfterDelay()
    { //작물 다시 자란 상태로
        yield return new WaitForSeconds(generateTime);

        //일정 시간 후 보이게
        gameObject.SetActive(true);
        isGrown = true;
    }

}
