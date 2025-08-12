<?php

namespace App\Models;

use App\Models\TruthTemplate;

class TruthTemplateRepository
{
    /**
     * 真相の総数を取得
     */
    public function getCount(): int
    {
        return TruthTemplate::count();
    }

    /**
     * IDから真相取得
     */
    public function findById(int $truthId)
    {
        return TruthTemplate::find($truthId);
    }

    /**
     * 全真相取得
     */
    public function getAll()
    {
        return TruthTemplate::all();
    }
}
