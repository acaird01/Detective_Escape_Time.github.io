using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gimic_One_StepDrawing : MonoBehaviour
{
    public List<GameObject> nodes; // ��� ����Ʈ
    public LineRenderer linePrefab; // ����(LineRenderer) ������ > �̰� ����Ʈ�� �ߺ� üũ���� ������ ���
    private GameObject selectedNode1 = null; // ù ��° ���õ� ���
    private GameObject selectedNode2 = null; // �� ��° ���õ� ���
    public List<LineRenderer> drawnEdges = new List<LineRenderer>(); // �׷��� ����
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

    IEnumerator Clear_OneStepDrawing() // ����������
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
    void FailDrawing() // �������� ��
    {
        drawingOnOff.CanvasClose();
    }

    

    public void SelectNode()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        GameObject selectedNode = nodes[int.Parse(clickObject.name) - 1];
        if (selectedNode1 == null) // ù ��° ��� ����
        {
            selectedNode1 = selectedNode;
        }
        else if (selectedNode2 == null) // �� ��° ��� ����
        {   
            selectedNode2 = selectedNode;

            if (selectedNode1 == selectedNode2) // �Ȱ����� �ι� �������� ��
            {
                ResetData();
                FailDrawing();
                return;
            }

            string edgeKey = GetEdgeKey(selectedNode1, selectedNode2);
            if(existingEdges.Contains(edgeKey))
            {
                ResetData();
                FailDrawing(); // �ߺ��� ��θ� ������
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

            // �� ��带 ����
            DrawEdge(selectedNode1.transform.position, selectedNode2.transform.position);
            existingEdges.Add(edgeKey);

            // ���� �ʱ�ȭ
            selectedNode1 = selectedNode2;
            selectedNode2 = null;
        }
    }

    string GetEdgeKey(GameObject node1, GameObject node2) // Ű�� �����ؼ� �ߺ�üũ
    {
        int id1 = int.Parse(node1.name);
        int id2 = int.Parse(node2.name);
        return id1 < id2 ? $"{id1}-{id2}" : $"{id2}-{id1}"; // ���� ���ǹ��� ���� �� :�� �� ��ȯ, ������ �� �޺κ� ��ȯ
        // Ŭ���� ��� 1��, 2���� ���� ���� ��� ��忡�� ��� ���� �̵��ߴ��� �����Ѵ�.
        // �� ���� �̸��� 1, 2�̷������� ���õ��ְ�, Ŭ���� ������ ũ��(����)�� ���� id1 < id2�� ���϶� {id1}-{id2}, �����϶� {id2}-{id1}���� Ű������ ������
        // �ߺ��� ���� ���� ��� ���� ��θ� ���ٴ� �ű⿡ ����ó���ȴ�.
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
