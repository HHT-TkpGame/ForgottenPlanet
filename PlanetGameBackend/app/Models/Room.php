<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Room extends Model
{
    protected $table = 'rooms';
}

public function players()
{
    //rooms�̒��ɕ�����players������Ƃ����֌W���`
    return $this->hasMany(Player::class);
}
