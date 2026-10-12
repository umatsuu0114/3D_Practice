using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false; // ドアが開いているかどうか

    [Header("ドアの設定")]
    public float openAngle = 90f;   // 開く角度
    public float speed = 3f;        // 開閉スピード（数字を大きくすると速くなります）

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // ゲーム開始時の角度（閉じた状態）を記録
        closedRotation = transform.rotation;
        // 開いた状態の目標角度を計算（Y軸にopenAngle分だけ回転）
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // 目標の角度を決める（開いているなら openRotation、閉じているなら closedRotation）
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        // 毎フレーム、現在の角度から目標の角度へゆっくり変化させる（Slerp）
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    // プレイヤーから呼び出される「ドア開閉」処理
    public void ToggleDoor()
    {
        isOpen = !isOpen; // フラグを切り替えるだけ！
    }
}