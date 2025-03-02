using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_StructureTree : MonoBehaviour
{
    public GameObject child_Tree; // 자라게 될 자식으로 들고있는 나무 여기 materals 바꿔서 나무 자라는거 구현할 예정
    
    Renderer childTree_Renderer; // 자라게 될 나무의 메테리얼 값 조절하기 위해 랜더러 변수에 저장 (최적화?)
    BoxCollider ParentTree_BoxCol; // 나무가 자란 후에 부모 오브젝트에서 켜줄 콜라이더, 나무가 자란 후 나무 자체에 상호작용 시 위치 알려주기 위해서
    MeshCollider child_Bed_MeshCol; // 나무가 자라기 전 화분 콜라이더 여기 상호작용 해서 나무를 키울 예정, 나무가 자란 후엔 끄기위해
    Image TakoPos_Leaf; // 타코 위치가 적힌 잎파리
    bool image_SetActive; // 이미지 켜져있는지 체크할 불값
    Kiara_TreeBed treeBed;

    GameObject player;
    TextController textController;

    Interaction_Gimics interaction;
    

    public Material tree_Wood; // 나무줄기 메테리얼
    public Material tree_Leaf; // 나무 잎파리 메테리얼
    Material[] mat;
    Material[] mat2; // renderer에 material에 셋팅될 material[] 이걸 바꿔 끼워줌으로써 외형 변경

    string Growthing_Text = "무언가 자라나는 것 같은데..\n점점 커지잖아!"; // 나무 자라기 시작하면 나올 텍스트
    string after_Growth_Text = "잎파리에 무언가 적혀있어.\n어디 자세히 봐야겠어."; // 성장한 후에 상호작용시 나올 텍스트

    public GameObject[] ParticleEffect;

    AudioSource audioSource;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        childTree_Renderer = child_Tree.gameObject.GetComponent<Renderer>();
        mat = new Material[1];
        mat2 = new Material[2];
        ParentTree_BoxCol = gameObject.GetComponent<BoxCollider>();
        child_Bed_MeshCol = gameObject.GetComponentInChildren<MeshCollider>();
        TakoPos_Leaf = gameObject.GetComponentInChildren<Image>();
        TakoPos_Leaf.gameObject.SetActive(false);
        treeBed = gameObject.GetComponentInChildren<Kiara_TreeBed>();
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        textController = player.GetComponentInChildren<TextController>();
        interaction = gameObject.GetComponentInChildren<Interaction_Gimics>();
        //interaction.run_Gimic = false; // 테스트용!!!!!!!!!!!!!!! 나중에 지우자!!!!!!!!!!!!!!!!!!!
        animator = gameObject.GetComponentInChildren<Animator>();

        SceneStartEffectSet();
        SetMaterials(); // 자라는 나무의 메테리얼 셋팅
        //SceneStartSetting_StructureTree(); // 기믹수행 여부에 따른 초기 셋팅
        StartCoroutine(WaitTouch());
    }


    void SceneStartEffectSet()
    {
        for(int i = 0; i < ParticleEffect.Length; i++)
        {
            ParticleEffect[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator GrowthTree() // 나무 자라기
    {
        StartCoroutine(textController.SendText(Growthing_Text));
        child_Bed_MeshCol.enabled = false;
        yield return new WaitForSeconds(1f);
        child_Tree.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        childTree_Renderer.materials = mat; // 줄기만
        ParticleEffect[0].gameObject.SetActive(true);
        audioSource.Play();
        animator.Play("Tree");
        yield return new WaitForSeconds(3.1f);
        for (int i = 1; i < ParticleEffect.Length; i++)
        {
            ParticleEffect[i].gameObject.SetActive(true);
        }
        audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        childTree_Renderer.materials = mat2; // 줄기 + 잎
        yield return new WaitForSeconds(1f);
        SceneStartEffectSet(); // 이펙트 비활성화 재활용하는거
        ParentTree_BoxCol.enabled = true;
        StartCoroutine(textController.SendText(after_Growth_Text));
    }

    IEnumerator WaitTouch() // 나무가 다 자란 상태에서 상호작용 할 예정 자라기전엔 콜라이더 끔으로써 상호작용 x
    {
        while (player)
        {
            if (interaction.run_Gimic == false)
            {
                yield return new WaitUntil(() => interaction.run_Gimic == true);
                if (!image_SetActive)
                {
                    textController.SetActiveFalseText();
                    player.GetComponentInChildren<PlayerCtrl>().keystrokes = true;
                    TakoPos_Leaf.gameObject.SetActive(true); // 상호작용 시 위치적힌 잎파리 띄워주기
                    image_SetActive = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                }
            }
            else
            {
                yield return new WaitUntil(() => interaction.run_Gimic == false);
                if(!image_SetActive)                
                {
                    textController.SetActiveFalseText();
                    player.GetComponentInChildren<PlayerCtrl>().keystrokes = true;
                    TakoPos_Leaf.gameObject.SetActive(true); // 상호작용 시 위치적힌 잎파리 띄워주기
                    image_SetActive = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                }
            }
        }
    }
    public void CanvasClose() // x버튼에 들어갈꺼
    {
        Time.timeScale = 1;
        player.GetComponentInChildren<PlayerCtrl>().keystrokes = false;
        TakoPos_Leaf.gameObject.SetActive(false);
        image_SetActive = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetMaterials()
    {
        mat[0] = tree_Wood;
        mat2[0] = tree_Wood;
        mat2[1] = tree_Leaf;
    }

    public void SceneStartSetting_StructureTree()
    {
        if (treeBed.Growth_Tree_True)
        {
            ParentTree_BoxCol.enabled = true;
            child_Bed_MeshCol.enabled = false;
            childTree_Renderer.materials = mat2;
        }
        else
        {
            ParentTree_BoxCol.enabled = false;
            child_Bed_MeshCol.enabled = true;
            childTree_Renderer.materials = new Material[0];
        }
    }
}
