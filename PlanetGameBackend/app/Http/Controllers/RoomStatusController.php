<?php

namespace App\Http\Controllers;

use Illuminate\Support\Facades\DB;
use Illuminate\Http\Request;


class RoomStatusController extends Controller
{
    public function getPlayerCountInRoom($roomId){
	$playerCount = DB::table('players')->where('room_id', $roomId)->count();
	return response()->json(['player_count' => $playerCount]);
         }
}
