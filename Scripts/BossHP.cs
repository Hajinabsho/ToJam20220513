using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public int maxHealth;
    public HealthBar healthbar;
    private int curHealth;
    Rigidbody rb;
    private bool hit;
    static private bool bossDead;


    public Material defaultMat;
    public Material flashMat;

    SkinnedMeshRenderer Mesh;

    public bool getbossDead()
    {
        if (curHealth <= 0)
        {
            bossDead = true;
        }

        else if (curHealth > 0)
        {
            bossDead = false; 
        }

        return bossDead;
    }

    public bool getHit()
    {
        return hit;
    }

    public void setHit(bool a)
    {
        hit = a;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        healthbar.UpdateHealth((float)curHealth / (float)maxHealth);

        SoundManagerScript.PlaySound("Damaged");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(1);
            //Debug.Log("Hit" + curHealth);
            
            Mesh.material = flashMat;

            Invoke("returnMesh", 0.2f);

            //boss.hit = false;
            Debug.Log("Hit for Mesh");
        }
    }

    void returnMesh()
    {
        Mesh.material = defaultMat;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Mesh = GetComponent<SkinnedMeshRenderer>();

        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
