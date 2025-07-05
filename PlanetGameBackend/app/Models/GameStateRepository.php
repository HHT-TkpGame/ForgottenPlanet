<?php

namespace App\Models;

use App\Models\GameState;

class GameStateRepository
{
    public function incrementGameProgressByRoomId($roomId){
        $gameState = GameState::where('room_id', $roomId)->first();
        $nextProgress = $gameState->game_progress->value + 1;
        $gameState->update(['game_progress' => GameState::from($nextProgress)]);
    }
}