using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tetris : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject[] figures;
    [SerializeField] public Material[] materials;
    public GameObject curFigure = null;

    [SerializeField] private float speed = 1f;
    public bool lockLeft;
    public bool lockRight;
    private float moveKD = 0.1f;
    private float turnKD = 0.1f;

    public float rotation = 0;
    public bool game = false;

    public List<GameObject> blocks = new List<GameObject>();

    void Update()
    {
        if (game)
        {
            if (curFigure == null)
            {
                curFigure = Instantiate(figures[Random.Range(0, figures.Length)]);
                curFigure.GetComponent<Figure>().Activate(speed);
                BoxCollider[] a = curFigure.GetComponentsInChildren<BoxCollider>();
                foreach (BoxCollider i in a)
                {
                    blocks.Add(i.gameObject);
                }
                lockLeft = false;
                lockRight = false;
                rotation = 0;
                CheckRows();
            }
            if (moveKD >= 0.1f && curFigure != null)
            {
                float x = Input.GetAxisRaw("Horizontal");
                if (x != 0 && ((x == -1 && !lockLeft) || (x == 1 && !lockRight)))
                {
                    curFigure.transform.position = new Vector3(curFigure.transform.position.x + (0.5f * x), curFigure.transform.position.y, curFigure.transform.position.z);
                    moveKD = 0;
                }
                lockRight = false;
                lockLeft = false;
            }
            else
            {
                moveKD += Time.deltaTime;
            }
            if (turnKD >= 0.1f && curFigure != null)
            {
                if (!lockLeft || lockRight)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        rotation += 90;
                        curFigure.transform.rotation = Quaternion.Euler(0, 0, rotation);
                    }
                }
                if (Input.GetAxisRaw("Vertical") == -1)
                {
                    curFigure.GetComponent<Figure>().speed = 5f;
                }
            }
            else
            {
                turnKD += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.Pause();
        }
    }

    private void CheckRows()
    {
        float y = 7.75f;
        while (y >= 0.25)
        {
            int x = 0;
            Collider[] colls = Physics.OverlapBox(new Vector3(0, y, -0.3f), new Vector3(7.5f, 0.1f, 0.1f));
            foreach (Collider coll in colls)
            {
                if(coll.tag == "Block")
                {
                    x++;
                }
            }
            if(y == 7.75f && x >= 1)
            {
                game = false;
                gm.GameOver();
                break;
            }
            else if(x == 15)
            {
                foreach (Collider coll in colls)
                {
                    if (coll.tag == "Block")
                    {
                        blocks.Remove(coll.gameObject);
                        Destroy(coll.gameObject);
                    }
                }
                foreach (GameObject i in blocks)
                {
                    if(i.transform.position.y > y)
                    {
                        i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y - 0.5f, i.transform.position.z);
                    }
                }
            }
            y -= 0.5f;
        }
    }

    public void SetSpeed(float sp)
    {
        speed = sp;
    }
}
