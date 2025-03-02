using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiara_StructureTree : MonoBehaviour
{
    public GameObject child_Tree; // �ڶ�� �� �ڽ����� ����ִ� ���� ���� materals �ٲ㼭 ���� �ڶ�°� ������ ����
    
    Renderer childTree_Renderer; // �ڶ�� �� ������ ���׸��� �� �����ϱ� ���� ������ ������ ���� (����ȭ?)
    BoxCollider ParentTree_BoxCol; // ������ �ڶ� �Ŀ� �θ� ������Ʈ���� ���� �ݶ��̴�, ������ �ڶ� �� ���� ��ü�� ��ȣ�ۿ� �� ��ġ �˷��ֱ� ���ؼ�
    MeshCollider child_Bed_MeshCol; // ������ �ڶ�� �� ȭ�� �ݶ��̴� ���� ��ȣ�ۿ� �ؼ� ������ Ű�� ����, ������ �ڶ� �Ŀ� ��������
    Image TakoPos_Leaf; // Ÿ�� ��ġ�� ���� ���ĸ�
    bool image_SetActive; // �̹��� �����ִ��� üũ�� �Ұ�
    Kiara_TreeBed treeBed;

    GameObject player;
    TextController textController;

    Interaction_Gimics interaction;
    

    public Material tree_Wood; // �����ٱ� ���׸���
    public Material tree_Leaf; // ���� ���ĸ� ���׸���
    Material[] mat;
    Material[] mat2; // renderer�� material�� ���õ� material[] �̰� �ٲ� ���������ν� ���� ����

    string Growthing_Text = "���� �ڶ󳪴� �� ������..\n���� Ŀ���ݾ�!"; // ���� �ڶ�� �����ϸ� ���� �ؽ�Ʈ
    string after_Growth_Text = "���ĸ��� ���� �����־�.\n��� �ڼ��� ���߰ھ�."; // ������ �Ŀ� ��ȣ�ۿ�� ���� �ؽ�Ʈ

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
        //interaction.run_Gimic = false; // �׽�Ʈ��!!!!!!!!!!!!!!! ���߿� ������!!!!!!!!!!!!!!!!!!!
        animator = gameObject.GetComponentInChildren<Animator>();

        SceneStartEffectSet();
        SetMaterials(); // �ڶ�� ������ ���׸��� ����
        //SceneStartSetting_StructureTree(); // ��ͼ��� ���ο� ���� �ʱ� ����
        StartCoroutine(WaitTouch());
    }


    void SceneStartEffectSet()
    {
        for(int i = 0; i < ParticleEffect.Length; i++)
        {
            ParticleEffect[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator GrowthTree() // ���� �ڶ��
    {
        StartCoroutine(textController.SendText(Growthing_Text));
        child_Bed_MeshCol.enabled = false;
        yield return new WaitForSeconds(1f);
        child_Tree.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        childTree_Renderer.materials = mat; // �ٱ⸸
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
        childTree_Renderer.materials = mat2; // �ٱ� + ��
        yield return new WaitForSeconds(1f);
        SceneStartEffectSet(); // ����Ʈ ��Ȱ��ȭ ��Ȱ���ϴ°�
        ParentTree_BoxCol.enabled = true;
        StartCoroutine(textController.SendText(after_Growth_Text));
    }

    IEnumerator WaitTouch() // ������ �� �ڶ� ���¿��� ��ȣ�ۿ� �� ���� �ڶ������ �ݶ��̴� �����ν� ��ȣ�ۿ� x
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
                    TakoPos_Leaf.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� ���ĸ� ����ֱ�
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
                    TakoPos_Leaf.gameObject.SetActive(true); // ��ȣ�ۿ� �� ��ġ���� ���ĸ� ����ֱ�
                    image_SetActive = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                }
            }
        }
    }
    public void CanvasClose() // x��ư�� ����
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
