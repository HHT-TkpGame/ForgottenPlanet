<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Position;

class PositionController extends Controller
{
        //Postでプレイヤーの座標を取得・更新（探索側）
    public function updatePosition(Request $request, $roomId)
    {
        $validated = $request->validate([
            'player_id' => 'required|string',
            'x' => 'required|numeric',
            'y' => 'required|numeric',
            'z' => 'required|numeric',
	    'rot_y' => 'required|numeric',
        ]);

        $position = Position::updateOrCreate(
            ['room_id' => $roomId, 'player_id' => $validated['player_id']],
            ['x' => $validated['x'], 'y' => $validated['y'], 'z' => $validated['z'], 'rot_y' => $validated['rot_y']]
        );

        return response()->json(['status' => 'ok']);
    }

    // GET: 座標を取得（通信室側）
    public function getPosition($roomId)
    {
        $position = Position::where('room_id', $roomId)->first();
        if (!$position) {
            return response()->json(['error' => 'No position found'], 404);
        }
        return response()->json($position);
    }
}
