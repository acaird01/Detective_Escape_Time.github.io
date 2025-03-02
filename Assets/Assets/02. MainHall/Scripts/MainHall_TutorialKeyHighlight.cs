using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_TutorialKeyHighlight : MonoBehaviour
{
    #region ������Ʈ, ������Ʈ�� �Ҵ��� ���� ����
    private GameObject player;                              // �÷��̾� ������Ʈ
    private MainHall_ObjectManager mainHall_ObjectManager;  // �������� ������Ʈ �Ŵ���

    // �÷��̾ ���� Ű�� ���ȴ��� Ȯ���ؼ� �����ϱ� ���� �̹��� ������Ʈ
    private KeyW keyW;
    private KeyA keyA;
    private KeyS keyS;
    private KeyD keyD;
    private KeyShift keyShift;
    private KeyI keyI;
    private KeyF keyF;
    private KeyEsc keyEsc;
    #endregion

    #region ���׸��� �迭 ����
    // �� Ű�� �������� �ٲ��� ���׸��� �迭
    public Material[] keyW_Material;
    public Material[] keyA_Material;
    public Material[] keyS_Material;
    public Material[] keyD_Material;
    public Material[] keyShift_Material;
    public Material[] keyI_Material;
    public Material[] keyF_Material;
    public Material[] keyEsc_Material;
    #endregion  

    // Start is called before the first frame update
    void Awake()
    {
        // �� Ű�� ���õ� �̹����� ���� ������Ʈ�� ã�ƿͼ� �Ҵ�
        keyW = gameObject.GetComponentInChildren<KeyW>();
        keyA = gameObject.GetComponentInChildren<KeyA>();
        keyS = gameObject.GetComponentInChildren<KeyS>();
        keyD = gameObject.GetComponentInChildren<KeyD>();
        keyShift = gameObject.GetComponentInChildren<KeyShift>();
        keyI = gameObject.GetComponentInChildren<KeyI>();
        keyF = gameObject.GetComponentInChildren<KeyF>();
        keyEsc = gameObject.GetComponentInChildren<KeyEsc>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ʃ�丮�� Ű ���̶���Ʈ ó������ ���ǹ� ����
        #region Ű�� ��������
        if (Input.GetKeyDown(KeyCode.W))    // w Ű �Է�
        {
            KeyImageChange(keyW.GetComponent<MeshRenderer>(), keyW_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.S))    // s Ű �Է�
        {
            KeyImageChange(keyS.GetComponent<MeshRenderer>(), keyS_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.A))    // a Ű �Է�
        {
            KeyImageChange(keyA.GetComponent<MeshRenderer>(), keyA_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.D))    // d Ű �Է�
        {
            KeyImageChange(keyD.GetComponent<MeshRenderer>(), keyD_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))    // shift Ű �Է�
        {
            KeyImageChange(keyShift.GetComponent<MeshRenderer>(), keyShift_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.I))    // i Ű �Է�
        {
            KeyImageChange(keyI.GetComponent<MeshRenderer>(), keyI_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.F))    // f Ű �Է�
        {
            KeyImageChange(keyF.GetComponent<MeshRenderer>(), keyF_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyDown(KeyCode.Escape))    // esc Ű �Է�
        {
            KeyImageChange(keyEsc.GetComponent<MeshRenderer>(), keyEsc_Material[1]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        #endregion

        #region Ű�� ������
        if (Input.GetKeyUp(KeyCode.W))    // w Ű �Է� ����
        {
            KeyImageChange(keyW.GetComponent<MeshRenderer>(), keyW_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.S))    // s Ű �Է� ����
        {
            KeyImageChange(keyS.GetComponent<MeshRenderer>(), keyS_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.A))    // a Ű �Է� ����
        {
            KeyImageChange(keyA.GetComponent<MeshRenderer>(), keyA_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.D))    // d Ű �Է� ����
        {
            KeyImageChange(keyD.GetComponent<MeshRenderer>(), keyD_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))    // shift Ű �Է� ����
        {
            KeyImageChange(keyShift.GetComponent<MeshRenderer>(), keyShift_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.I))    // i Ű �Է� ����
        {
            KeyImageChange(keyI.GetComponent<MeshRenderer>(), keyI_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.F))    // f Ű �Է� ����
        {
            KeyImageChange(keyF.GetComponent<MeshRenderer>(), keyF_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        if (Input.GetKeyUp(KeyCode.Escape))    // esc Ű �Է� ����
        {
            KeyImageChange(keyEsc.GetComponent<MeshRenderer>(), keyEsc_Material[0]);    // �ش� Ű�� �޽��������� ��� Ű �̹��� ���׸���� �Լ� ȣ��
        }
        #endregion
    }

    // �Է¹��� Ű�� �ش��ϴ� �̹����� �ش� �̹����� ���׸����� �ٲٱ� ���� �Լ�
    private void KeyImageChange(MeshRenderer _keyImage, Material _changeMaterial)
    {
        _keyImage.material = _changeMaterial;   // �ش� Ű �̹����� ���׸����� �ٲٰ����ϴ� ���׸���� �����ؼ� ����
    }
}
