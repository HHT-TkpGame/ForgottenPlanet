<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class ChatMessage extends Model
{
    protected $fillable = [
	'room_id',
	'player_id',
	'message',
	'sent_at',
    ];
    //created_at,update_atŽg‚í‚È‚¢‚Ì‚Åfalse
    public $timestamps = false;
}
