using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Station : MonoBehaviour, IInteractable
{
    public bool isCurrentlyInteractable { get; }

    public Vector3 position { get { return transform.position; } }
    public GameObject GetGameObject() { return gameObject; }

    public float outlineThickness = 1;
    protected MeshFilter[] outlines;
    protected Material outlineMat;
    void Awake()
    {

        BuildMeshList();
    }
    void OnEnable()
    {
        transform.DOPunchScale(-transform.localScale, 0.5f);
    }

    private void BuildMeshList()
    {
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
        outlines = new MeshFilter[meshes.Length];
        outlineMat = new Material(Resources.Load("Unlit_Outline") as Material);
        outlineMat.SetFloat("_Thick", outlineThickness);
        for (int i = 0; i < outlines.Length; i++)
        {
            GameObject go = new GameObject("outliner");
            go.transform.parent = meshes[i].transform.parent;
            go.transform.localPosition = meshes[i].transform.localPosition;
            go.transform.localRotation = meshes[i].transform.localRotation;
            go.transform.localScale = meshes[i].transform.localScale;
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            int matCount = meshes[i].GetComponent<MeshRenderer>().materials.Length;
            mr.materials = new[] { outlineMat, outlineMat };
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = meshes[i].mesh;
            outlines[i] = mf;
        }

        SetOutline(false, Color.black);
    }
    virtual public void Interact(PlayerController player)
    {
        
    }

    virtual public void EnterRange(PlayerController player)
    {
        
    }

    virtual public void LeaveRange(PlayerController player)
    {
        
    }


    private void SetOutline(bool set, Color color)
    {
        outlineMat.SetColor("_Color", color);
        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].gameObject.SetActive(set);
        }
    }
    public void EnterPrimarySelect(PlayerController player)
    {

        SetOutline(true, player.myColor);
    }

    public void LeavePrimarySelect(PlayerController player)
    {

        SetOutline(false, Color.black);
    }
}
