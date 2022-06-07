using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMaterialSwitch : MonoBehaviour
{
    public MeshRenderer[] Renderers;
    public Collider[] Colliders;
    public Material AltMaterial;

    private Material[] defMaterials;
    private bool currentAltMat;

    private void SwitchMaterials (bool _toAlt)
    {
        if (_toAlt == currentAltMat)
            return;

        for (int z = 0; z < Renderers.Length; z++)
            if (Renderers[z])
            {
                if (_toAlt)
                    Renderers[z].material = AltMaterial;
                else
                    Renderers[z].material = defMaterials[z];
            }

        currentAltMat = _toAlt;
    }

    private bool CheckRaycast ()
    {
        if (!Camera.main)
            return false;

        Ray camRay = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
        Physics.Raycast(camRay, out RaycastHit hitInfo);

        for (int z = 0; z < Colliders.Length; z++)
            if (hitInfo.collider == Colliders[z])
                return true;

        return false;
    }

    void Start()
    {
        defMaterials = new Material[Renderers.Length];
        
        for (int z = 0; z < Renderers.Length; z++)
            defMaterials[z] = Renderers[z].material;

        currentAltMat = false;
    }

    void Update()
    {
        SwitchMaterials(CheckRaycast());
    }
}
