<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Services\GameStateService;

class GameStateController extends Controller
{
    protected GameStateService $service;

    public function __construct(GameStateService $service)
    {
        $this->service = $service;
    }

    /**
     * ゲーム進行状態の取得
     */
    public function getProgress(int $roomId)
    {
        $progress = $this->service->getProgressByRoomId($roomId);
        return response()->json(['game_progress' => $progress]);
    }

    /**
     * ゲーム進行状態を次へ進める
     */
    public function updateProgress(int $roomId)
    {
        $progress = $this->service->updateProgressByRoomId($roomId);
        return response()->json(['game_progress' => $progress]);
    }
}
