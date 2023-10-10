using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{

    float timeElapsed = 0.0f;
    float dayDuration = 600.0f;
    bool isTimeRunning = true;

    public bool IsRunning { get { return isTimeRunning; } set { isTimeRunning = value; } }

    int hour = 0;
    int day = 0;

    string[] days = { "월", "화", "수", "목", "금", "토", "일" };

    public void Update()
    {
        if(isTimeRunning)
        {
            timeElapsed += Time.deltaTime;
            hour = (int)(timeElapsed * 24 / dayDuration);

        }

        if (timeElapsed > dayDuration)
        {
            hour = 0;
            timeElapsed = 0;
            day++;
            if (day >= days.Length)
                day = 0;
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }
    public void RunTime()
    {
        Time.timeScale = 1;
    }

    public int GetHour() { return hour; }

    public int GetOneHourTime()
    {
        return (int)dayDuration / 24;
    }

    public string GetDay() 
    {
        return days[day];
    }
}
