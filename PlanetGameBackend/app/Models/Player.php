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

    // Room�Ƃ̃����[�V�����i1��Room�ɑ�����j���`
    public function room()
    {
        return $this->belongsTo(Room::class);
    }
}
