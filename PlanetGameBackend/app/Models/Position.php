<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Position extends Model
{
// �e�[�u�����iLaravel�̖����K���ɏ]���Ă���Εs�v�����A�������Ă�OK�j
    protected $table = 'positions';

    // ���������������J����
    protected $fillable = [
        'room_id',
        'player_id',
        'x',
        'y',
        'z',
	'rot_y',
    ];

    // created_at, updated_at ���g��Ȃ��ꍇ
    public $timestamps = false;
}
