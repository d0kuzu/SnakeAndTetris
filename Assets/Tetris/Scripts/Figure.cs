using UnityEngine;

public class Figure : MonoBehaviour
{
    private bool falling;
    public float speed;
    public Tetris tetris;

    private void Start()
    {
        tetris = Camera.main.GetComponent<Tetris>();
    }

    void Update()
    {
        if (falling && tetris.game)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        }
        else if (tetris.game)
        {
            Transform[] asd = GetComponentsInChildren<Transform>();
            Debug.Log(asd.Length);
            if(asd.Length == 1) { Destroy(this.gameObject); }
        }
    }

    public void Activate(float sp)
    {
        falling = true;
        speed = sp;
    }

    public void Deactivate()
    {        
        falling = false;
        Blocks[] bl = GetComponentsInChildren<Blocks>();
        foreach (Blocks b in bl)
        {
            b.Deactivate();
        }
        tetris.curFigure = null;
    }
}
