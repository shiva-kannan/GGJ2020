using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Property : MonoBehaviour
{
    [SerializeField]
    private GameObject explodeVFX;

    public float foodValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void Explode()
    {
        GameObject vfx = Instantiate(explodeVFX) as GameObject;
        vfx.transform.position = transform.position;
        Destroy(vfx, vfx.GetComponent<ParticleSystem>().main.duration);
    }

    
}
