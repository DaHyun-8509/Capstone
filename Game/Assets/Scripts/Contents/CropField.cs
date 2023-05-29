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
    { //�۹� �ٽ� �ڶ� ���·�
        yield return new WaitForSeconds(genertateTime);

        //���� �ð� �� ���̰�
        crop.SetActive(true);
        isGrown = true;
    }

}
