using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public float missleAttackRate = 1f;
    public int columns = 11;
    public Projectile misslePrefab;
    public AnimationCurve speed;
    public int amountAlive => this.totalInvaders - this.amountKilled;
    private Vector3 _direction = Vector2.right;
    public int amountKilled {get; private set;}
    public int totalInvaders => this.rows * this.columns;
    public float precentKilled => this.amountKilled / this.totalInvaders;


    private async void Awake() {
        for(int row = 0; row < this.rows; row++){
            float width = this.columns - 1;
            float height = this.rows - 1;
            Vector2 centering = new Vector2(-width / 2 , -height / 4);
            Vector3 rowPosition =  new Vector3(centering.x, centering.y + (row * 0.5f), 0.0f);
            for(int col = 0; col < this.columns; col++){
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * 1f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start() {
        InvokeRepeating(nameof(Missle), this.missleAttackRate, this.missleAttackRate);
    }
    private void Update() {
        this.transform.position += _direction * this.speed.Evaluate(this.precentKilled) * Time.deltaTime;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach(Transform invader in this.transform){

            if(!invader.gameObject.activeInHierarchy){
                continue;
            }

            if(_direction == Vector3.right && invader.position.x >= (rightEdge.x - 0.5f)){
                AdvanceRow();
            }else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 0.5f)){
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow(){
        _direction.x *= -1.0f;
        Vector3 position = this.transform.position;
        position.y -= 0.5f;
        this.transform.position = position;
    }

    private void Missle(){
        foreach(Transform invader in this.transform){
            if(!invader.gameObject.activeInHierarchy){
                continue;
            }
            if(Random.value < (2.0f / (float)this.amountAlive)){
                Instantiate(this.misslePrefab, invader.position, Quaternion.identity);
                break;
            }
        }

    }
   private void InvaderKilled(){
       amountKilled++;

       if(this.amountKilled >= this.totalInvaders){
           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       }
   }
}
