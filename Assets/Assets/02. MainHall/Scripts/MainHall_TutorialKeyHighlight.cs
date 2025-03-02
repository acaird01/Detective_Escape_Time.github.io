using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHall_TutorialKeyHighlight : MonoBehaviour
{
    #region 오브젝트, 컴포넌트를 할당할 변수 모음
    private GameObject player;                              // 플레이어 오브젝트
    private MainHall_ObjectManager mainHall_ObjectManager;  // 복도씬의 오브젝트 매니저

    // 플레이어가 무슨 키를 눌렸는지 확인해서 적용하기 위한 이미지 오브젝트
    private KeyW keyW;
    private KeyA keyA;
    private KeyS keyS;
    private KeyD keyD;
    private KeyShift keyShift;
    private KeyI keyI;
    private KeyF keyF;
    private KeyEsc keyEsc;
    #endregion

    #region 메테리얼 배열 모음
    // 각 키가 눌렸을때 바꿔줄 메테리얼 배열
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
        // 각 키와 관련된 이미지를 가진 오브젝트를 찾아와서 할당
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
        // 튜토리얼 키 하이라이트 처리해줄 조건문 모음
        #region 키를 눌렀을때
        if (Input.GetKeyDown(KeyCode.W))    // w 키 입력
        {
            KeyImageChange(keyW.GetComponent<MeshRenderer>(), keyW_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.S))    // s 키 입력
        {
            KeyImageChange(keyS.GetComponent<MeshRenderer>(), keyS_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.A))    // a 키 입력
        {
            KeyImageChange(keyA.GetComponent<MeshRenderer>(), keyA_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.D))    // d 키 입력
        {
            KeyImageChange(keyD.GetComponent<MeshRenderer>(), keyD_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))    // shift 키 입력
        {
            KeyImageChange(keyShift.GetComponent<MeshRenderer>(), keyShift_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.I))    // i 키 입력
        {
            KeyImageChange(keyI.GetComponent<MeshRenderer>(), keyI_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.F))    // f 키 입력
        {
            KeyImageChange(keyF.GetComponent<MeshRenderer>(), keyF_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyDown(KeyCode.Escape))    // esc 키 입력
        {
            KeyImageChange(keyEsc.GetComponent<MeshRenderer>(), keyEsc_Material[1]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        #endregion

        #region 키를 땠을때
        if (Input.GetKeyUp(KeyCode.W))    // w 키 입력 종료
        {
            KeyImageChange(keyW.GetComponent<MeshRenderer>(), keyW_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.S))    // s 키 입력 종료
        {
            KeyImageChange(keyS.GetComponent<MeshRenderer>(), keyS_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.A))    // a 키 입력 종료
        {
            KeyImageChange(keyA.GetComponent<MeshRenderer>(), keyA_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.D))    // d 키 입력 종료
        {
            KeyImageChange(keyD.GetComponent<MeshRenderer>(), keyD_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))    // shift 키 입력 종료
        {
            KeyImageChange(keyShift.GetComponent<MeshRenderer>(), keyShift_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.I))    // i 키 입력 종료
        {
            KeyImageChange(keyI.GetComponent<MeshRenderer>(), keyI_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.F))    // f 키 입력 종료
        {
            KeyImageChange(keyF.GetComponent<MeshRenderer>(), keyF_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        if (Input.GetKeyUp(KeyCode.Escape))    // esc 키 입력 종료
        {
            KeyImageChange(keyEsc.GetComponent<MeshRenderer>(), keyEsc_Material[0]);    // 해당 키의 메쉬랜더러와 흰색 키 이미지 메테리얼로 함수 호출
        }
        #endregion
    }

    // 입력받은 키에 해당하는 이미지와 해당 이미지의 메테리얼을 바꾸기 위한 함수
    private void KeyImageChange(MeshRenderer _keyImage, Material _changeMaterial)
    {
        _keyImage.material = _changeMaterial;   // 해당 키 이미지의 메테리얼을 바꾸고자하는 메테리얼로 변경해서 적용
    }
}
