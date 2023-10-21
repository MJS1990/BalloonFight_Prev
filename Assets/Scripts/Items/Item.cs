using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Item : MonoBehaviour
{
    [SerializeField]
    public List<Image> remainImages;

    int maxCount;
    [HideInInspector]
    public int currentCount;

    [SerializeField]
    Image PanelImage;

    void Start()
    {
        maxCount = remainImages.Count;
        currentCount = maxCount;
    }

    void FixedUpdate()
    {
        
    }

    public void Buy()
    {
        remainImages[maxCount - currentCount].color = new Color(1, 0, 0, 1);
        currentCount -= 1;
    }

    public void ApplyAmount(int count)
    {
        for (int i = 0; i < (maxCount - count); i++)
            Buy();
    }
}
