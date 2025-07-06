<?php

namespace App\Models;

use App\Models\Room;

/**
 * ルームのリポジトリクラス
 */
class RoomRepository
{

    /**
     * プレイヤー情報つきの全ルームを取得
     */
    public function getAllRoomsWithPlayers()
    {
        return Room::with('players')->get();
    }
    /**
     * 指定されたルームを削除
     */
    public function delete(Room $room)
    {
        $room->delete();
    }

    /**
     * 合言葉をもとに部屋を検索
     * @param string 合言葉
     * @return //部屋情報のレコード（見つからなければnull）
     */
    public function findByKeyword(string $keywordHash)
    {
        return Room::where('keyword', $keywordHash)->first();
    }

    /**
     * ハッシュ化した合言葉で部屋を作成
     * @param string ハッシュ化された合言葉
     * @return Room 作成した部屋
     */
    public function createRoom(string $keywordHash)
    {
        return Room::create(['keyword' => $keywordHash]);
    }

    /**
     * 部屋の数を取得
     * @return int 部屋数
     */
    public function getRoomCount()
    {
        return Room::count();
    }    
    
}
