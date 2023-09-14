using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range, Bow};
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
        else if (type == Type.Bow && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shoot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f); //0.1�� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
        yield return null; //1������ ���
    }
    //Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    //Use() ���η�ƾ + Swing() �ڷ�ƾ(Co-Op) 
    IEnumerator Shot()
    {
        //#1. �Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = intantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 80;
        yield return null;
        
        //#2. ź�� ����
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRb = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(-3, -2);
        caseRb.AddForce(caseVec, ForceMode.Impulse);
        caseRb.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }

    IEnumerator Shoot()
    {
        //#1. ȭ�� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = intantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;
        yield return null;
    }
}
