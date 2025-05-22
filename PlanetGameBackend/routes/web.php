<?php
use Illuminate\Support\Facades\Route;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

Route::get('/api/testweb', function () {
    return response()->json(['message' => 'from web']);
});
Route::get('/debug/path', function (\Illuminate\Http\Request $request) {
    dd([
        'full_url' => $request->fullUrl(),
        'path' => $request->path(),
        'uri' => $request->getRequestUri(),
    ]);
});

//match method
Route::post('/match', function (Request $request) {
    $keyword = $request->input('keyword');
    $playerId = $request->input('player_id');

    if (!$keyword || !$playerId) {
        return response()->json(['status' => 'error', 'message' => 'Missing data'], 400);
    }

    $keywordHash = md5($keyword);

    //search room
    $room = DB::table('rooms')->where('keyword', $keywordHash)->first();

    //room not found
    if (!$room) {
        $roomCount = DB::table('rooms')->count();
        if ($roomCount >= 10) {
            return response()->json(['status' => 'error', 'message' => 'server is full'], 403);
        }
        //create room
        $roomId = DB::table('rooms')->insertGetId([
            'keyword' => $keywordHash,
            'created_at' => now(),
        ]);

        //entry player
        DB::table('players')->insert([
            'room_id' => $roomId,
            'player_id' => $playerId,
        ]);

        return response()->json([
            'status' => 'created',
            'room_id' => $roomId,
            'player_id' => $playerId,
            'player_count' => 1,
        ]);
    }
    //room found 
    else {
        $roomId = $room->id;

        $count = DB::table('players')->where('room_id', $roomId)->count();
        if ($count >= 2) {
            return response()->json(['status' => 'error', 'message' => 'room is full'], 403);
        }
        //Join as a guest
        DB::table('players')->insert([
            'room_id' => $roomId,
            'player_id' => $playerId,
        ]);

        return response()->json([
            'status' => 'joined',
            'room_id' => $roomId,
            'player_id' => $playerId,
            'player_count' => $count + 1,
        ]);
    }
});


?>
