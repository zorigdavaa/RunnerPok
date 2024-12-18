using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlusOne : MonoBehaviour
{
    [SerializeField] TMP_Text tmpText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Set(string text, Color color)
    {
        tmpText.text = text;
        tmpText.color = color;
    }
    public void Set(string text, Color color, Vector2 size)
    {
        tmpText.text = text;
        tmpText.color = color;
        tmpText.GetComponent<RectTransform>().sizeDelta = size;
    }
}
