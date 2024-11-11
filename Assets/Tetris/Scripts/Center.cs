using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    private Tetris tet;

    private void Start()
    {
        tet = Camera.main.GetComponent<Tetris>();
    }

    private void Update()
    {
        
    }
}
