using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // Private
    private Rigidbody rb;
    private MyNetworkAudioManager audioManager;

    // Public
    [Header("Global")]
    public Animator anime;
    public MyNetworkAnimator weapAnim;
    public MyNetworkAnimator2 globalAnim;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float crouchSpeed = 2.5f;
    private float speed;
    private bool isMoving = false;

    [Header("Rotation")]
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;
    public float sensx = 3f;

    [Header("Jump")]
    public PlayerFoot playerFoot;

    [Header("Crouch")]
    private bool isCrouch = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioManager = GetComponent<MyNetworkAudioManager>();
        speed = walkSpeed;
    }

    public void Move (float _xM, float _zM)
    {
        if(_xM == 1f && _zM != 0f)
        {
            _xM = 0.75f;
        }else if(_xM == -1f && _zM != 0f)
        {
            _xM = -0.75f;
        }

        if(_zM == 1f && _xM != 0f)
        {
            _zM = 0.75f;
        }else if(_zM == -1f && _xM != 0f)
        {
            _zM = -0.75f;
        }
        //calcule des 2 composantes
        Vector3 _moveH = transform.right * _xM;
        Vector3 _moveV = transform.forward * _zM;
        //normalisation des 2 composantes en 1 V3D
        Vector3 _velocity = new Vector3(_xM, 0, _zM) * speed;
        // Animations
        anime.SetFloat("Forward", _zM);
        anime.SetFloat("Left", -_xM);
        // si un deplacement est capté
        if (_velocity != Vector3.zero)
        {
            //on déplace le joueur
            transform.Translate(_velocity * Time.deltaTime);
            if (!isMoving)
            {
                int clip = Random.Range(0, 3);
                audioManager.CmdPlay("Walk", clip);
                isMoving = true;
                weapAnim.CmdSetBool("walk", true);
            }
        }
        else
        {
            if (isMoving)
            {
                audioManager.CmdStop("Walk");
                isMoving = false;
                weapAnim.CmdSetBool("walk", false);
            }
        }
    }

    public void Rotate (float _yR)
    {
        Vector3 _rotation = new Vector3(0, _yR, 0) * sensx * 20;
        transform.Rotate(_rotation * Time.deltaTime);

        Quaternion rot = transform.rotation;

        rot = new Quaternion(Mathf.Clamp(rot.x, minimumVert, maximumVert), rot.y, rot.z, rot.w);

        transform.rotation = rot;
    }

    public void Jump ()
    {
        if(!isCrouch)
        {
            if (playerFoot.Jump())
            {
                audioManager.CmdPlay("Jump", 0);
                anime.SetTrigger("Jump");
                weapAnim.CmdSetTrigger("Jump");
            }
        }
    }

    public void Crouch ()
    {
        if (!isCrouch)
        {
            isCrouch = true;
            anime.SetBool("Crouch", true);
            weapAnim.CmdSetBool("Crouch", true);
            globalAnim.CmdSetBool("IsCrouch", true);
            speed = crouchSpeed;
        }
        else
        {
            isCrouch = false;
            anime.SetBool("Crouch", false);
            weapAnim.CmdSetBool("Crouch", false);
            globalAnim.CmdSetBool("IsCrouch", false);
            speed = walkSpeed;
        }
        audioManager.CmdPlay("Crouch", 0);
    }

    public void PlaySound (string name, int num)
    {
        audioManager.CmdPlay(name, num);
    }

    public void ChangeSensitivity(float sens)
    {
        sensx = sens;
    }
}
