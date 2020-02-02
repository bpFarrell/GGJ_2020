using UnityEngine;
using DG.Tweening;
namespace Items
{
    public class ItemProduce : ItemBase
    {
        float spawnTime;
        float lifeSpan = 20;
        public PlantType plantType;
        public GameObject progressRing;
        public Material ringMat;
        public bool isCurrentlyInteractable { get; }
        public void Interact(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

        public void EnterRange(PlayerController player)
        {
            throw new System.NotImplementedException();
        }

        public void LeaveRange(PlayerController player)
        {
            throw new System.NotImplementedException();
        }
        public void Start() {
            spawnTime = Time.time;
            MeshRenderer mr = progressRing.GetComponent<MeshRenderer>();
            ringMat = new Material(mr.material);
            mr.material = ringMat;
        }
        public void Update() {
            float t = (Time.time - spawnTime) / lifeSpan;
            progressRing.transform.eulerAngles = new Vector3(90, 0, 0);
            ringMat.SetFloat("_T", 1 - t - 1);
            if(spawnTime + lifeSpan < Time.time) {
                if(heldBy!=null)
                    heldBy.ThrowHeld(0.1f);
                transform.DOScale(0, 0.5f).onComplete = () => { FinishClear(); };
                //Destroy(this);
            }
        }
        public void FinishClear() {
            Destroy(gameObject);
        }
    }
}