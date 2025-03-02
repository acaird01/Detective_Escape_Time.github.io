using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gimic_One_StepDrawing : MonoBehaviour
{
    public List<GameObject> nodes; // 노드 리스트
    public LineRenderer linePrefab; // 간선(LineRenderer) 프리팹 > 이건 리스트에 중복 체크위해 저장할 경로
    private GameObject selectedNode1 = null; // 첫 번째 선택된 노드
    private GameObject selectedNode2 = null; // 두 번째 선택된 노드
    public List<LineRenderer> drawnEdges = new List<LineRenderer>(); // 그려진 간선
    public HashSet<string> existingEdges = new HashSet<string>();

    Usual_Room usual_Room;
    OneStepDrawingOnOff drawingOnOff;
    Kiara_DrawingCheck drawingCheck;


    private Dictionary<int, List<int>> CanGoList = new Dictionary<int, List<int>>
    {
        {1, new List<int> {2, 5} },
        {2, new List<int> {1, 3, 5} },
        {3, new List<int> {2, 4} },
        {4, new List<int> {3, 5} },
        {5, new List<int> {1, 2, 4} }
    };

    private void OnEnable()
    {
        StartCoroutine(Clear_OneStepDrawing());
    }
    // Start is called before the first frame update
    void Start()
    {
        usual_Room = GameObject.Find("Usual_Room_Door").GetComponent<Usual_Room>();
        drawingOnOff = usual_Room.gameObject.GetComponentInChildren<OneStepDrawingOnOff>();
        drawingCheck = usual_Room.gameObject.GetComponentInChildren<Kiara_DrawingCheck>();
        //StartCoroutine(Clear_OneStepDrawing());
    }

    private void Update()
    {
        if(selectedNode1 != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.y;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    IEnumerator Clear_OneStepDrawing() // 성공했을때
    {
        yield return new WaitUntil(() => OneStepDrawing_ClearCheck(6) == true);
        
        usual_Room.DoorOpen = true;
        drawingCheck.SaveDrawingData(true);
        drawingCheck.SceneStartSetting();
        for (int i = 0; i < drawnEdges.Count; i++)
        {
            Destroy(drawnEdges[i].gameObject);
        }
        //interaction.run_Gimic = true;
        drawingOnOff.CanvasClose();
        
    }
    void FailDrawing() // 실패했을 대
    {
        drawingOnOff.CanvasClose();
    }

    

    public void SelectNode()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        GameObject selectedNode = nodes[int.Parse(clickObject.name) - 1];
        if (selectedNode1 == null) // 첫 번째 노드 선택
        {
            selectedNode1 = selectedNode;
        }
        else if (selectedNode2 == null) // 두 번째 노드 선택
        {   
            selectedNode2 = selectedNode;

            if (selectedNode1 == selectedNode2) // 똑같은거 두번 선택했을 때
            {
                ResetData();
                FailDrawing();
                return;
            }

            string edgeKey = GetEdgeKey(selectedNode1, selectedNode2);
            if(existingEdges.Contains(edgeKey))
            {
                ResetData();
                FailDrawing(); // 중복된 경로를 탔을때
                return;
            }

            int firstNode = int.Parse(selectedNode1.gameObject.name);
            int secondNode = int.Parse(selectedNode2.gameObject.name);

            if (!CanGoList[firstNode].Contains(secondNode))
            {
                ResetData();
                FailDrawing();
                return;
            }

            // 두 노드를 연결
            DrawEdge(selectedNode1.transform.position, selectedNode2.transform.position);
            existingEdges.Add(edgeKey);

            // 상태 초기화
            selectedNode1 = selectedNode2;
            selectedNode2 = null;
        }
    }

    string GetEdgeKey(GameObject node1, GameObject node2) // 키값 생성해서 중복체크
    {
        int id1 = int.Parse(node1.name);
        int id2 = int.Parse(node2.name);
        return id1 < id2 ? $"{id1}-{id2}" : $"{id2}-{id1}"; // 앞의 조건문이 참일 때 :의 앞 반환, 거짓일 때 뒷부분 반환
        // 클릭된 노드 1번, 2번의 값을 비교해 몇번 노드에서 몇번 노드로 이동했는지 저장한다.
        // 각 노드는 이름이 1, 2이런식으로 세팅되있고, 클릭된 노드들의 크기(순서)를 비교해 id1 < id2가 참일때 {id1}-{id2}, 거짓일때 {id2}-{id1}값을 키값으로 저장해
        // 중복된 값이 들어올 경우 같은 경로를 탔다는 거기에 실패처리된다.
    }

    public void ResetData()
    {
        for (int i = 0; i < drawnEdges.Count; i++)
        {
            Destroy(drawnEdges[i].gameObject);
        }
        drawnEdges = new List<LineRenderer>();
        existingEdges = new HashSet<string>();
        selectedNode1 = null;
        selectedNode2 = null;
    }

    void DrawEdge(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
        drawnEdges.Add(line);
    }

    public bool OneStepDrawing_ClearCheck(int totalNodes)
    {
        return (drawnEdges.Count == totalNodes); 
    }


}
