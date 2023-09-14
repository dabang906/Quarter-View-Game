using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZone;

    
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public GameObject menuSet;

    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Text hasDaggerTxt;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weapon4Img;
    public Image weaponRImgGre;
    public Image weaponRImgDag;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;
    public Text curScoreText;
    public Text bestText;
    DataManager dm;

    public AudioSource gameAudio;
    public AudioSource stageAudio;

    void Awake()
    {
        stageAudio.Stop();
        dm = FindObjectOfType<DataManager>();
        
        enemyList = new List<int>();
        //maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if (PlayerPrefs.HasKey("MaxScore")) PlayerPrefs.SetInt("MaxScore", 0);
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    public void StageStart()
    {
        gameAudio.Stop();
        stageAudio.Play();
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZone.SetActive(false);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        stageAudio.Stop();
        gameAudio.Play();

        player.transform.position = Vector3.up * 0.8f;

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZone.SetActive(true);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        player.stage++;
        DataManager.instance.nowPlayer.stage = player.stage ;
        DataManager.instance.nowPlayer.hasGrenade = player.hasGrenades;
        for(int i = 0; i < player.hasWeapons.Length; i++)
        {
            DataManager.instance.nowPlayer.hasWeapon[i] = player.hasWeapons[i];
        }
        DataManager.instance.nowPlayer.equipItem = player.equipItem;
        dm.SaveData();
        print("save�Ǿ����ϴ�." + player.stage);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true );
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator InBattle()
    {
        if (player.stage % 5 == 0)
        {
            enemyCntD++;
            print(enemyCntD);
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for (int index = 0; index < player.stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                }
            }
            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }
        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(isBattle)
        {
            playTime += Time.deltaTime;
        }
    }

    public void Resume()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        menuSet.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameExit()
    {
        dm.SaveData();
        Application.Quit();
    }

    void LateUpdate()
    {
        DataManager.instance.nowPlayer.stage = player.stage;
        DataManager.instance.nowPlayer.score = player.score;
        DataManager.instance.nowPlayer.health = player.health;
        DataManager.instance.nowPlayer.ammo = player.ammo;
        DataManager.instance.nowPlayer.coin = player.coin;
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "Stage" + player.stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60); 
        int second =(int)(playTime  % 60);

        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" +string.Format("{0:00}", second);

        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
        {
            playerAmmoTxt.text = " - / " + player.ammo;
        }
        else if (player.equipWeapon.type == Weapon.Type.Melee)
        {
            playerAmmoTxt.text = " - / " + player.ammo;
        }
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weapon4Img.color = new Color(1, 1, 1, player.hasWeapons[3] ? 1 : 0);
        weaponRImgGre.color = new Color(1, 1, 1, player.hasGrenades > 0 && player.equipItem == 0 ? 1 : 0);
        weaponRImgDag.color = new Color(1, 1, 1, player.equipItem == 1 ? 1 : 0);
        if (player.equipItem == 1)
        {
            hasDaggerTxt.gameObject.SetActive(true);
            hasDaggerTxt.text = player.hasDagger.ToString();
        }
        else hasDaggerTxt.gameObject.SetActive(false);

        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();

        //���� ü�� UI
        if (boss != null )
        {
            if(boss.curHealth <= 0)
            {
                bossHealthBar.localScale = Vector3.zero;
            }
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200;
        }

    }
}
