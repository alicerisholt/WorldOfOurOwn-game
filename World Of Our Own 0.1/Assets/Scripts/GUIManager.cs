using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public GameObject populationManager;

    public Button buildModeButton, populationButton; //, researchButton, politicsButton  milestonesButton;
    public Button buildModeCloseButton, personCloseButton; //, researchButton, politicsButton,  milestonesButton;
    public GameObject buildModePanel, personPanel, populationPanel ; //, researchPanel, politicsPanel, , milestonesPanel;
    public GameObject populationRowPrefab;

    // PERSON PANEL
    public TextMeshProUGUI personHeader, personSubheader;
    public GameObject healthBar, happinessBar;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateButtons();
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    private void InstantiateButtons()
    {
        buildModeButton.onClick.AddListener(() => OpenCloseBuildMode());
        buildModeCloseButton.onClick.AddListener(() => OpenCloseBuildMode());
        //researchButton.onClick.AddListener(() => OpenCloseResearch());
        //politicsButton.onClick.AddListener(() => OpenClosePolitics());
        //milestonesButton.onClick.AddListener(() => OpenCloseMilestones());
        //economyButton.onClick.AddListener(() => OpenCloseEconomy());

        //populationButton.onClick.AddListener(() => OpenClosePerson(populationManager.GetComponent<PopulationManager>().personList[0]));
        populationButton.onClick.AddListener(() => OpenClosePopulation());

    }

    private void OpenCloseBuildMode()
    {
        // sätt grid till färg
        if (buildModePanel.activeSelf)
        {
            buildModePanel.SetActive(false);
        }
        else
        {
            DeactivateAll();
            buildModePanel.SetActive(true);
        } 
    }


    private void OpenClosePopulation()
    {
        if (populationPanel.activeSelf)
        {
            populationPanel.SetActive(false);
        }
        else
        {
            DeactivateAll();
            PopulationManager pm = populationManager.GetComponent<PopulationManager>();
            int offset = 0;
            foreach(GameObject person in pm.personList)
            {
                InstantiatePopulationRow(person, 200-(offset*24));
                offset++;
            }
            populationPanel.SetActive(true);
        }
    }


    private void OpenCloseResearch()
    {
        throw new NotImplementedException();
    }


    private void OpenClosePolitics()
    {
        throw new NotImplementedException();
    }


    public void OpenClosePerson(GameObject person)
    {
        Person personP = person.GetComponent<Person>();
        personHeader.text = personP.personName;
        personSubheader.text = personP.gender + ", " + personP.age.ToString() + " years old";

        RectTransform healthBarRT = healthBar.GetComponent<RectTransform>();
        healthBarRT.sizeDelta = new Vector2((float)0.01 * 260 * personP.health, healthBarRT.sizeDelta.y);
        healthBarRT.LeanSetLocalPosX((float)((0.01 * 260 * personP.health) - 260) / 2);

        RectTransform happinessBarRT = happinessBar.GetComponent<RectTransform>();
        happinessBarRT.sizeDelta = new Vector2((float)0.01 * 260 * personP.happiness, happinessBarRT.sizeDelta.y);
        happinessBarRT.LeanSetLocalPosX((float)((0.01 * 260 * personP.happiness) - 260) / 2);

        DeactivateAllPersons();
        personPanel.SetActive(true);
        LeanTweenSmallBounce(personPanel);
    }


    private void OpenCloseMilestones()
    {
        throw new NotImplementedException();
    }


    private void OpenCloseEconomy()
    {
        throw new NotImplementedException();
    }


    private void DeactivateAll()
    {
        buildModePanel.SetActive(false);
        populationPanel.SetActive(false);
        personPanel.SetActive(false);
    }

    private void DeactivateAllPersons()
    {
        personPanel.SetActive(false);
    }


    private void InstantiatePopulationRow(GameObject person, int yPos)
    {
        GameObject row = Instantiate(populationRowPrefab, populationPanel.GetComponent<Transform>());
        PopulationRow rowScript = row.GetComponent<PopulationRow>();
        Person personScript = person.GetComponent<Person>();
        rowScript.SetPopulationRow(person,
                                   personScript.personName, 
                                   personScript.age.ToString(), 
                                   personScript.gender, 
                                   personScript.happiness.ToString() + "%", 
                                   personScript.health.ToString() + "%");
        row.transform.localPosition = new Vector3(0, yPos, 0);
    }

    public void LeanTweenSmallBounce(GameObject gameObject)
    {
        LeanTween.scale(gameObject, new Vector3((float)1, (float)1, 0), (float)0.1);
        LeanTween.scale(gameObject, new Vector3((float)1.01, (float)1.01, 0), (float)0.1).setDelay((float)0.1);
        LeanTween.scale(gameObject, new Vector3((float)1, (float)1, 0), (float)0.2).setDelay((float)0.3);
    }
}
