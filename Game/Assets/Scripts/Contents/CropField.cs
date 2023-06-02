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
    { //�۹� �ٽ� �ڶ� ���·�
        yield return new WaitForSeconds(generateTime);

        //���� �ð� �� ���̰�
        gameObject.SetActive(true);
        isGrown = true;
    }

}
