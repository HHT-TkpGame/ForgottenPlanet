<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Position;

class PositionController extends Controller
{
        //Post�Ńv���C���[�̍��W���擾�E�X�V�i�T�����j
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

    // GET: ���W���擾�i�ʐM�����j
    public function getPosition($roomId)
    {
        $position = Position::where('room_id', $roomId)->first();
        if (!$position) {
            return response()->json(['error' => 'No position found'], 404);
        }
        return response()->json($position);
    }
}
