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

    public int maxStep;

    public List<Room> rooms = new List<Room>();

    List<GameObject> farRooms = new List<GameObject>();
    List<GameObject> lessRooms = new List<GameObject>();
    List<GameObject> oneDoorRooms = new List<GameObject>();

    public WallType wallType;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, StartPoint.position, Quaternion.identity).GetComponent<Room>());
            ChangePoint();
        }

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        foreach (var room in rooms)
        {
            SetRoom(room,room.transform.position);
        }
        FindEndRoom();
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

    public void SetRoom(Room room,Vector3 roomPosition) {
        room.uproom = Physics2D.OverlapCircle(roomPosition+new Vector3(0,Yoffset,0), 0.2f, roomlayer);
        room.downroom = Physics2D.OverlapCircle(roomPosition + new Vector3(0,-Yoffset, 0), 0.2f, roomlayer);
        room.leftroom = Physics2D.OverlapCircle(roomPosition + new Vector3(-Xoffset, 0, 0), 0.2f, roomlayer);
        room.rightroom = Physics2D.OverlapCircle(roomPosition + new Vector3(Xoffset, 0, 0), 0.2f, roomlayer);
        room.UpDateRoom();
        switch (room.doorNumber)
        {
            case 1:
                if (room.uproom)
                    Instantiate(wallType.singleup, roomPosition, Quaternion.identity);
                if (room.downroom)
                    Instantiate(wallType.singledown, roomPosition, Quaternion.identity);
                if (room.leftroom)
                    Instantiate(wallType.singleleft, roomPosition, Quaternion.identity);
                if (room.rightroom)
                    Instantiate(wallType.singleright, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (room.uproom&& room.downroom)
                    Instantiate(wallType.doubleupdown, roomPosition, Quaternion.identity);
                if (room.uproom && room.leftroom)
                    Instantiate(wallType.doubleupleft, roomPosition, Quaternion.identity);
                if (room.uproom && room.rightroom)
                    Instantiate(wallType.doubleupright, roomPosition, Quaternion.identity);
                if (room.leftroom&& room.downroom)
                    Instantiate(wallType.doubleleftdown, roomPosition, Quaternion.identity);
                if (room.leftroom && room.rightroom)
                    Instantiate(wallType.doubleleftright, roomPosition, Quaternion.identity);
                if (room.rightroom&&room.downroom)
                    Instantiate(wallType.doubledownright, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (room.uproom==false)
                    Instantiate(wallType.triglenoup, roomPosition, Quaternion.identity);
                if (room.downroom == false)
                    Instantiate(wallType.triglenodown, roomPosition, Quaternion.identity);
                if (room.rightroom == false)
                    Instantiate(wallType.triglenoright, roomPosition, Quaternion.identity);
                if (room.leftroom == false)
                    Instantiate(wallType.triglenoleft, roomPosition, Quaternion.identity);
                break;
            case 4:
                Instantiate(wallType.four, roomPosition, Quaternion.identity);
                break;
        }
    }

    public void FindEndRoom() {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessRooms.Add(room.gameObject);
        }

        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                oneDoorRooms.Add(farRooms[i]);
        }

        for (int i = 0; i < lessRooms.Count; i++)
        {
            if (lessRooms[i].GetComponent<Room>().doorNumber == 1)
                oneDoorRooms.Add(lessRooms[i]);
        }
        if (oneDoorRooms.Count != 0)
            endroom = oneDoorRooms[Random.Range(0, oneDoorRooms.Count)];
        else if(oneDoorRooms.Count == 0)
            endroom = farRooms[Random.Range(0, oneDoorRooms.Count)]; 
    }
}

[System.Serializable]
public class WallType
{
    public GameObject singleup, singledown, singleleft, singleright,
        doubleupdown, doubleupleft, doubleupright, doubleleftright, doubleleftdown, doubledownright,
        triglenoup, triglenodown, triglenoleft, triglenoright,
        four;
}
