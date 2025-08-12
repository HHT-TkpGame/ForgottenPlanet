<?php

namespace App\Models;

use App\Models\ClueTemplate;

class ClueTemplateRepository
{
    /**
     * 真相IDに属する手がかりの個数を取得
     */
    public function getCountByTruthId(int $truthId)
    {
        return ClueTemplate::where('truth_id', $truthId)
            ->orderBy('clue_id')
            ->count();
    }

    /**
     * 真相IDと範囲指定で手がかり取得
     */
    public function getByTruthIdAndRange(int $truthId, int $start, int $end)
    {
        return ClueTemplate::where('truth_id', $truthId)
            ->whereBetween('clue_id', [$start, $end])
            ->orderBy('clue_id')
            ->get();
    }
}
