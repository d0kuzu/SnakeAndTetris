using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private List<GameObject> snake;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject tale;

    private GameObject curApple = null;

    public bool game = false;
    private bool moving;
    private float speed = 10;
    private float moveSpeed = 0.25f;
    private int moveSide = 1;
    private int side = 1;

    private int skipNum = 4;
    private int skip;
    private int skipTale;
    private List<Vector3> movePos = new();

    void Start()
    {
    }

    void Update()
    {
        if (game)
        {
            if (curApple == null)
            {
                SpawnApple();
            }
            if (moveSide != 1 && moveSide != 3)
            {
                float x = Input.GetAxisRaw("Horizontal");
                side = x == 1 ? 1 : x == -1 ? 3 : side;
            }
            else
            {
                float y = Input.GetAxisRaw("Vertical");
                side = y == 1 ? 4 : y == -1 ? 2 : side;
            }
            Move(moving);
            CheckHead();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.Pause();
        }
    }

    private void SpawnApple()
    {
        List<Vector3> spaces = new();
        for (float y = -10.5f; y < 11; y++)
        {
            for (float x = -10.5f; x < 11; x++)
            {
                if (snake.Find(a => x - 1 < a.transform.position.x && a.transform.position.x < x + 1 && y - 1 < a.transform.position.y && a.transform.position.y < y + 1) == null)
                {
                    spaces.Add(new Vector3(x,y));
                }
            }
        }
        curApple = Instantiate(apple, spaces[Random.Range(0, spaces.Count)], Quaternion.Euler(0,0,0));
    }

    private void EatApple()
    {
        Debug.Log("eat");
        Destroy(curApple);
        curApple = null;
        snake.Insert(0, Instantiate(tale, snake[0].transform.position, Quaternion.Euler(0,0,0)));
        snake.Insert(0, Instantiate(tale, snake[0].transform.position, Quaternion.Euler(0, 0, 0)));
        snake.Insert(0, Instantiate(tale, snake[0].transform.position, Quaternion.Euler(0, 0, 0)));
        snake.Insert(0, Instantiate(tale, snake[0].transform.position, Quaternion.Euler(0, 0, 0)));
        skipTale = 4;
    }

    private void Move(bool moving)
    {
        if (!moving)
        {
            int newMoveSide = side;
            skip--;
            if (skip == 0)
            {
                moveSide = side;
            }
            else
            {
                newMoveSide = moveSide;
            }
            movePos.Clear();
            for (int i = skipTale; i < snake.Count; i++)
            {
                if (i == snake.Count - 1) movePos.Add(new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z));
                
                else movePos.Add(new Vector3(snake[i+1].transform.position.x, snake[i+1].transform.position.y, snake[i+1].transform.position.z));
            }
            if (newMoveSide == 1) movePos.Add(new Vector3(head.transform.position.x + moveSpeed, head.transform.position.y, head.transform.position.z));
            else if (newMoveSide == 2) movePos.Add(new Vector3(head.transform.position.x, head.transform.position.y - moveSpeed, head.transform.position.z));
            else if (newMoveSide == 3) movePos.Add(new Vector3(head.transform.position.x - moveSpeed, head.transform.position.y, head.transform.position.z));
            else if (newMoveSide == 4) movePos.Add(new Vector3(head.transform.position.x, head.transform.position.y + moveSpeed, head.transform.position.z));
            this.moving = true;
            skip = skip <= 0 ? skipNum : skip;
        }
        else
        {
            bool done = true;
            for (int i = skipTale; i < snake.Count; i++)
            {
                if(snake[i].transform.position != movePos[i-skipTale])
                {
                    snake[i].transform.position = Vector3.MoveTowards(snake[i].transform.position, movePos[i - skipTale], speed * Time.deltaTime);
                    done = false;
                }
            }
            if (done && head.transform.position == movePos.Last()) 
            { 
                this.moving = false;
                skipTale = skipTale > 0 ? skipTale - 1 : 0;
            }
            else if (head.transform.position != movePos.Last())
            {
                head.transform.position = Vector3.MoveTowards(head.transform.position, movePos.Last(), speed * Time.deltaTime);
            }
        }
    }

    private void CheckHead()
    {
        float x = head.transform.position.x;
        float y = head.transform.position.y;
        if (moveSide == 1) x += 0.45f;
        else if (moveSide == 3) x -= 0.45f;
        else if (moveSide == 2) y -= 0.45f;
        else if (moveSide == 4) y += 0.45f;
        Vector2 overSide = new Vector2(x, y);
        Collider2D[] colls = Physics2D.OverlapCircleAll(overSide, 0.01f);
        foreach (Collider2D coll in colls)
        {
            if (coll.tag == "Apple")
            {
                EatApple();
            }
            else if (coll.tag == "Snake" || coll.tag == "Wall")
            {
                Debug.Log(coll.name);
                game = false;
                gm.GameOver();
                break;
            }
        }
    }

    public void SetSpeed(float sp)
    {
        speed = sp;
    }
}
