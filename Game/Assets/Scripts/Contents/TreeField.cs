using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeField : MonoBehaviour
{
    public GameObject grownTreePrefab;
    private GameObject currentTree;

    private bool isGrown ;

    public bool IsGrown
    { 
        get { return isGrown; }
        set { isGrown = value; }
    }

    private void Start()
    {
        currentTree = transform.Find("Tree_Apple").gameObject;
        isGrown = true;
    }



    public IEnumerator GrowTreeAfterDelay(float delay)
    { //���� �ٽ� �ڶ� ���·�
        yield return new WaitForSeconds(delay);

        Transform treeTransform = currentTree.transform;

        //������ �ִ� ���ŵ��� �����κ��� �и�
        treeTransform.DetachChildren();
        
        //���ο� ���� ����
        GameObject newTree = Instantiate(grownTreePrefab, treeTransform.position, treeTransform.rotation, transform);

        //���� ���� ����
        Destroy(currentTree);

        currentTree = newTree;

        isGrown = true;
    }

}
