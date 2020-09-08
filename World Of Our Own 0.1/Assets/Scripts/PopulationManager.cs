using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    private List<string> femaleNames;
    private List<string> maleNames;
    public GameObject personPrefab;
    public List<GameObject> personList;
    public List<GameObject> singleFemales;
    public List<GameObject> singleMales;
    public GameObject timelineManager;
    private TimelineManager tm;

    System.Random random = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
        tm = timelineManager.GetComponent<TimelineManager>();
        FillFemaleNamesLists();
        FillMaleNamesLists();
        GenerateBaseCrew();
    }

    private void FixedUpdate()
    {
        
    }


    public string GetPersonList()
    {
        string personString = "";
        foreach (GameObject person in personList)
        {
            personString += "[ " + person.GetComponent<Person>().personName + ", " + person.GetComponent<Person>().age + "]";
        }
        if (personString == "") { personString = "No person"; }
        return personString;
    }


    private void GenerateBaseCrew()
    {
        InstantiateNewPerson("Grace", 24, "Female", 60f, 82f);
        InstantiateNewPerson("Nelson", 25, "Male", 60f, 82f);
        InstantiateNewPerson("Kit", 27, "Male", 60f, 82f);
        InstantiateNewPerson("Kurt", 27, "Male", 60f, 82f);
        InstantiateNewPerson("Hugo", 25, "Male", 60f, 82f);
        InstantiateNewPerson("Steven", 35, "Male", 60f, 82f);
        InstantiateNewPerson("Emilio", 40, "Male", 60f, 82f); 
        InstantiateNewPerson("Cleo", 41, "Female", 60f, 82f);
        InstantiateNewPerson("Millie", 25, "Female", 60f, 82f);
        InstantiateNewPerson("Elsa", 22, "Female", 60f, 82f);
        InstantiateNewPerson("Victoria", 28, "Female", 60f, 82f);
        InstantiateNewPerson("Sally", 33, "Female", 60f, 82f);
        foreach (GameObject person in personList)
        {
            AddToSinglesList(person);
        }
    }


    private void InstantiateNewPerson(string name, int age, string gender, float happiness, float health, GameObject mother=null, GameObject father=null)
    {
        Vector3 personPosition = new Vector3(0, 0, 0);
        GameObject person = Instantiate(personPrefab, personPosition, Quaternion.identity) as GameObject;
        person.GetComponent<Person>().CreatePerson(name, age, gender, happiness, health, mother, father);
        personList.Add(person);
        if (mother != null)
        {
            mother.GetComponent<Person>().children.Add(person);
        }
        if (father != null)
        {
            father.GetComponent<Person>().children.Add(person);
        }
    }


    public void AgePopulation()
    {
        foreach (GameObject person in personList)
        {
            person.GetComponent<Person>().age++;
            // TODO
            // if person age is greater than 18 add them to singles list
        }
    }


    private void AddToSinglesList(GameObject person)
    {
        Person p = person.GetComponent<Person>();
        if (p.gender == "Female")
        {
            singleFemales.Add(person);
        }
        else
        {
            singleMales.Add(person);
        }

    }


    public void MakeRandomCouple()
    {
        int type = random.Next(1, 21);
        if (singleFemales.Count > 0 && singleMales.Count > 0)
        {
            int randomMale1 = random.Next(0, singleMales.Count );
            int randomFemale1 = random.Next(0, singleFemales.Count );

            if (type < 19 && singleFemales.Count > 0 && singleMales.Count > 0)
            {
                GameObject female = singleFemales[randomFemale1];
                GameObject male = singleMales[randomMale1];
                singleFemales.Remove(female);
                singleMales.Remove(male);

                Person f = female.GetComponent<Person>();
                Person m = male.GetComponent<Person>();

                f.spouse = male;
                m.spouse = female;
                Debug.Log( type + " New couple: " + f.personName + " and " + m.personName);

            }
            else if (type == 19 && singleFemales.Count > 1)
            {
                int randomFemale2 = random.Next(0, singleFemales.Count );
                GameObject female1 = singleFemales[randomFemale1];
                GameObject female2 = singleFemales[randomFemale2];
                singleFemales.Remove(female1);
                singleFemales.Remove(female2);

                Person f1 = female1.GetComponent<Person>();
                Person f2 = female2.GetComponent<Person>();

                f1.spouse = female2;
                f2.spouse = female1;
                Debug.Log("19 New couple: " + f1.personName + " and " + f2.personName);
            }
            else if (type == 20 && singleMales.Count > 1)
            {
                int randomMale2 = random.Next(0, singleMales.Count );
                GameObject male1 = singleMales[randomMale1];
                GameObject male2 = singleMales[randomMale2];
                singleMales.Remove(male1);
                singleMales.Remove(male2);

                Person m1 = male1.GetComponent<Person>();
                Person m2 = male2.GetComponent<Person>();

                m1.spouse = male2;
                m2.spouse = male1;
                Debug.Log("20 New couple: " + m1.personName + " and " + m2.personName);
            }
            else
            {
                Debug.Log("Didn't make couple");
            }
        }
    }

    
    public void MakeRandomBaby()
    {
        for (int i = 0; i < personList.Count; i++)
        {
            Person person = personList[i].GetComponent<Person>();
            if (person.age > 19 && person.gender == "Female" && !singleFemales.Contains(personList[i]) && person.pregnant == null && person.spouse.GetComponent<Person>().gender == "Male")
            {
                WDateTime pregnantDate = new WDateTime();
                pregnantDate.year = tm.currentYear;
                pregnantDate.month = tm.currentMonth;
                pregnantDate.day = tm.currentDay;

                Debug.Log(person.personName + " got pregnant on " + pregnantDate.year + "/" + pregnantDate.month + "/" + pregnantDate.day);
                person.pregnant = pregnantDate;
                GiveBirth(personList[i]);
                break;
            }
        }
    }


    public void GiveBirth(GameObject mother)
    {
        Person motherPerson = mother.GetComponent<Person>();
        Person fatherPerson = motherPerson.spouse.GetComponent<Person>();
        GameObject father = motherPerson.spouse;
    
        motherPerson.pregnant = null;

        String babyGender = "Female";
        String babyName = "Hej";

        int rand = random.Next(0, 2);
        if (rand == 0) {
            babyGender = "Male";
            if(maleNames.Count < 1) { FillMaleNamesLists(); }
            rand = random.Next(0, maleNames.Count);
            babyName = maleNames[rand];
            maleNames.Remove(maleNames[rand]);
        } else
        {
            if (femaleNames.Count < 1) { FillFemaleNamesLists(); }
            rand = random.Next(0, femaleNames.Count);
            babyName = femaleNames[rand];
            femaleNames.Remove(femaleNames[rand]);
        }

        InstantiateNewPerson(babyName, 0, babyGender, 60f, 60f, mother, father);

        Debug.Log(motherPerson.personName + " and " + fatherPerson.personName + " gave birth to " + babyName );
    }


    public void KillPerson(GameObject person)
    {
        Debug.Log("Kills person: " + person.GetComponent<Person>().personName + " at Y:" + timelineManager.GetComponent<TimelineManager>().currentYear + ", M:" + timelineManager.GetComponent<TimelineManager>().currentMonth + ", D:" + timelineManager.GetComponent<TimelineManager>().currentDay);

        if (singleFemales.Contains(person)){
            singleFemales.Remove(person);
        } else if (singleMales.Contains(person)){
            singleMales.Remove(person);
        }

        Destroy(person);
        personList.Remove(person);
    }

    private void FillFemaleNamesLists()
    {
        femaleNames = new List<string>(new string[100] { "Amelia", "Alice", "Anna", "Ari", "Amy", "Audrey", "Aria", "Adelyn", "Amara", "Alina", "Agnes" ,"Aina", "Adina", "Alicia", "Alva", "Allie", "Amal", "Ally", "Amie", "Amine",
                                        "Ana", "Andrea", "Anita", "Anja", "Asta", "Astrid", "Athena", "Ava", "Ayla", "Avin",
                                        "Bella", "Belle", "Betty", "Billie", "Bodil", "Bonnie", "Britta", "Bria", "Brie", "Becky",
                                        "Cecilia", "Charlie", "Camila", "Cora", "Callie", "Cindy", "Celine", "Clara", "Carmen", "Celia",
                                        "Daisy", "Dana", "Dalia", "Daniela", "Diana", "Dima", "Doris", "Demi", "Danielle", "Danika",
                                        "Emma", "Ella", "Emilia", "Elise", "Eve", "Elle", "Emelia", "Elena", "Esther", "Edith", "Eloise", "Elsy", "Elsa", "Emmy", "Elin", "Elvira", "Estrid", "Evin", "Erica", "Enya",
                                        "Fatima", "Felice", "Felicia", "Fiona", "Flora", "Freya", "Frida", "Farah", "Frankie", "Felicia",
                                        "Gabriela", "Gry", "Gloria", "Gina", "Gemma", "Greta", "Gracie", "Gianna", "Gwen", "Gisele"});
    }

    private void FillMaleNamesLists()
    {
        maleNames = new List<string>(new string[100] {   "Aaron", "Abed", "Abe", "Adam", "Adrian", "August", "Axel", "Alexander", "Alfred", "Arthur", "Atlas" ,"Arvid", "Arnold", "Anton", "Antonio", "Albus", "Atticus", "Allen", "Archer", "Angelo",
                                        "Ben", "Benjamin", "Bill", "Billy", "Bowie", "Brian", "Bruno", "Bennett", "Braden", "Boston",
                                        "Christopher", "Connor", "Caleb", "Carlos", "Ceasar", "Collin", "Charles", "Calvin", "Caspar", "Conrad",
                                        "Dan", "Daniel", "Danny", "Dante", "Denis", "Dilan", "Dominic", "Dylan", "Douglas", "Damon",
                                        "Elijah", "Easton", "Ezekiel", "Eric", "Eduardo", "Edgar", "Ethan", "Elias", "Everett", "Elliot", "Emil", "Edwin", "Emanuel", "Edison", "Elton", "Einar", "Elias", "Elvin", "Eskil", "Eugen",
                                        "Felix", "Ferdinand", "Finn", "Frans", "Fred", "Freddy", "Fredric", "Ford", "Franklin", "Forrest",
                                        "Gunnar", "George", "Grant", "Grey", "Greyson", "Giovanni", "Gary", "Gavin", "Gianni", "Gordon", "Gibson", "Gilbert", "Glenn", "Gustav", "Gregory", "Gabriel", "Gibson", "Garret", "Gus", "Gunder"});
    }
}
