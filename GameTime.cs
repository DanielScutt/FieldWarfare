using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public Transform sun;
    public AudioSource dayTimeSound;
    public AudioSource nightTimeSound;
    float dayCycleInSeconds = 4f;

    private const float SECOND = 1;
    private const float MINUTE = 60 * SECOND;
    private const float HOUR = 60 * MINUTE;
    private const float DAY = 24 * HOUR;

    private const float DEGREES_PER_SECOND = 360;

    private float _degreeRotation;

    private float _timeOfDay;

    private int _numberOfDays;

    bool turnOff = false;
    bool daytime;
    bool nighttime;

    Quaternion startingPosition;
    float endPosition;

    public CropManager cropManager;

    // Start is called before the first frame update
    void Start()
    {
        _numberOfDays = 1;
        _timeOfDay = 0;

        _degreeRotation = DEGREES_PER_SECOND / (dayCycleInSeconds * MINUTE);

        startingPosition = sun.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (sun.eulerAngles.x > -20 && sun.eulerAngles.x < 200)
        {
            daytime = true;
            nighttime = false;
        }
        else
        {
            daytime = false;
            nighttime = true;
        }

        if(daytime)
        {
            sun.Rotate(new Vector3(_degreeRotation, 0, 0) * Time.deltaTime, Space.World);
        }
        
        if(nighttime)
        {
            cropManager.NightTime();
            sun.localEulerAngles = new Vector3(-90, 0, 0);
            if(!nightTimeSound.isPlaying)
            {
                nightTimeSound.Play();
                dayTimeSound.Stop();
            }

        }

        Debug.Log(nighttime);
        _timeOfDay += Time.deltaTime;

    }

    public void NextDay()
    {
        nightTimeSound.Stop();
        dayTimeSound.Play();
        _numberOfDays++;
        cropManager.NextDay();
        sun.localEulerAngles = new Vector3(0, 0, 0);
    }

    public bool IsNightTime()
    {
        return nighttime;
    }

    public int GetCurrentDay()
    {
        return _numberOfDays;
    }

    public void SetDaytime()
    {
        sun.localEulerAngles = new Vector3(-10, 0, 0);
    }
}
