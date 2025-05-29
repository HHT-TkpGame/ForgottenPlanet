<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Room;
use App\Models\Player;

class PlayerController extends Controller
{
    //クライアントの最後に通信された時間を更新するメソッド（クライアントから定期的に呼び出す）
    public function heartbeat(Request $request){
    $validated = $request->validate([
	    'room_id' => 'required|integer',
	    'player_id' => 'required|string',
	]);
	
	//Roomの存在チェック
	if (!Room::find($validated['room_id'])) {
            return response()->json(['error' => 'Room not found'], 404);
    	}

	$player = Player::updateOrCreate(
	    ['room_id'=>$validated['room_id'],'player_id'=> $validated['player_id']],
	    ['last_request_time'=>now(),]
	);		

	return response()->json(['status'=>'heartbeat updated']);
    }

}
