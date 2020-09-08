using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextButton : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(this.gameObject, new Vector3((float)1, (float)1, 0), (float)0.1);
        LeanTween.scale(this.gameObject, new Vector3((float)1.01, (float)1.01, 0), (float)0.1).setDelay((float)0.1);
        LeanTween.scale(this.gameObject, new Vector3((float)1, (float)1, 0), (float)0.2).setDelay((float)0.3);
    }


}
