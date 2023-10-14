using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{

    float totalTimeElapsed = 0.0f;
    float dayTimeElapsed = 0f;
    float dayDuration = 600.0f;
    public float DayDuration { get { return dayDuration; } }
    bool isTimeRunning = true;

    public bool IsRunning { get { return isTimeRunning; } set { isTimeRunning = value; } }

    int hour = 0;
    int day = 0;

    string[] days = { "월", "화", "수", "목", "금", "토", "일" };


    public void Update()
    {
        
        if(isTimeRunning)
        {
            totalTimeElapsed += Time.deltaTime;
            dayTimeElapsed += Time.deltaTime;
            hour = (int)(dayTimeElapsed * 24 / dayDuration);

        }

        if (dayTimeElapsed > dayDuration)
        {
            hour = 0;
            dayTimeElapsed = 0;
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
