﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereShoot : CodeBlock
{
    public override string GetName()
    {
        return "SphereShot()";
    }

    public override void OnShot(params object[] args)
    {
        if (args.Length > 3 && (bool)args[2] == false)
        {
            return;
        }

        var proj = (GameObject)args[0];
        var proj2 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 45));
        var proj3 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 90));
        var proj4 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 135));
        var proj5 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, 180));
        var proj6 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -45));
        var proj7 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -90));
        var proj8 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -135));
        var proj9 = Object.Instantiate(proj, proj.transform.position, proj.transform.rotation * Quaternion.Euler(0, 0, -180));

        PlayerController.OnShoot.Invoke(proj2, false, false);
        PlayerController.OnShoot.Invoke(proj3, false, false);
        PlayerController.OnShoot.Invoke(proj4, false, false);
        PlayerController.OnShoot.Invoke(proj5, false, false);
        PlayerController.OnShoot.Invoke(proj6, false, false);
        PlayerController.OnShoot.Invoke(proj7, false, false);
        PlayerController.OnShoot.Invoke(proj8, false, false);
        PlayerController.OnShoot.Invoke(proj9, false, false);
    }

    public override OnX GetOnX()
    {
        return OnX.OnShoot;
    }

    public override int SpawnChance()
    {
        return 5;
    }
}