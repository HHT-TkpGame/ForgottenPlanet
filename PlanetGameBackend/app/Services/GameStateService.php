<?php

namespace App\Services;

use App\Models\GameStateRepository;

class GameStateService
{
    protected GameStateRepository $repository;

    public function __construct(GameStateRepository $repository)
    {
        $this->repository = $repository;
    }

    /**
     * ゲーム進行状態を初期化する
     */
    public function init(int $roomId)
    {
        $this->repository->init($roomId);
    }

    /**
     * ゲーム進行状態を1段階進める
     */
    public function updateProgressByRoomId(int $roomId)
    {
       return $this->repository->updateProgressByRoomId($roomId);
    }

    /**
     * 現在のゲーム進行状態を取得する
     */
    public function getProgressByRoomId(int $roomId)
    {
        return $this->repository->getProgressByRoomId($roomId);
    }
}
