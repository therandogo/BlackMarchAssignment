using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshPro txt;

    public void setText(string str) { txt.SetText(str); }

    public Transform getTextTransform() { return txt.transform; }

    public void setTextPosition(Vector3 v3) { txt.transform.position = v3; }
}
