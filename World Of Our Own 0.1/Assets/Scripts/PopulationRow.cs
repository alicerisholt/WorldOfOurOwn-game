using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulationRow : MonoBehaviour
{
    public GameObject GUIManager;
    public Button personButton;
    public TextMeshProUGUI personName, age, gender, workplace, housing, happiness, health;

    public void SetPopulationRow(GameObject person, string personName, string age, string gender, string happiness, string health, string workplace = "None", string housing = "None")
    {
        GameObject GUImanager = GameObject.FindWithTag("GUIManager");
        GUIManager guiM = GUImanager.GetComponent<GUIManager>();
        personButton.onClick.AddListener(() => guiM.OpenClosePerson(person));

        this.personName.text = personName;
        this.age.text = age;
        this.gender.text = gender;
        this.happiness.text = happiness;
        this.health.text = health;

        if (workplace != "None")
        {
            this.workplace.text = workplace;
        } else
        {
            // DEACTIVATE BUTTON
            this.workplace.text = workplace;
        }

        if (housing != "None")
        {
            this.housing.text = housing;
        }
        else
        {
            // DEACTIVATE  BUTTON
            this.housing.text = housing;
        }
        /*            LeanTween.scale(populationPanel, new Vector3((float)1, (float)1, 0), (float)0.1);
            LeanTween.scale(populationPanel, new Vector3((float)1.01, (float)1.01, 0), (float)0.2).setDelay((float)0.01);
            LeanTween.scale(populationPanel, new Vector3((float)1, (float)1, 0), (float)0.3).setDelay((float)0.1);
         */
    }
}
