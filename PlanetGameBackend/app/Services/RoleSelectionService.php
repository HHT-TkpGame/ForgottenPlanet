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
     * 指定ルームのすべてのプレイヤーの状態を取得する
     */
    public function getSelectionsByRoomId(int $roomId)
    {
        return $this->repository->findByRoomId($roomId);
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
        $hasConflict = true;
        if($isAllLocked){
            $hasConflict = $this->repository->hasRoleConflict($roomId);
        }
        return $hasConflict;
    }
}
