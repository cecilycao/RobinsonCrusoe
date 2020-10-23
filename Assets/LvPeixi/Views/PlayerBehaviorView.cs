using UniRx;
using UnityEngine;

public class PlayerBehaviorView : MonoBehaviour
{
    Rigidbody rigid;
    PlayerMovementPresenter movement;
    Animator anim;
    [SerializeField]
    MeshRenderer meshRenderer;
    public Texture2D[] texture2Ds;
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    bool isWalk;

    private void Awake()
    {
        movement = GetComponent<PlayerMovementPresenter>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitMeshRender();

        IsWalking();

        IsIdling();

        OnVelocityChanged();

        UpdateTexture();

        UpdateRigidbodyWhenWalking();

        UpdateAnimator();
    }

    private void Update()
    {
       
    }
    float FilterVelocityData(float axisSpeed)
    {
        if (axisSpeed>0)
        {
            return 1;
        }
        else if (axisSpeed<0)
        {
            return -1;
        }
        return 0;
    }

    void InitMeshRender()
    {
        meshRenderer.material.SetTexture("_BaseMap", texture2Ds[0]);
    }

    void UpdateRigidbodyWhenWalking()
    {
        Observable.EveryUpdate()
         .Subscribe(x =>
         {
             var _velocity = new Vector3(movement.Velocity.Value.x, rigid.velocity.y, movement.Velocity.Value.z);
             rigid.velocity = -_velocity;
         });
    }
    void UpdateAnimator()
    {
        Observable.EveryUpdate()
            .Subscribe(x =>
            {
                anim.SetBool("IsWalk", isWalk);
            });
    }
    void UpdateTexture()
    {

        Observable.EveryUpdate()
            .Subscribe(x =>
            {
                var dir_x = FilterVelocityData(movement.Velocity.Value.x);
                var dir_z = FilterVelocityData(movement.Velocity.Value.z);

                direction.x = dir_x;
                direction.z = dir_z;

                if (dir_x>0)
                {
                    SetMaterialTex(1);
                }

                if (dir_x<0)
                {
                    SetMaterialTex(2);
                }
                if (dir_x == 0 && dir_z > 0)
                {
                    SetMaterialTex(0);
                }
                if (dir_x ==0 && dir_z < 0)
                {
                    SetMaterialTex(3);
                }
            });
    }
    void IsWalking()
    {
        Observable.EveryUpdate()
            .Where(x => isWalk)
            .Subscribe(x =>
            {
                UpdateAnimator();
            });
    }
    void IsIdling()
    {
        Observable.EveryUpdate()
            .Where(x => !isWalk)
            .Subscribe(x =>
            {

            });
    }
    void OnVelocityChanged()
    {
        movement.Velocity
            .Subscribe(x =>
            {
                isWalk = x.sqrMagnitude > 0.1f;
                UpdateAnimator();         
            });
    }  
    void SetMaterialTex(int textNum)
    {
        meshRenderer.material.SetTexture("_BaseMap", texture2Ds[textNum]);
    }
}
