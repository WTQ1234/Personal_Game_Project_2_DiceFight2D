using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HRL;

public class WindManager : MonoSingleton<WindManager>
{
    public List<AreaEffector2D> effector2Ds;

    void Start()
    {
        effector2Ds = new List<AreaEffector2D>(GameObject.FindObjectsOfType<AreaEffector2D>());
        SetListActive(false);
    }

    public void SetListActive(bool is_active)
    {
        foreach(var effector in effector2Ds)
        {
            effector.enabled = is_active;
        }
    }
}
