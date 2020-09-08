using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimelineManager : MonoBehaviour
{
    System.Random random = new System.Random();

    public TextMeshProUGUI yearMonthText;
    public int currentYear, currentMonth, currentDay;
    private string[] months;

    public float currentSpeed;
    private float pause = 0;
    private float speed1 = 1;
    private float speed2 = 2;
    private float speed3 = 3;
    private bool automaticProgression = true; // SÄTT TILL FALSE SEN

    public GameObject populationManager;
    private PopulationManager pm;


    private void Start()
    {
        Time.fixedDeltaTime = 1;
        currentYear = 2080;
        currentMonth = 1;
        currentDay = 1;
        months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        pm = populationManager.GetComponent<PopulationManager>();
    }

    // Updates once every day
    void FixedUpdate()
    {
        PassOneDay();
        if (automaticProgression)
        {
            KillPeople();
            pm.MakeRandomCouple();
            pm.MakeRandomBaby();
        }  
    }



    private void KillPeople()
    {
        int rand = 0;
        List<GameObject> personList = pm.personList;
        for (int i = 0; i < personList.Count; i++)
        {
            rand = random.Next(1, 10000);
            GameObject person = pm.personList[i];
            int personAge = person.GetComponent<Person>().age;

            if (personAge < 7)
            {
                if (rand <= 1) // 0.001 %
                {
                    pm.KillPerson(person);
                    i--;
                }
            }
            else if (personAge < 16)
            {
                if (rand <= 2)
                {
                    pm.KillPerson(person);
                    i--;
                }
            }
            else if (personAge < 30)
            {
                if (rand <= 3)
                {
                    pm.KillPerson(person);
                    i--;
                }
                else
                {
                }
            }
            else if (personAge < 65)
            {
                if (rand <= 100)
                {
                    pm.KillPerson(person);
                    i--;
                } else
                {
                }
            }
            else
            {
                if (rand <= 500)
                {
                    pm.KillPerson(person);
                    i--;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSpeed = speed1;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSpeed = speed2;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSpeed = speed3;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.01f;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSpeed = pause;
            Time.timeScale = 0;
        }
    }

    private void PassOneDay()
    {
        if ((currentMonth == 1 || currentMonth == 3 || currentMonth == 5 || currentMonth == 7 || currentMonth == 8 || currentMonth == 10 || currentMonth == 12) && currentDay == 31 ||
    (currentMonth == 2) && currentDay == 21 ||
    (currentMonth == 4 || currentMonth == 6 || currentMonth == 9 || currentMonth == 11) && currentDay == 21)
        {
            currentDay = 1;
            currentMonth++;
        }
        else
        {
            currentDay++;
        }
        if (currentMonth == 13)
        {
            currentMonth = 1;
            currentYear++;
            pm.AgePopulation();
        }

        yearMonthText.text = "Year " + currentYear + ", " + months[currentMonth - 1] + " " + currentDay;
    }



}
