<?php
namespace App\Models;

use App\Models\Player;

class PlayerRepository
{
    public function getPlayersInRoom(int $roomId)
    {
        return Player::where('room_id', $roomId)->get();
    }

    /**
     * 指定されたルームに所属するプレイヤー数を取得
     *
     * @param int $roomId ルームのID
     * @return int プレイヤーの人数
     */
    public function countPlayersInRoom(int $roomId): int
    {
        return Player::where('room_id', $roomId)->count();
    }

    /**
     * プレイヤー情報を新規作成
     *
     * @param array $data プレイヤー情報（'room_id', 'player_id', 'is_host', 'is_commander' など）
     * @return Player 作成されたプレイヤーモデル
     */
    public function create(array $data)
    {
        return Player::create($data);
    }
}
