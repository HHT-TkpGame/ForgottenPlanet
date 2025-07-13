<?php

namespace App\Models;

use App\Models\GameState;
use App\Enums\GameProgress;
use Exception;

use function PHPUnit\Framework\isNull;

class GameStateRepository
{
    public function init($roomId){
        GameState::updateOrCreate([
            'room_id' => $roomId,
            'game_progress' => GameProgress::Select,
        ]);
    }
    public function updateProgressByRoomId($roomId){
        $gameState = GameState::where('room_id', $roomId)->first();
        if(is_null($gameState)){
            throw new \Exception("Game state not found for room_id: $roomId");
        }
        $nextProgress = $gameState->game_progress->value + 1;
        $gameState->update(['game_progress' => $nextProgress]);
        return $gameState->game_progress->value;
    }
    
    public function getProgressByRoomId($roomId){
        $gameState = GameState::where('room_id', $roomId)->first();
        return $gameState->game_progress->value;
    }
}