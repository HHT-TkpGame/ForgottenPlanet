<?php

namespace App\Http\Controllers;

use App\Models\AnswerRepository;
use App\Models\ClueRepository;
use Illuminate\Http\Request;

class AnswerController extends Controller
{
    private $answerRepository;
    private $clueRepository;
    public function __construct(
        AnswerRepository $answerRepository,
        ClueRepository $clueRepository
    )
    {
        $this->answerRepository = $answerRepository;
        $this->clueRepository = $clueRepository;
    }
    public function getAnswer($roomId){
        $ans = $this->answerRepository->getAnswer($roomId);
        $clues = $this->clueRepository->countSharedByRoomId($roomId);
        return response()->json([
            'answer_id' => $ans->answer_id,
            'found_clues' => $clues
        ]);
    }
    public function postAnswer(Request $request, $roomId){
        $data = [];
        $data = $request->validate([
            'answer_id' => ['required','integer','between:1,6'],
        ]);
        $this->answerRepository->postAnswer($roomId, $data['answer_id']);
        return response()->json(['answerData' => $data]);
    }
}