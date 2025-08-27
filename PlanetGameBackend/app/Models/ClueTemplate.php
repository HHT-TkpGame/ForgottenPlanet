<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class ClueTemplate extends Model
{
    protected $table = 'clue_templates_tbl';
    protected $primaryKey = ['truth_id', 'clue_id'];
    public $incrementing = false;
    protected $fillable = [
        'truth_id',
        'clue_id',
        'title',
        'description',
    ];

    /**
     * 親の真相テンプレート
     */
    public function truth()
    {
        return $this->belongsTo(TruthTemplate::class, 'truth_id', 'truth_id');
    }
}
