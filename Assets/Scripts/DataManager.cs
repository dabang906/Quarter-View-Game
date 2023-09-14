using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    #region Json��������

    //�̸�, ����, ����, �������� ����
    public string name;
    public int ammo;
    public int coin;
    public float health;
    public int score;
    public int stage;
    public bool[] hasWeapon = new bool[4];
    public int hasGrenade;
    public int hasDagger;
    public int equipItem;

    #endregion
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance; // �̱�������

    public PlayerData nowPlayer = new PlayerData(); // �÷��̾� ������ ����

    public string path; // ���
    public int nowSlot; // ���� ���Թ�ȣ

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/save";	// ��� ����
        print(path);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }

    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }
}