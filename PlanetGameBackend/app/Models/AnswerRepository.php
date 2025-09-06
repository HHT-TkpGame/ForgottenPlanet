<?php

namespace App\Models;

use App\Models\Answer;

class AnswerRepository
{
    /**
     * 指定された roomId と answerId で新しい回答を登録する
     *
     * @param int $roomId
     * @param int $answerId
     * @return Answer
     */
    public function postAnswer(int $roomId, int $answerId)
    {
        return Answer::updateOrCreate(
            ['room_id' => $roomId],
            ['answer_id' => $answerId]
        );
    }

    /**
     * 指定された roomId に対応する全ての answerId を取得する
     *
     * @param int $roomId
     * @return \Illuminate\Support\Collection
     */
    public function getAnswer(int $roomId): int
    {
        return Answer::where('room_id', $roomId)->value('answer_id') ?? 0;
    }
}
