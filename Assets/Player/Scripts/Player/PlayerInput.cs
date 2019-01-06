using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{

    private PlayerMovement playerMovement;
    private PlayerWeapon playerWeapon;
    private PlayerEmote playerEmote;

    private InputManager inputManager;

    public bool canMove = true;

    public bool playEmote = false;

    public PlayerAtributes playerAtributes;
    


    private void Start()
    {
        if (!isLocalPlayer) enabled = false;
        playerMovement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerEmote = GetComponent<PlayerEmote>();
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
            playerEmote.StopEmote();
        }
        if (Input.GetKey(inputManager.GetKeyCode("right")))
        {
            _xM += 1;
            playerEmote.StopEmote();
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
            playerEmote.StopEmote();
        }
        if (Input.GetKey(inputManager.GetKeyCode("forward")))
        {
            _zM += 1;
            playerEmote.StopEmote();
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
            playerEmote.StopEmote();
        }

        // Crouch
        if (Input.GetKeyDown(inputManager.GetKeyCode("crouch")))
        {
            playerMovement.Crouch();
            playerEmote.StopEmote();
        }

        // Shot
        if (Input.GetKey(inputManager.GetKeyCode("shot")))
        {
            playerWeapon.Shot();
            playerEmote.StopEmote();
        }

        // Cut
        if (Input.GetKeyDown(inputManager.GetKeyCode("cut")))
        {
            playerWeapon.Cut();
            playerEmote.StopEmote();
        }

        // Grenade
        if (Input.GetKeyDown(inputManager.GetKeyCode("grenade")))
        {
            playerWeapon.LaunchGrenade();
            playerEmote.StopEmote();
        }

        // Reload
        if (Input.GetKeyDown(inputManager.GetKeyCode("reload")))
        {
            playerWeapon.Reload();
            playerEmote.StopEmote();
        }

        // Aim
        if (Input.GetKeyDown(inputManager.GetKeyCode("aim")))
        {
            playerWeapon.Aim(true);
            playerEmote.StopEmote();
        } else if (Input.GetKeyUp(inputManager.GetKeyCode("aim")))
        {
            playerWeapon.Aim(false);
            playerEmote.StopEmote();
        }

        // Switch Weapon
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            playerWeapon.SwitchWeaponWheel(1);
            playerEmote.StopEmote();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            playerWeapon.SwitchWeaponWheel(-1);
            playerEmote.StopEmote();
        }

        if (Input.GetKeyDown(inputManager.GetKeyCode("weap1")))
        {
            playerWeapon.SwitchWeaponIndex(0);
            playerEmote.StopEmote();
        }

        if (Input.GetKeyDown(inputManager.GetKeyCode("weap2")))
        {
            playerWeapon.SwitchWeaponIndex(1);
            playerEmote.StopEmote();
        }

        // Emotes
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerAtributes.playerUI.emoteMenu.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            playerAtributes.playerUI.emoteMenu.SetActive(false);
        }
    }
}
