using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Select : MonoBehaviour
{
    GameManager gameManager;
    public GameObject creat;	// �÷��̾� �г��� �Է�UI
    public Text[] slotText;		// ���Թ�ư �Ʒ��� �����ϴ� Text��
    public Text[] scoreText;
    public Text newPlayerName;	// ���� �Էµ� �÷��̾��� �г���

    bool[] savefile = new bool[3];	// ���̺����� �������� ����

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // ���Ժ��� ����� �����Ͱ� �����ϴ��� �Ǵ�.
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}"))	// �����Ͱ� �ִ� ���
            {
                savefile[i] = true;			// �ش� ���� ��ȣ�� bool�迭 true�� ��ȯ
                DataManager.instance.nowSlot = i;	// ������ ���� ��ȣ ����
                DataManager.instance.LoadData();	// �ش� ���� ������ �ҷ���
                slotText[i].text = DataManager.instance.nowPlayer.name;	// ��ư�� �г��� ǥ��
                scoreText[i].text = "�ְ� ���� :" + DataManager.instance.nowPlayer.score.ToString();
            }
            else	// �����Ͱ� ���� ���
            {
                slotText[i].text = "�������";
            }
        }
        // �ҷ��� �����͸� �ʱ�ȭ��Ŵ.(��ư�� �г����� ǥ���ϱ������̾��� ����)
        DataManager.instance.DataClear();
    }

    public void Slot(int number)	// ������ ��� ����
    {
        DataManager.instance.nowSlot = number;	// ������ ��ȣ�� ���Թ�ȣ�� �Է���.

        if (savefile[number])	// bool �迭���� ���� ���Թ�ȣ�� true��� = ������ �����Ѵٴ� ��
        {
            DataManager.instance.LoadData();	// �����͸� �ε��ϰ�
            GoGame();	// ���Ӿ����� �̵�
        }
        else	// bool �迭���� ���� ���Թ�ȣ�� false��� �����Ͱ� ���ٴ� ��
        {
            Creat();	// �÷��̾� �г��� �Է� UI Ȱ��ȭ
        }
    }

    public void Creat()	// �÷��̾� �г��� �Է� UI�� Ȱ��ȭ�ϴ� �޼ҵ�
    {
        creat.gameObject.SetActive(true);
    }

    public void GoGame()	// ���Ӿ����� �̵�
    {
        if (!savefile[DataManager.instance.nowSlot])	// ���� ���Թ�ȣ�� �����Ͱ� ���ٸ�
        {
            DataManager.instance.nowPlayer.name = newPlayerName.text; // �Է��� �̸��� �����ؿ�
            DataManager.instance.SaveData(); // ���� ������ ������.
        }
        gameManager.GameStart();
    }
}