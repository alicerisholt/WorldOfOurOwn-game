using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDateTime
{
    public int day = 0;
    public int month = 0;
    public int year = 0;

    private int daysSince = 0;
    private int age = 0;

    public int getDaysSince()
    {
        return daysSince;
    }

    public int getAge()
    {

        return age;
    }

}
