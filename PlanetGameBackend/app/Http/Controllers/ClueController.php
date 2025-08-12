<?php

namespace App\Http\Controllers;

use App\Service\ClueService;
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
}