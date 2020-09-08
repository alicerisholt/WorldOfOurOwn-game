using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    // BUILDINGS AND BUTTONS
    public Button restaurantButton, basecampButton;
    public GameObject restaurantBuilding, basecampBuilding;

    // BUILD MODE VARIABLES
    private GameObject currentBuilding;
    private int rotation = 0;
    Vector3 tempPosition = new Vector3();
    int tempIndex = 0;

    // GENERATE GRID
    [Range(0, 1)]
    public float outlinePercent;
    public Transform tilePrefab;
    public Vector2 mapSize;
    private GameObject[] tileList;

    public Material tile;
    public Material tileSuccess;
    public Material tileError;


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        SetTileList();
        InstantiateButtons();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentBuilding != null)
        {
            MoveCurrentBuilding();
            RotateCurrentBuilding();
            PlaceIfClicked();
            DestroyIfRightClicked();
        }
    }


    ///////////////// BUTTONS AND INTERFACE ////////////////

    private void InstantiateButtons()
    {
        restaurantButton.onClick.AddListener(() => InstantiateBuilding(1));
        basecampButton.onClick.AddListener(() => InstantiateBuilding(2));
    }


    ///////////////////// MAP AND GRID /////////////////////
    
    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + x, 0, -mapSize.y / 2 + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }
    }


    private void SetTileList()
    {
        tileList = GameObject.FindGameObjectsWithTag("Tile");
    }


    private void UpdateTileColorsForBuildingSize(int i, string buildingSize)
    {
        Material currentColor = tile;
        if (IsLegalPlacement(i, buildingSize)){
            currentColor = tileSuccess;
        } else
        {
            currentColor = tileError;
        }

        tileList[i].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i - 1].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i + 1].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i + 99].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i + 100].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i + 101].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i - 99].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i - 100].GetComponent<MeshRenderer>().material = currentColor;
        tileList[i - 101].GetComponent<MeshRenderer>().material = currentColor;

        switch (buildingSize)
        {
            case "3x3":
                // No need to color more
                break;
            case "3x5":
                if (rotation % 2 == 0) // Even
                {
                    tileList[i - 199].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i - 200].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i - 201].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 199].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 200].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 201].GetComponent<MeshRenderer>().material = currentColor;

                } else
                {
                    tileList[i - 2].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 2].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 98].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i + 102].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i - 98].GetComponent<MeshRenderer>().material = currentColor;
                    tileList[i - 102].GetComponent<MeshRenderer>().material = currentColor;
                }    
                break;
        }
    }


    ///////////////////// BUILDING INSTANTIATION AND PLACEMENT /////////////////////

    private void InstantiateBuilding(int buildingType)
    {
        switch (buildingType)
        {
            case 1:
                if (currentBuilding == null) { currentBuilding = Instantiate(restaurantBuilding); }
                else { Destroy(currentBuilding); }
                break;
            case 2:
                if (currentBuilding == null) { currentBuilding = Instantiate(basecampBuilding); }
                else {  Destroy(currentBuilding); }
                break;
        }
    }

    private void MoveCurrentBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {

            // Settng the tile colors
            for (int j = 0; j < tileList.Length; j++) { tileList[j].GetComponent<MeshRenderer>().material = tile; }
            for (int i = 0; i < tileList.Length ; i++)
            {
                tempPosition = new Vector3(Mathf.Round(hitInfo.point.x), Mathf.Round(hitInfo.point.y), Mathf.Round(hitInfo.point.z));
                if (tempPosition == tileList[i].transform.position)
                {
                    UpdateTileColorsForBuildingSize(i, currentBuilding.tag);
                }
            }
            currentBuilding.transform.position = new Vector3(Mathf.Round(hitInfo.point.x), Mathf.Round(hitInfo.point.y), Mathf.Round(hitInfo.point.z)); 
            currentBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            currentBuilding.transform.Rotate(0, 90*rotation, 0);



        }
    }


    private bool IsLegalPlacement(int i, string buildingSize)
    {
        switch (buildingSize)
        {
            case "3x3":
                if (tileList[i].GetComponent<Tile>().occupied || tileList[i + 1].GetComponent<Tile>().occupied || tileList[i - 1].GetComponent<Tile>().occupied ||
                    tileList[i + 100].GetComponent<Tile>().occupied || tileList[i + 99].GetComponent<Tile>().occupied || tileList[i + 101].GetComponent<Tile>().occupied ||
                    tileList[i - 99].GetComponent<Tile>().occupied || tileList[i - 100].GetComponent<Tile>().occupied || tileList[i - 101].GetComponent<Tile>().occupied)
                {
                    return false;
                }
                break;

            case "3x5":
                if (rotation % 2 == 0) // Even
                {
                    if (tileList[i].GetComponent<Tile>().occupied || tileList[i + 1].GetComponent<Tile>().occupied || tileList[i - 1].GetComponent<Tile>().occupied ||
                        tileList[i + 100].GetComponent<Tile>().occupied || tileList[i + 99].GetComponent<Tile>().occupied || tileList[i + 101].GetComponent<Tile>().occupied ||
                        tileList[i - 99].GetComponent<Tile>().occupied || tileList[i - 100].GetComponent<Tile>().occupied || tileList[i - 101].GetComponent<Tile>().occupied ||
                        tileList[i + 199].GetComponent<Tile>().occupied || tileList[i + 200].GetComponent<Tile>().occupied || tileList[i + 201].GetComponent<Tile>().occupied ||
                        tileList[i - 199].GetComponent<Tile>().occupied || tileList[i - 200].GetComponent<Tile>().occupied || tileList[i - 201].GetComponent<Tile>().occupied )
                    {
                        return false;
                    }
                }
                else
                {
                    if (tileList[i].GetComponent<Tile>().occupied || tileList[i + 1].GetComponent<Tile>().occupied || tileList[i - 1].GetComponent<Tile>().occupied ||
                        tileList[i + 100].GetComponent<Tile>().occupied || tileList[i + 99].GetComponent<Tile>().occupied || tileList[i + 101].GetComponent<Tile>().occupied ||
                        tileList[i - 99].GetComponent<Tile>().occupied || tileList[i - 100].GetComponent<Tile>().occupied || tileList[i - 101].GetComponent<Tile>().occupied ||
                        tileList[i + 2].GetComponent<Tile>().occupied || tileList[i + 98].GetComponent<Tile>().occupied || tileList[i + 102].GetComponent<Tile>().occupied ||
                        tileList[i - 2 ].GetComponent<Tile>().occupied || tileList[i - 98].GetComponent<Tile>().occupied || tileList[i - 102].GetComponent<Tile>().occupied)
                    {
                        return false;
                    }
                }
                break;  
        }
       return true;
    }


    private void RotateCurrentBuilding()
    {
         if (Input.GetKeyDown(KeyCode.R))
         {
            Debug.Log("rotated");
             rotation += 1;
         }
    }


    private void PlaceIfClicked()
    {
        if (IsLegalPlacement(GetTileIndex(), currentBuilding.tag) && Input.GetMouseButtonDown(0))
        {
            SetTilesToOccupied();
            currentBuilding = null;
            rotation = 0;
            for (int j = 0; j < tileList.Length; j++) { tileList[j].GetComponent<MeshRenderer>().material = tile; }

        }
    }


    private void SetTilesToOccupied()
    {

        tileList[GetTileIndex()].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() + 1].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() - 1].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() + 99].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() + 100].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() + 101].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() - 99].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() - 100].GetComponent<Tile>().occupied = true;
        tileList[GetTileIndex() - 101].GetComponent<Tile>().occupied = true;

        switch (currentBuilding.tag)
        {
            case "3x3":
                // Only need ones above
                break;

            case "3x5":
                if (rotation % 2 == 0) // Even
                {
                    tileList[GetTileIndex() - 199].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() - 200].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() - 201].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 199].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 200].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 201].GetComponent<Tile>().occupied = true;
                }
                else
                {
                    tileList[GetTileIndex() - 2].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 2].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 98].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() + 102].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() - 98].GetComponent<Tile>().occupied = true;
                    tileList[GetTileIndex() - 102].GetComponent<Tile>().occupied = true;
                }
                break;
        }
    }


    private void DestroyIfRightClicked()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentBuilding);
            currentBuilding = null;
            rotation = 0;
            for (int j = 0; j < tileList.Length; j++) { tileList[j].GetComponent<MeshRenderer>().material = tile; }
        }
    }


    private int GetTileIndex()
    {
        tempIndex = 0;
        for (int i = 0; i < tileList.Length; i++)
        {
            if (currentBuilding.transform.position == tileList[i].transform.position)
            {
                tempIndex = i;
            }
        }
        return tempIndex;
    }

}

  
                