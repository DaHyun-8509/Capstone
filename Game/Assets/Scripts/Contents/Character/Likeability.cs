using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Likeability : MonoBehaviour
{
    [SerializeField]
    float like = 0;
    public float Like {  get { return like; } 
        set 
        { 
            like = value;
            SetGrade();
        } 
    }

    [SerializeField]
    int grade = 0;
    public int Grade { get {  return grade; }}

    public void Increase(float amount)
    {
        like += amount;
        SetGrade();
    }

    private void SetGrade()
    {
        if (like >= 2)
        {
            grade = 1;
        }
        if (like >= 20)
        {
            grade = 2;
        }
        if (like >= 30)
        {
            grade = 3;
        }
        if (like >= 40)
        {
            grade = 4;
        }
    }
}
