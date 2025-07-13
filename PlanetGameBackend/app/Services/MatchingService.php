<?php

namespace App\Services;

use Illuminate\Http\Request;
use App\Services\RoleSelectionService;
use App\Services\GameStateService;
use App\Models\PlayerRepository;
use App\Models\RoomRepository;

class MatchingService
{
    /**
     * @var RoleSelectionService
     */
    protected $roleSelectionService;

    /**
     * @var GameStateService
     */
    protected $gameStateService;

    /**
     * @var RoomRepository
     */
    protected $roomRepository;

    /**
     * @var PlayerRepository
     */
    protected $playerRepository;

    public function __construct(
        RoleSelectionService $roleSelectionService,
        GameStateService $gameStateService,
        RoomRepository $roomRepository,
        PlayerRepository $playerRepository
    )
    {
        $this->roleSelectionService = $roleSelectionService;
        $this->gameStateService = $gameStateService;
        $this->roomRepository = $roomRepository;
        $this->playerRepository = $playerRepository;
    }

    public function handleMatch(Request $request)
    {
        $keyword = $request->input('keyword');
        $playerId = $request->input('player_id');
        if (!$keyword || !$playerId) {
                return response()->json(['status' => 'error', 'message' => 'Missing data'], 400);
        }
        $keywordHash = md5($keyword);

        //ポストされた合言葉に対応した部屋を探す
        $room = $this->roomRepository->findByKeyword($keywordHash);

        //部屋がなかった場合
        if (!$room) {
            $roomCount = $this->roomRepository->getRoomCount();
            //10部屋以上あったら作成しない
            if ($roomCount >= 10) {
                return response()->json(['status' => 'error', 'message' => 'server is full'], 403);
            }
            
            $roomData = $this->roomRepository->createRoom($keywordHash);
	        $isHost = true;
            //プレイヤーの情報設定
            $this->playerRepository->create([
                'room_id' => $roomData->id,
	            'is_host'=>$isHost,
                'player_id' => $playerId,
            ]);

            //ゲーム進行の初期化
            $this->gameStateService->init($roomData->id);
            $currentState = $this->gameStateService->getProgressByRoomId($roomData->id);
            //roleSelectionの初期データ作成
            $this->roleSelectionService->updateSelection(
                $playerId,
                $roomData->id,
                $isHost,
                false
            );
            //is_commanderは初期値でis_hostの値が入る
            return response()->json([
                'status' => 'created',
                'room_id' => $roomData->id,
	            'is_host'=>$isHost,
                'player_id' => $playerId,
                'is_commander' => $isHost,
                'player_count' => 1,
                'game_progress' => $currentState,
            ]);
        }
        //ポストされた合言葉に対応した部屋があった場合
        else {
        //部屋が満員だったら参加できない
        $count = $this->playerRepository->countPlayersInRoom($room->id);
        if ($count >= 2) {
            return response()->json(['status' => 'error', 'message' => 'room is full'], 403);
        }

        $isHost = false;
        
        //部屋に参加
        $this->playerRepository->create([
            'room_id' => $room->id,
	        'is_host'=>$isHost,
            'player_id' => $playerId,
        ]);

        $currentState = $this->gameStateService->getProgressByRoomId($room->id);
        //roleSelectionの初期データ作成
        $this->roleSelectionService->updateSelection(
                $playerId,
                $room->id,
                $isHost,
                false
            );
        //is_commanderは初期値でis_hostの値が入る
        return response()->json([
            'status' => 'joined',
            'room_id' => $room->id,
	        'is_host'=>$isHost,
            'player_id' => $playerId,
	        'is_commander' => $isHost,
            'player_count' => $count + 1,
            'game_progress' => $currentState,
        ]);
        }
    }
}
