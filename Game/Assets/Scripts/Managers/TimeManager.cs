using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{

    float timeElapsed = 0.0f;
    float dayDuration = 600.0f;

    int hour = 0;
    public void Update()
    {

        timeElapsed += Time.deltaTime;
        hour = (int)(timeElapsed * 24 / dayDuration);

        if (timeElapsed > dayDuration)
        {
            timeElapsed = 0;
            hour = 0;
        }
    }

    public int GetHour() { return hour; }
}
