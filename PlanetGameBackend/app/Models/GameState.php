<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use App\Enums\GameProgress;

class GameState extends Model
{
    protected $table = 'game_states_tbl';
    protected $fillable = ['room_id', 'game_progress'];

    protected $casts = [
        'game_progress' => GameProgress::class,
    ];

    public function room()
    {
        return $this->belongsTo(Room::class);
    }
}
