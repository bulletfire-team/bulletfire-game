using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{

    private PlayerMovement playerMovement;
    private PlayerWeapon playerWeapon;

    private InputManager inputManager;

    public bool canMove = true;
    


    private void Start()
    {
        if (!isLocalPlayer) enabled = false;
        playerMovement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (!canMove) return;
        // Move
        float _xM = 0;
        if (Input.GetKey(inputManager.GetKeyCode("left")))
        {
            _xM -= 1;
        }
        if (Input.GetKey(inputManager.GetKeyCode("right")))
        {
            _xM += 1;
        }
        /*
         * -1 gauche
         * 0 ne bouge pas
         * 1 droite
         */
        float _zM = 0;
        if (Input.GetKey(inputManager.GetKeyCode("backward")))
        {
            _zM -= 1;
        }
        if (Input.GetKey(inputManager.GetKeyCode("forward")))
        {
            _zM += 1;
        }
        /*
         * -1 recule
         * 0 ne bouge pas
         * 1 avance
         */
        playerMovement.Move(_xM, _zM);

        // Rotation
        float _yR = Input.GetAxisRaw("Mouse X");
        playerMovement.Rotate(_yR);

        // Jump
        if (Input.GetKeyDown(inputManager.GetKeyCode("jump")))
        {
            playerMovement.Jump();
        }

        // Crouch
        if (Input.GetKeyDown(inputManager.GetKeyCode("crouch")))
        {
            playerMovement.Crouch();
        }

        // Shot
        if (Input.GetKey(inputManager.GetKeyCode("shot")))
        {
            playerWeapon.Shot();
        }

        // Cut
        if (Input.GetKeyDown(inputManager.GetKeyCode("cut")))
        {
            playerWeapon.Cut();
        }

        // Grenade
        if (Input.GetKeyDown(inputManager.GetKeyCode("grenade")))
        {
            playerWeapon.LaunchGrenade();
        }

        // Reload
        if (Input.GetKeyDown(inputManager.GetKeyCode("reload")))
        {
            playerWeapon.Reload();
        }

        // Aim
        if (Input.GetKeyDown(inputManager.GetKeyCode("aim")))
        {
            playerWeapon.Aim(true);
        } else if (Input.GetKeyUp(inputManager.GetKeyCode("aim")))
        {
            playerWeapon.Aim(false);
        }

        // Switch Weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            playerWeapon.SwitchWeaponWheel(1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            playerWeapon.SwitchWeaponWheel(-1);
        }

        if (Input.GetKeyDown(inputManager.GetKeyCode("weap1")))
        {
            playerWeapon.SwitchWeaponIndex(0);
        }

        if (Input.GetKeyDown(inputManager.GetKeyCode("weap2")))
        {
            playerWeapon.SwitchWeaponIndex(1);
        }
    }
}
