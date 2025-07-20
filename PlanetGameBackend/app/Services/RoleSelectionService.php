<?php

namespace App\Services;

use App\Models\RoleSelectionRepository;

class RoleSelectionService
{
    protected $repository;

    public function __construct(RoleSelectionRepository $repository)
    {
        $this->repository = $repository;
    }

    /**
     * プレイヤーの役職選択状態を保存または更新する
     */
    public function updateSelection(string $playerId, int $roomId, bool $isCommander, bool $isLocked)
    {
        return $this->repository->createOrUpdate($playerId, $roomId, [
            'is_commander' => $isCommander,
            'is_locked' => $isLocked,
        ]);
    }
    /**
     * プレイヤーの役職を更新する
     */
    public function updateRole(string $playerId, int $roomId, bool $isCommander)
    {
        return $this->repository->createOrUpdate($playerId, $roomId, [
            'is_commander' => $isCommander
        ]);
    }

    /**
     * 指定ルームのすべてのプレイヤーの状態を取得する
     */
    public function getSelectionsByRoomId(int $roomId)
    {
        return $this->repository->findByRoomId($roomId);
    }

    /**
     * 指定ルーム内のすべてのプレイヤーの is_locked を false に更新
     *
     * @param int $roomId
     * @return int 更新されたレコード数
     */
    public function unlockAllByRoomId($roomId)
    {
        return $this->repository->unlockAllByRoomId($roomId);
    }

    /**
     * 全員がロック済みかどうか
     */
    public function allLocked(int $roomId)
    {
        return $this->repository->isAllLockedInRoom($roomId);
    }

    /**
     * 役職が被っていないかどうか
     */
    public function getHasConflict(int $roomId)
    {
        $isAllLocked = $this->allLocked($roomId);
        //全員がロックしてなければ衝突扱い
        //Unity側で両方ロックしてないと呼べないが一応
        if(!$isAllLocked){ return true; }
        
        $hasConflict = $this->repository->hasRoleConflict($roomId);
        if($hasConflict){
            //役職被ってたら選びなおし
            $this->repository->unlockAllByRoomId($roomId);
        }
        
        return $hasConflict;
    }
}
