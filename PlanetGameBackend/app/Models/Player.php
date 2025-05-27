<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Player extends Model
{
    protected $table = 'players';

    protected $fillable = [
        'room_id',
        'player_id',
        'is_host',
        'is_commander',
        'last_request_time',
    ];

    // Roomとのリレーション（1つのRoomに属する）を定義
    public function room()
    {
        return $this->belongsTo(Room::class);
    }
}
