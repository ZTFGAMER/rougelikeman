using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.instance.survivors.Count > 0)
        {
            Vector3 vector = new Vector3();
            foreach (SurvivorHuman human in GameManager.instance.survivors)
            {
                if (human != null)
                {
                    vector += human.transform.position;
                }
            }
            base.transform.position = vector / ((float) GameManager.instance.survivors.Count);
        }
    }
}

