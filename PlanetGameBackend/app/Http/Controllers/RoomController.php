<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Room;

class RoomController extends Controller
{
    //roomIdに対応するカラムを削除する
    public function deleteRoom($roomId){
		$room = Room::find($roomId);
		if(!$room){
			return response()->json(['error' => 'Room not found'], 404);	
		}
		//カスケード削除を設定しているので、
		//ルームに紐づいているplayersとpositionsのカラムも一緒に削除される	
		$room->delete();

		return response()->json(['status' => 'deleted']);
    }
}
