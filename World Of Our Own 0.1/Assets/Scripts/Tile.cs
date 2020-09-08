using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material hover;
    public Material normal;
    public MeshRenderer meshRenderer;
    public bool occupied = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normal;
    }
}
