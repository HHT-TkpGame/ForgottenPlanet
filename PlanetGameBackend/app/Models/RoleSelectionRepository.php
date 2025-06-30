<?php

namespace App\Models;

use App\Models\RoleSelection;

class RoleSelectionRepository
{
    /**
    * プレイヤーのレコードを新規作成 or 更新
    * @param string プレイヤーのID
    * @param int ルームのID
    * @param array is_commanderやis_lockedなどの状態
    * @return 
    */
    public function createOrUpdate(string $playerId, int $roomId, array $data)
    {
        return RoleSelection::updateOrCreate(
            ['player_id' => $playerId, 'room_id' => $roomId],
            $data
        );
    }


    public function findByRoomId(int $roomId)
    {
        return RoleSelection::where('room_id', $roomId)->get();
    }

    public function findByPlayer(string $playerId, int $roomId)
    {
        return RoleSelection::where('player_id', $playerId)
                            ->where('room_id', $roomId)
                            ->first();
    }

    /**
     * 全プレイヤーがキャラ選択をしているか
     * @param int ルームのID
     */
    public function isAllLockedInRoom(int $roomId)
    {
        return RoleSelection::where('room_id', $roomId)
            ->where('is_locked', false)
            ->count() === 0;
    }

    /**
     * 役職が被っていないかチェック
     * @param int ルームのID
     */
    public function hasRoleConflict(int $roomId)
    {
        $roles = RoleSelection::where('room_id', $roomId)
            ->pluck('is_commander');

        return $roles->count() !== $roles->unique()->count();
    }
}
