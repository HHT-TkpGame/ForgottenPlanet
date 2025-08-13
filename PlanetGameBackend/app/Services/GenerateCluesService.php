<?php

namespace App\Services;

use App\Models\TruthTemplateRepository;
use App\Models\ClueTemplateRepository;
use App\Models\ClueRepository;

class GenerateCluesService
{
    private const CLUE_PATTERN_COUNT = 3;
    private $truthTemplateRepository;
    private $clueTemplateRepository;
    private $clueReposirory;
    public function __construct(
        TruthTemplateRepository $truthTemplateRepository,
        ClueTemplateRepository $clueTemplateRepository,
        ClueRepository $clueReposirory
    ) {
        $this->truthTemplateRepository = $truthTemplateRepository;
        $this->clueTemplateRepository = $clueTemplateRepository;  
        $this->clueReposirory = $clueReposirory;
    }
    public function generateByRoomId($roomId)
    {
        $truthId = $this->getRandomTruthId();
        [$start, $end] = $this->getRandomClueRange($truthId);
        $clueTemplates = $this->clueTemplateRepository->getByTruthIdAndRange($truthId, $start, $end);
        foreach($clueTemplates as $clueTemplate)
        {
            $data = [
                'room_id' => $roomId,
                'truth_id' => $clueTemplate->truth_id,
                'clue_id' => $clueTemplate->clue_id,
                'is_shared' => false,
            ];
            $this->clueReposirory->create($data);
        }
    }
    /**
     * 今回のマッチで使う真相IDをランダムで生成
     */
    private function getRandomTruthId(): int
    {
        $truthCount = $this->truthTemplateRepository->getCount();
        return random_int(1, $truthCount);
    }
    /**
     * truthIdをもとに、生成する手がかりの範囲を生成
     * ランダムで3パターンのうちどれかになる
     */
    private function getRandomClueRange(int $truthId): array
    {
        $cluesCount = $this->clueTemplateRepository->getCountByTruthId($truthId);
        //ここでcluesCount(現在は15が返る)をもとに、3パターンに手がかりの生成を切り替えたい
        //パターンはそれぞれ1~5,6~10,11~15

        ////////////////
        //truthIdが5のときは1~5固定にする
        ////////////////
        $chunkSize = $cluesCount / self::CLUE_PATTERN_COUNT;
        $ranges = [];
        for ($i = 0; $i < self::CLUE_PATTERN_COUNT; $i++) {
            $start = $i * $chunkSize + 1;
            $end   = ($i + 1) * $chunkSize;
            $ranges[] = [$start, $end];
        }
        return $ranges[array_rand($ranges)];
    }
}
