<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class RoleSelection extends Model
{
    protected $table = 'role_selections_tbl';

    protected $fillable = [
        'player_id',
        'room_id',
        'is_commander',
        'is_locked',
        'has_conflict',
    ];

    public function player()
    {
        return $this->belongsTo(Player::class, 'player_id', 'player_id');
    }
    // Roomとのリレーション
    public function room()
    {
        return $this->belongsTo(Room::class);
    }
}
