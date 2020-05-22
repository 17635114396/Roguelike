using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomgenerater : MonoBehaviour
{
    public enum Direction { up,down,left,right};
    public Direction dir;
    public LayerMask roomlayer;

    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor;
    public Color endColor;
    private GameObject endroom;

    [Header("位置信息")]
    public Transform StartPoint;
    public float Xoffset;
    public float Yoffset;

    public List<GameObject> rooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, StartPoint.position, Quaternion.identity));
            ChangePoint();
        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        endroom = rooms[0];
        foreach (var room in rooms)
        {
            if (room.transform.position.sqrMagnitude>endroom.transform.position.sqrMagnitude) {
                endroom = room;
            }
        }
        endroom.GetComponent<SpriteRenderer>().color = endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void ChangePoint() {
        do
        {
            dir = (Direction)Random.Range(0, 4);
            switch (dir)
            {
                case Direction.up:
                    StartPoint.position += new Vector3(0, Yoffset, 0);
                    break;
                case Direction.down:
                    StartPoint.position += new Vector3(0, -Yoffset, 0);
                    break;
                case Direction.left:
                    StartPoint.position += new Vector3(-Xoffset, 0, 0);
                    break;
                case Direction.right:
                    StartPoint.position += new Vector3(Xoffset, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(StartPoint.position, 0.2f, roomlayer));
    }
}
