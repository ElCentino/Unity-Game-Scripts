using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class TimeClass : MonoBehaviour {

    private int hour;
    private int minute;
    private float seconds;
    private int time;

    public Text timer;
    public static float timeScale = 1;

    void Awake()
    {
        hour = 0;
        minute = 0;
        seconds = 0;
    }

    void LateUpdate()
    {
        if(GameInstructions.GAME_MODE == Literals.GAME_MODES.NORNAL_GAME_MODE)
        {
            UpdateCountUpTime();
        }
        else if(GameInstructions.GAME_MODE == Literals.GAME_MODES.SURVIVAL_GAME_MODE)
        {
            UpdateCountDownTime();
        }
        else if(GameInstructions.GAME_MODE == Literals.GAME_MODES.HEAT_GAME_MODE)
        {
            UpdateCountUpTime();
        }
    }

    void FixedUpdate()
    {
        timer.text = GetTime();
    }

    void UpdateCountUpTime()
    {
        seconds += Time.deltaTime * timeScale;
        BalanceCountUp();
    }

    void UpdateCountDownTime()
    {
        seconds -= (!GameInstructions.TIME_UP) ? Time.deltaTime * timeScale : 0;
        BalanceCountDown();      
    }

    void BalanceCountUp()
    {
        if(seconds >= 60f)
        {
            minute++;
            seconds = 0;
        }
        else if(minute >= 60)
        {
            hour++;
            minute = 0;
        }
    }

    void BalanceCountDown()
    {
        if(seconds <= 0)
        {
            if(minute != 0)
            {
                minute--;
                seconds = 59f;              
            }           
            else if(seconds <= 0)
            {
                GameInstructions.TIME_UP = true;
            }          
        }
        else if(minute <= 0)
        {
            if(hour != 0)
            {
                hour--;
                minute = 59;
            }          
        }
    }

    public int Hour
    {
        set { hour = value; }
        get { return hour; }
    }

    public int Minute
    {
        set { minute = value; }
        get { return minute; }
    }

    public float Seconds
    {
        set { seconds = value; }
        get { return seconds; }
    }

    private string Standadize(string value)
    {
        int actualValue = int.Parse(value);

        if(actualValue <= 9)
        {
            return "0" + value.ToString();
        }

        return value.ToString();
    }

    public string GetTime()
    {
        return Standadize(Minute.ToString()) + " : " + Standadize(((int)Seconds).ToString());
    }

    public float GetDecimal()
    {
        float numberOfSeconds = (Minute * 60) + Mathf.Round(Seconds);

        return numberOfSeconds;
    }

    public float GetDecimal(string time)
    {
        if(time == "00:00" || time == "0" || string.IsNullOrEmpty(time))
        {
            return 0;
        }
        else
        {
            int divider = time.IndexOf(':');
            int pminute = Int32.Parse(time.Substring(0, divider));
            float pseconds = Int32.Parse(time.Substring(divider + 1, (time.Length - 1) - divider));

            float numberOfSeconds = (pminute * 60) + Mathf.Round(pseconds);

            return numberOfSeconds;
        }       
    }

    public void UpdateBonusTime()
    {
        float numberOfSeconds = GetDecimal();

        int sMinute = 0;
        int sSecond = 0;

        for(int i = 0; i < numberOfSeconds; i++)
        {
            sSecond++;

            if(sSecond >= 60)
            {
                sMinute++;
                sSecond = 0;
            }
        }

        Minute = sMinute;
        Seconds = sSecond;
    }

    public string CompareTime(string timeA, string timeB)
    {
        float secondsA = GetDecimal(timeA);
        float secondsB = GetDecimal(timeB);

        if(secondsA >= secondsB)
        {
            return timeA;
        }
        else
        {
            return timeB;
        }
    }

    public bool BCompareTime(string currentTime, string otherTime)
    {
        float secondsA = GetDecimal(currentTime);
        float secondsB = GetDecimal(otherTime);

        if(secondsA > secondsB)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
