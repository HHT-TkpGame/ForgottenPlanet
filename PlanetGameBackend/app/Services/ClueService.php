<?php

namespace App\Services;

use App\Models\ClueRepository;

class ClueService
{
    /**
     * @var ClueRepository
     */
    private $clueRepository;

    public function __construct(
        ClueRepository $clueRepository
    ){
        $this->clueRepository = $clueRepository;
    }
    /**
     * 指定された部屋の、今回のマッチで選ばれた真相のIDと手がかりの範囲を取得
     * @return array 真相Idと手がかりIDの範囲
     */
    public function getByRoomId($roomId)
    {
        $clues = $this->clueRepository->findByRoomId($roomId);
        if($clues->isEmpty()){
            return [];
        }
        //$cluesの中のtruth_idと、clue_idから範囲を取得
        //範囲は取得したデータの中のclue_idの最大値と最小値
        $truthId = $clues->first()->truth_id;
        $minClueId = $clues->min('clue_id');
        $maxClueId = $clues->max('clue_id');
        return [
            'truth_id' => $truthId,
            'clues_range' => [$minClueId, $maxClueId],
        ];
    }
    public function getSharedByRoomId($roomId){
        return $this->clueRepository->getSharedByRoomId($roomId);
    }
    public function setSharedByClueId($roomId, $clueId)
    {
        return $this->clueRepository->setSharedByClueId($roomId, $clueId);
    }
}