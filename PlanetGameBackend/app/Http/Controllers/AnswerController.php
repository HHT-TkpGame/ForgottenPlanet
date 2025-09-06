<?php

namespace App\Http\Controllers;

use App\Models\AnswerRepository;
use Illuminate\Http\Request;

class AnswerController extends Controller
{
    private $answerRepository;
    public function __construct(
        AnswerRepository $answerRepository
    )
    {
        $this->answerRepository = $answerRepository;
    }
    public function getAnswer($roomId){
        $res = $this->answerRepository->getAnswer($roomId);
        return response()->json(['answer_id' => $res->answer_id]);
    }
    public function postAnswer(Request $request, $roomId){
        $data = [];
        $data = $request->validate([
            'answer_id' => ['required','integer','between:1,6'],
        ]);
        $this->answerRepository->postAnswer($roomId, $data['answer_id']);
    }
}