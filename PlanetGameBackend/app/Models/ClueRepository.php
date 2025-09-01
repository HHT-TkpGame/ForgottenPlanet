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
    /**
     * roomIdに対応する手がかりの共有情報を取得
     * Unity側からポーリングで定期実行
     */
    public function getSharedByRoomId($roomId){
        //clue_idとis_sheredだけ返し、Unity側で値が変わった時だけ反映するようにしたい
        return Clue::where('room_id',$roomId)
                ->select('clue_id','is_shared')
                ->get();
    }
    /**
     * 指定されたclueIdの共有情報を共有済みに変更
     * roomごとにclue_idが被ることはないのでclueIdで絞り込む
     */
    public function setSharedByClueId($roomId, $clueId)
    {
        Clue::where('room_id',$roomId)
        ->where('clue_id', $clueId)
        ->update(['is_shared' => true]);
    }
}
