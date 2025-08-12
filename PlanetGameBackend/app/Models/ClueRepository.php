<?php

namespace App\Models;

use App\Models\Clue;

class ClueRepository
{
    /**
     * レコードを登録
     */
    public function create(array $data)
    {
        return Clue::create($data);
    }
    /**
     * roomIdに対応する登録された手がかりを全件取得
     */
    public function findByRoomId($roomId)
    {
        return Clue::where('room_id', $roomId)->get();
    }
}
