using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void ShapeSelected(int shape)
    {
        GameManager.instance.ObjectSelected((Shape)shape);
    }

}

