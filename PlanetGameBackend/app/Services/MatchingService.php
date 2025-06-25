<?php

namespace App\Services;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class MatchingService
{
    public function handleMatch(Request $request)
    {
        $keyword = $request->input('keyword');
	$playerId = $request->input('player_id');
	if (!$keyword || !$playerId) {
            return response()->json(['status' => 'error', 'message' => 'Missing data'], 400);
	}
        $keywordHash = md5($keyword);

        //ポストされた合言葉に対応した部屋を探す
        $room = DB::table('rooms')->where('keyword', $keywordHash)->first();

        //部屋がなかった場合
        if (!$room) {
            $roomCount = DB::table('rooms')->count();
            //10部屋以上あったら作成しない
            if ($roomCount >= 10) {
                return response()->json(['status' => 'error', 'message' => 'server is full'], 403);
            }
            //部屋作成
            $roomId = DB::table('rooms')->insertGetId([
                'keyword' => $keywordHash,
                'created_at' => now(),
            ]);
	        $isHost = true;
	        //役職をランダムで決定
	        $isCommander = (bool)random_int(0, 1);
            //プレイヤーの情報設定
            DB::table('players')->insert([
                'room_id' => $roomId,
	            'is_host'=>$isHost,
                'player_id' => $playerId,
	            'is_commander' => $isCommander,
            ]);

            return response()->json([
                'status' => 'created',
                'room_id' => $roomId,
	            'is_host'=>$isHost,
                'player_id' => $playerId,
                'is_commander' => $isCommander,
                'player_count' => 1,
            ]);
        }
        //ポストされた合言葉に対応した部屋があった場合
        else {
            $roomId = $room->id;

        //部屋が満員だったら参加できない
        $count = DB::table('players')->where('room_id', $roomId)->count();
        if ($count >= 2) {
            return response()->json(['status' => 'error', 'message' => 'room is full'], 403);
        }

        $isHost = false;
        //他プレイヤーの役職を取得する
        $players = DB::table('players')->where('room_id', $roomId)->get();
        // もし誰もいなかった場合未定義エラーにならないように対策。
        //$playersが空だった場合最初の人なので司令官とする
        if ($players->isEmpty()) {
            $isCommander = true;
        } else {
            //被らないように反転
            $existRole = $players[0]->is_commander;
            $isCommander = !$existRole;
        }
        //部屋に参加
        DB::table('players')->insert([
            'room_id' => $roomId,
	        'is_host'=>$isHost,
            'player_id' => $playerId,
	        'is_commander' => $isCommander,
        ]);

        return response()->json([
            'status' => 'joined',
            'room_id' => $roomId,
	        'is_host'=>$isHost,
            'player_id' => $playerId,
	        'is_commander' => $isCommander,
            'player_count' => $count + 1,
        ]);
        }
    }
}
