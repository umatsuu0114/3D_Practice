using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // 移動スピード
    public float mouseSensitivity = 2.0f; // マウスの感度
    public float jumpForce = 5.0f; // ジャンプ力
    public float pushForce = 10.0f; // 物体を押し飛ばす力

    private float xRotation = 0f;
    private Transform cameraTransform;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- ① マウス視点移動（右クリックを押している間） ---
        if (Input.GetMouseButton(1)) // 1は「右クリック」
        {
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }


        // --- ② キーボード移動（W/A/S/D） ---
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);


        // --- ③ ジャンプ処理（スペースキー） ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }


        // --- ④ 物体を押し飛ばす・仕掛けを触る処理（左クリック） ---
        if (Input.GetMouseButtonDown(0)) // 0は「左クリック」
        {
            // 画面中央（カメラの正面）に向かって目に見えない光線を飛ばす
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hit;

            // 5メートル以内の距離にある物体に当たったら
            if (Physics.Raycast(ray, out hit, 5.0f))
            {
                // ドア (Door) コンポーネントが付いているか確認
                Door door = hit.collider.GetComponent<Door>();
                if (door != null)
                {
                    door.ToggleDoor(); // ドアの開閉処理を呼び出す
                    return; // ドアを開けた場合はここで処理を終える
                }

                // 当たった対象に Rigidbody（物理挙動）が付いていれば力を加える
                Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    targetRb.AddForce(cameraTransform.forward * pushForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}