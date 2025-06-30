<?php
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ApiTestController;
use App\Http\Controllers\RoomStatusController;
use App\Http\Controllers\RoomController;
use App\Http\Controllers\PositionController;
use App\Http\Controllers\PlayerController;
use App\Http\Controllers\ChatMessageController;
use App\Http\Controllers\MatchingController;
use App\Http\Controllers\RoleSelectionController;


Route::get('/hello', [ApiTestController::class, 'hello']);

//マッチング処理
Route::post('/match', [MatchingController::class, 'match']);

//部屋の接続人数を取得
Route::get('/room/{roomId}/playerCount', [RoomStatusController::class, 'getPlayerCountInRoom']);

//roomIdに対応するプレイヤーの座標を更新
Route::post('/room/{roomId}/position', [PositionController::class, 'updatePosition']);

//roomIdに対応するプレイヤーの座標を取得
Route::get('/room/{roomId}/position', [PositionController::class, 'getPosition']);

//roomIdに対応する部屋を削除
Route::delete('room/{roomId}', [RoomController::class, 'deleteRoom']);

//プレイヤーの最後に通信された時間を更新
Route::post('player/heartbeat', [PlayerController::class, 'heartbeat']);

//roomIdに対応する部屋のチャットをストアする
Route::post('room/{roomId}/chat', [ChatMessageController::class, 'store']);

//roomIdに対応する部屋のチャットを取得
Route::get('room/{roomId}/chat', [ChatMessageController::class, 'fetch']);

//roomIdに対応する部屋のプレイヤーの選択情報を取得
Route::get('room/{roomId}/roles', [RoleSelectionController::class, 'getSelectionsInRoom']);

//クライアントの役職などの更新
Route::post('room/{roomId}/roles', [RoleSelectionController::class, 'updateSelection']);

//役職が被っているかチェック
Route::get('room/{roomId}/roles/conflict', [RoleSelectionController::class, 'checkConflictInRoom']);
?>
