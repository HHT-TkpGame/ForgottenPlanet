<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class TruthTemplate extends Model
{
    protected $table = 'truth_templates_tbl';
    protected $primaryKey = 'truth_id'; 
    protected $keyType = 'int';

    protected $fillable = [
        'truth_id',
        'name',
        'description',
    ];

    /**
     * 関連する手がかりテンプレート
     */
    public function clues()
    {
        return $this->hasMany(ClueTemplate::class, 'truth_id', 'truth_id');
    }
}
