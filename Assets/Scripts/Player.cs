using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 0.5f;
    private bool isActive;
    private void Update() {
        if(Input.GetKey(KeyCode.LeftArrow)){
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.RightArrow)){
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            Shoot();
        }
    }

    private void Shoot(){
        if(!isActive){
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestoryed;
            isActive = true;
        }
        
    }

    private void LaserDestoryed(){
        isActive = false;
    }   

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missle")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

