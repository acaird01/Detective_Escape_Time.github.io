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
    Vector2 inputVector; // x, z�� �޾ƿͼ� �ӽ÷� ������ ����
    Vector3 moveVector; // ���������� ĳ���͸� �����̱� ���� �ʿ��� ����

    private PlayerInputActions playerInputAction; // ���� action input asset
    private TextController textController;
    public bool invOpen = false; // �κ��丮 Ȱ��ȭ ����
    public bool escOpen = false; // ����â Ȱ��ȭ ����
    public bool keystrokes = false; // Ű�Է� �ȹް� �Ϸ��� > ��Ʈ��, �ε� �� ��� �̰� true�� �ٲ��ָ� true�� �� Ű �Է� �ȹްڴٴ� �Ҹ�
    bool runTrue = true;

    private float hotKeyNum = 1;

    public float playerMoveSpeed = 3;

    [SerializeField]
    private GameObject inventoryPanel; // �κ��丮 UI �г�
    [SerializeField]
    private GameObject reticleDot; // ������

    [SerializeField]
    Canvas escPanel; // ����â ĵ����
    AudioManager audioManager; // ����â �����ϴ� ����� �Ŵ���

    [SerializeField]
    GameObject itemTextPanel; // ������ ����â

    Rigidbody rigidbody;
    Camera characterCamera;
    public Canvas characterCanvas; // ��Ʈ�ο��� ī�޶�� player ui�����Ѱ� ������
    MouseCamLook mouseCamLook; // ���콺 Ŀ�� ������� �̰͵� �����;;
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
        playerInputAction.Enable(); // �׼� Asset Ȱ��ȭ 
        SceneManager.sceneLoaded += OnSceneLoad; // ������Ʈ Ȱ��ȭ �ɶ� �̺�Ʈ �Ҵ�
    }

    private void OnDisable() // �׼� Asset ��Ȱ��ȭ
    {
        playerInputAction.Disable();
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) // �� ���� �ε���� �� �����ϴ°�
    {
        if (FindObjectOfType<AudioManager>()) // �׾��� ����� �Ŵ����� �ִ��� ���� üũ
        {
            audioManager = FindObjectOfType<AudioManager>();
            /*if (GameManager.instance.nowSceneName != "01. IntroScene")
            {
                audioManager.SaveSensitivity();
            }*/
            escPanel = audioManager.GetComponentInChildren<Canvas>(); // ���� escpanel�Ҵ�
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

    void OnMovement(InputAction.CallbackContext value) // inputAction���� value �޾Ƽ� ��½�ų vector���� ����
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
                        textController.SetActiveFalseText();    // ��ȣ�ۿ� �ؽ�Ʈ ��Ȱ��ȭ
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
            ItemManager._instance.SelectHotkeySlot((int)hotKeyNum); // ��Ű ����
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
                        textController.SetActiveFalseText();    // ��ȣ�ۿ� �ؽ�Ʈ ��Ȱ��ȭ
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
        // ����â �߰� �ϱ�
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
    public void LoadingSceneCloseAll() // �ε������� ȣ��
    {
        keystrokes = true;
        characterCanvas.gameObject.SetActive(false);
        if(itemTextPanel)
        {
            itemTextPanel.gameObject.SetActive(false);
        }
        if (!escOpen && escPanel) // �ε��߿� ����â�� �����ִٸ� ����
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
        if (!invOpen && inventoryPanel) // �κ��丮 ���������� ����
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            inventoryPanel.SetActive(false);
            invOpen = false;

        }
        if (textController != null) // �ؽ�Ʈ �������� ����
        {
            textController.SetActiveFalseText();
        }
        if (ItemManager._instance.interaction_F)    // ��ȣ�ۿ� Ȯ�ο� �̹��� ��Ȱ��ȭ
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