<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Room extends Model
{
    protected $table = 'rooms';
    public function players()
    {
	//roomsの中に複数のplayersがいるという関係を定義
	return $this->hasMany(Player::class);
    }

}