<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Answer extends Model
{
    // テーブル名
    protected $table = 'answers_tbl';

    protected $primaryKey = 'id';

    protected $fillable = [
        'answer_id',
        'room_id',
    ];

    /**
     * Roomリレーション
     */
    public function room()
    {
        return $this->belongsTo(Room::class, 'room_id');
    }
}
