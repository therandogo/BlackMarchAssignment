using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class WorldTile : MonoBehaviour
{
    private Transform tileTransform;
    private bool isBlocked;
    private bool isSelected;
    Material material;
    Color color;


    //setting the transform of the instanced tile at start
    private void Start()
    {
        tileTransform = transform;
        material = GetComponent<Renderer>().material;
        color = material.color;
    }


    //Mouse hover effects
    private void OnMouseEnter()
    {
        transform.DOScaleY(1.4f, 0.1f);
    }

    public void OnMouseExit()
    {
        transform.DOScaleY(1f, 0.1f);
    }


    //Mouse selection effects
    public void OnSelectionSuccess()
    {
        StartCoroutine(ChangeColorSuccess());
        isSelected = true;
    }

    public void OnSelectionFailure()
    {
        StartCoroutine(ChangeColorFailure());
        isSelected = true;
    }

    private IEnumerator ChangeColorSuccess()
    {
        yield return material.DOBlendableColor(Color.green, 0.5f).WaitForCompletion();
        material.DOBlendableColor(color, 0.5f);
        isSelected = false;
    }

    private IEnumerator ChangeColorFailure()
    {
        yield return material.DOBlendableColor(Color.red, 0.5f).WaitForCompletion();
        material.DOBlendableColor(color, 0.5f);
        isSelected = false;
    }


    //Getters and Setters
    public Transform getTileTransform() { return tileTransform; }

    public bool getTileBlockedStatus() { return isBlocked; }

    public bool getSelected() { return isSelected; }

    public void setTileBlockedStatus(bool status) { isBlocked = status; }
}
