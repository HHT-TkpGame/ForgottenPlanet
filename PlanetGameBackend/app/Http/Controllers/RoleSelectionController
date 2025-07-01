<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Services\RoleSelectionService;

class RoleSelectionController extends Controller
{
    protected $service;

    public function __construct(RoleSelectionService $service)
    {
        $this->service = $service;
    }

    /**
     * プレイヤーの役職とロック状態を保存・更新
     */
    public function updateSelection(Request $request, int $roomId)
    {
        $validated = $request->validate([
            'player_id' => 'required|string',
            'is_commander' => 'required|boolean',
            'is_locked' => 'required|boolean',
        ]);

        $result = $this->service->updateSelection(
            $validated['player_id'],
            $roomId,
            $validated['is_commander'],
            $validated['is_locked']
        );

        return response()->json($result);
    }

    /**
     * 再選択
     * 指定ルーム内の全てのプレイヤーのis_lockedをfalseにする
     */
    public function unlockAllInRoom($roomId)
    {
        $this->service->unlockAllByRoomId($roomId);
        return response()->json([
            'result' => 'ResetAllLockState'
        ]);
    }

    /**
     * 指定ルーム内のすべてのプレイヤーの状態を取得
     */
    public function getSelectionsInRoom($roomId)
    {
        $result = $this->service->getSelectionsByRoomId($roomId);
        return response()->json([
            'selections' => $result
        ]);
    }

    /**
     * 全員がロック済かつ役職が被っていないかチェック
     */
    public function checkConflictInRoom($roomId)
    {
        $hasConflict = $this->service->getHasConflict($roomId);

        return response()->json([
            'has_conflict' => $hasConflict
        ]);
    }
}