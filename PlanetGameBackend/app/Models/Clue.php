<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Clue extends Model
{
    protected $table = 'clues_tbl';

    protected $fillable = [
        'room_id',
        'truth_id',
        'clue_id',
        'is_shared',
    ];
}
