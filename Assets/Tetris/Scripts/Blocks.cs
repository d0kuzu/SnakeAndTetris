using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    private Tetris tetris;

    void Start()
    {
        tetris = Camera.main.GetComponent<Tetris>();
    }

    void Update()
    {
        if (tetris.game && name == "falling")
        {
            CheckInWall();
            CheckAround();
            CheckGround();
        }
    }

    private void CheckInWall()
    {
        Collider[] colls = Physics.OverlapBox(transform.position, new Vector3(0.125f, 0.125f, 0.125f));
        float x = 0;
        foreach (Collider coll in colls)
        {
            if (coll.name != "falling")
            {
                GameObject parent = transform.parent.gameObject;
                if (parent.transform.position.x < coll.transform.position.x)
                {
                    parent.transform.position = new Vector3(parent.transform.position.x - 0.5f, parent.transform.position.y, parent.transform.position.z);
                    x += 0.5f;
                    break;
                }
                else if (parent.transform.position.x > coll.transform.position.x)
                {
                    parent.transform.position = new Vector3(parent.transform.position.x + 0.5f, parent.transform.position.y, parent.transform.position.z);
                    x -= 0.5f;
                    break;
                }
            }
        }

        colls = Physics.OverlapBox(transform.position, new Vector3(0.125f, 0.125f, 0.125f));
        foreach (Collider coll in colls)
        {
            if (coll.name != "falling")
            {
                GameObject parent = transform.parent.gameObject;
                parent.transform.position = new Vector3(parent.transform.position.x + x, parent.transform.position.y, parent.transform.position.z);

                tetris.rotation += 90;
                parent.transform.rotation = Quaternion.Euler(0, 0, tetris.rotation);
            }
        }
    }

    public void Deactivate()
    {
        name = "block";
        float x = 0.25f;
        while (x <= 7.75f)
        {
            if ((x > transform.position.y && x - transform.position.y < 0.25f) || (x < transform.position.y && transform.position.y - x < 0.25f))
            {
                transform.position = new Vector3(transform.position.x, x, transform.position.z);
                break;
            }
            x += 0.5f;
        }
    }

    private bool CheckGround()
    {
        Collider[] colls = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), 0.01f);

        foreach (Collider coll in colls)
        {
            if (coll.name != "falling" && coll.tag != "Wall")
            {
                GetComponentInParent<Figure>().Deactivate();
                return false;
            }
        }
        return true;
    }

    private void CheckAround()
    {
        if(name == "falling")
        {
            if (!tetris.lockLeft)
            {
                Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z), new Vector3(0.01f, 0.25f, 0.25f));
                foreach (Collider coll in colls)
                {
                    if (coll.name != "falling" && coll.name != "Top")
                    {
                        tetris.lockLeft = true;
                        break;
                    }
                }
            }
            if (!tetris.lockRight)
            {
                Collider[] colls = Physics.OverlapBox(new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z), new Vector3(0.01f, 0.25f, 0.25f));
                foreach (Collider coll in colls)
                {
                    if (coll.name != "falling" && coll.name != "Top")
                    {
                        tetris.lockRight = true;
                        break;
                    }
                }
            }
        }
    }
}
