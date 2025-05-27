<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Room extends Model
{
    protected $table = 'rooms';
}

public function players()
{
    //rooms‚Ì’†‚É•¡”‚Ìplayers‚ª‚¢‚é‚Æ‚¢‚¤ŠÖŒW‚ğ’è‹`
    return $this->hasMany(Player::class);
}
