using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance;
    Vector2 inputVector; // x, z값 받아와서 임시로 저장할 변수
    Vector3 moveVector; // 실질적으로 캐릭터를 움직이기 위해 필요한 벡터

    private PlayerInputActions playerInputAction; // 만든 action input asset
    private TextController textController;
    public bool invOpen = false; // 인벤토리 활성화 유무
    public bool escOpen = false; // 설정창 활성화 유무
    public bool keystrokes = false; // 키입력 안받게 하려고 > 인트로, 로딩 씬 등에서 이걸 true로 바꿔주면 true일 때 키 입력 안받겠다는 소리
    bool runTrue = true;

    private float hotKeyNum = 1;

    public float playerMoveSpeed = 3;

    [SerializeField]
    private GameObject inventoryPanel; // 인벤토리 UI 패널
    [SerializeField]
    private GameObject reticleDot; // 조준점

    [SerializeField]
    Canvas escPanel; // 설정창 캔버스
    AudioManager audioManager; // 설정창 관리하는 오디오 매니저

    [SerializeField]
    GameObject itemTextPanel; // 아이템 설명창

    Rigidbody rigidbody;
    Camera characterCamera;
    public Canvas characterCanvas; // 인트로에서 카메라랑 player ui관련한거 끄려고
    MouseCamLook mouseCamLook; // 마우스 커서 사라져서 이것도 꺼얄듯;;
    Number_if_Gimic number_Gimic;

    private void Awake()
    {
        playerInputAction = new PlayerInputActions();

        playerInputAction.Player.Move.started += OnMovement; 
        playerInputAction.Player.Move.performed += OnMovement; 
        playerInputAction.Player.Move.canceled += OnMovement; 

        playerInputAction.Player.MoveFast.performed += Speed_Fast; 
        playerInputAction.Player.MoveFast.canceled += Speed_Restoration;

        playerInputAction.Player.Inv.started += Inventory; 

        playerInputAction.Player.HotKey.started += HotKey; 

        playerInputAction.Player.ESC.started += ESC;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        textController = GetComponentInChildren<TextController>();
        rigidbody = GetComponent<Rigidbody>();
        characterCamera = gameObject.GetComponentInChildren<Camera>();
        mouseCamLook = characterCamera.GetComponent<MouseCamLook>();
        characterCanvas = gameObject.GetComponentInChildren<Canvas>();
        StartCoroutine(Intro());
    }

    private void Update()
    {
        if (!keystrokes)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ItemManager._instance.hotkeyItemIndex == 10)
                {
                    Item_Irys_Halo halo = GameObject.FindAnyObjectByType<Item_Irys_Halo>();
                    halo.HaloUse();
                }
                if (ItemManager._instance.hotkeyItemIndex == 33)
                {
                    Kiara_MemoUse memoUse = GameObject.FindAnyObjectByType<Kiara_MemoUse>();
                    memoUse.MemoUse();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!keystrokes)
        {
            //shoot ray to sense wall collider, if so set playerMoveSpeed to 0.1
            if (Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.GetMask("Wall")))
            {
                playerMoveSpeed = 0.5f;
            }
            else
            {
                if (!runTrue)
                {
                    playerMoveSpeed = 5f;
                }
                else
                {
                    playerMoveSpeed = 8f;
                }
            }
            rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveVector) * playerMoveSpeed * Time.deltaTime);

            
        }
    }

    private void OnEnable()
    {
        playerInputAction.Enable(); // 액션 Asset 활성화 
        SceneManager.sceneLoaded += OnSceneLoad; // 오브젝트 활성화 될때 이벤트 할당
    }

    private void OnDisable() // 액션 Asset 비활성화
    {
        playerInputAction.Disable();
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) // 씬 새로 로드됬을 때 실행하는거
    {
        if (FindObjectOfType<AudioManager>()) // 그씬에 오디오 매니저가 있는지 먼저 체크
        {
            audioManager = FindObjectOfType<AudioManager>();
            /*if (GameManager.instance.nowSceneName != "01. IntroScene")
            {
                audioManager.SaveSensitivity();
            }*/
            escPanel = audioManager.GetComponentInChildren<Canvas>(); // 새로 escpanel할당
            escPanel.gameObject.SetActive(false);
        }
        if(FindObjectOfType<Number_if_Gimic>())
        {
            number_Gimic = FindObjectOfType<Number_if_Gimic>();
        }
    }

    IEnumerator Intro()
    {
        characterCamera.gameObject.SetActive(false);
        characterCanvas.gameObject.SetActive(false);
        mouseCamLook.enabled = false;
        yield return new WaitUntil(() => GameManager.instance.nowSceneName != "01. IntroScene");
        characterCamera.gameObject.SetActive(true);
        characterCanvas.gameObject.SetActive(true);
        mouseCamLook.enabled = true;
    }

    void OnMovement(InputAction.CallbackContext value) // inputAction에서 value 받아서 출력시킬 vector값에 저장
    {
        inputVector = value.ReadValue<Vector2>();
        moveVector = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    void Speed_Fast(InputAction.CallbackContext value)
    {
        //playerMoveSpeed *= 2;
        runTrue = true;
    }

    void Speed_Restoration(InputAction.CallbackContext value)
    {
        //playerMoveSpeed /= 2;
        runTrue = false;
    }


    void Inventory(InputAction.CallbackContext value)
    {
        if (!keystrokes)
        {
            if (!escOpen)
            {
                invOpen = !invOpen;
                if (invOpen)
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;

                    if (textController != null)
                    {
                        textController.SetActiveFalseText();    // 상호작용 텍스트 비활성화
                    }
                    inventoryPanel.SetActive(true);

                    reticleDot.SetActive(false);
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    inventoryPanel.SetActive(false);

                    reticleDot.SetActive(true);
                }
            }
        }
    }

    void HotKey(InputAction.CallbackContext value)
    {
        if (!keystrokes)
        {
            hotKeyNum = float.Parse(value.control.displayName);
            ItemManager._instance.SelectHotkeySlot((int)hotKeyNum); // 핫키 선택
        }
    }

    void ESC(InputAction.CallbackContext value)
    {
        if (!keystrokes)
        {
            if (!invOpen)
            {
                escOpen = !escOpen;
                if (escOpen)
                {
                    if (textController != null)
                    {
                        textController.SetActiveFalseText();    // 상호작용 텍스트 비활성화
                    }
                    if (number_Gimic != null)
                    {
                        number_Gimic.PanelOpen();
                    }
                    Cursor.lockState = CursorLockMode.None;

                    if (escPanel)
                    {
                        escPanel.gameObject.SetActive(true);
                    }
                    
                    Time.timeScale = 0;

                    if (reticleDot)
                    {
                        reticleDot.SetActive(false);
                    }
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    if (number_Gimic != null)
                    {
                        number_Gimic.PanelClose();
                    }

                    if (escPanel)
                    {
                        escPanel.gameObject.SetActive(false);
                    }

                    if (audioManager)
                    {
                        audioManager.SaveSensitivity();
                    }

                    if (reticleDot)
                    {
                        reticleDot.SetActive(true);
                    }
                }
            }
            else if (invOpen)
            {
                invOpen = !invOpen;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;

                inventoryPanel.SetActive(false);
                reticleDot.SetActive(true);
            }
        }
        else
        {
            return;
        }
        // 설정창 뜨게 하기
    }

    public void ExitEscButton()
    {
        escOpen = !escOpen;
        if (!escOpen)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            escPanel.gameObject.SetActive(false);
            if (number_Gimic != null)
            {
                number_Gimic.PanelClose();
            }
            reticleDot.SetActive(true);
        }
    }
    public void ExitInvButton()
    {
        invOpen = !invOpen;
        if (!invOpen)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryPanel.SetActive(false);

            reticleDot.SetActive(true);
        }
    }
    public void LoadingSceneCloseAll() // 로딩씬에서 호출
    {
        keystrokes = true;
        characterCanvas.gameObject.SetActive(false);
        if(itemTextPanel)
        {
            itemTextPanel.gameObject.SetActive(false);
        }
        if (!escOpen && escPanel) // 로딩중에 설정창이 열려있다면 끄기
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            escPanel.gameObject.SetActive(false);
            escOpen = false;

        }
        if (escOpen)
        {
            escOpen = false;
        }
        if (!invOpen && inventoryPanel) // 인벤토리 열려있으면 끄기
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryPanel.SetActive(false);
            invOpen = false;

        }
        if (textController != null) // 텍스트 열려있음 끄기
        {
            textController.SetActiveFalseText();
        }
        if (ItemManager._instance.interaction_F)    // 상호작용 확인용 이미지 비활성화
        {
            ItemManager._instance.interaction_F.SetActive(false);
        }

    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        StartCoroutine(TimeStop());
    //    }

    //}

    //IEnumerator TimeStop()
    //{
    //    Time.timeScale = 0;
    //    yield return new WaitForSeconds(0.00000000000000001f);
    //    Time.timeScale = 1;
    //}
}