<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Position extends Model
{
// テーブル名（Laravelの命名規則に従っていれば不要だが、明示してもOK）
    protected $table = 'positions';

    // 複数代入を許可するカラム
    protected $fillable = [
        'room_id',
        'player_id',
        'x',
        'y',
        'z',
	'rot_y',
    ];

    // created_at, updated_at を使わない場合
    public $timestamps = false;
}
