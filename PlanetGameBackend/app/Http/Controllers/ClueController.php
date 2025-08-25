<?php

namespace App\Http\Controllers;

use App\Services\ClueService;
use Illuminate\Http\Request;

class ClueController extends Controller
{
    private $clueService;
    public function __construct(
        ClueService $clueService
    )
    {
        $this->clueService = $clueService;
    }
    public function getCluesByRoomId($roomId){
        $clueData = $this->clueService->getByRoomId($roomId);
        return response()->json($clueData);
    }

    public function getClueShared($roomID){
        $data = $this->clueService->getSharedByRoomId($roomID)
            ->map(function ($clue) {
            return [
                'clue_id'   => $clue->clue_id,
                'is_shared' => (bool) $clue->is_shared
            ];
        })
        ->values();
        return response()->json(['sharedClues' => $data]);
    }
    public function postClueShared($roomID, $clueId){
        $this->clueService->setSharedByClueId($roomID, $clueId);
    }
}