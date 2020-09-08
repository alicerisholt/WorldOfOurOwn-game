using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public string personName;
    public int age;
    public float happiness;
    public float health;
    public int personNumber;
    public string gender; // "Female" or "Male"
    public WDateTime pregnant = null;

    public GameObject mother;
    public GameObject father;
    public GameObject spouse;
    public List<GameObject> children;

    public void CreatePerson(string personName, int age, string gender, float happiness, float health, GameObject mother=null, GameObject father=null)
    {
        this.personName = personName;
        this.age = age;
        this.gender = gender;
        this.happiness = happiness;
        this.health = health;
        this.mother = mother;
        this.father = father;
    }
}


