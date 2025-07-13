<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\ChatMessage;

class ChatMessageController extends Controller
{
    //チャットを送る
    public function store(Request $request, $roomId){
	$max_message_length = 30;
	$validated = $request->validate([
	    'player_id' => 'required|string',
	    'message' => 'required|string|max:'.$max_message_length,
	]);

	$chat = ChatMessage::create([
	    'room_id' => $roomId,
	    'player_id' => $validated['player_id'],
	    'message' => $validated['message'],
	    'sent_at' => now(),
	]);
	return response()->json(['status'=>'success', 'chat => $chat,']);
    }

    //チャットを取得する
    //sinceに入っている時間以前のチャットは拾わない
    public function fetch(Request $request, $roomId){
	    $validated = $request->validate([
            'since'     => 'nullable|date',
            'player_id' => 'required|string',
        ]);
        $query = ChatMessage::where('room_id', $roomId);
        if ($validated['since']) {
            $query->where('sent_at', '>', $validated['since']);
        }

	//sent_atを昇順でソート
	$messages = $query->orderBy('sent_at', 'asc')->get([
            'player_id', 'message', 'sent_at',
        ]);

        return response()->json(['messages' => $messages]);
    }
}
