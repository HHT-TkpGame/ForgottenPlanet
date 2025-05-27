<?php
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ApiTestController;
use App\Http\Controllers\RoomStatusController;
use App\Http\Controllers\RoomController;
use App\Http\Controllers\PositionController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

//API�e�X�g�p
Route::get('/hello', [ApiTestController::class, 'hello']);

//�����t�}�b�`���O
Route::post('/match', function (Request $request) {
    $keyword = $request->input('keyword');
    $playerId = $request->input('player_id');


    if (!$keyword || !$playerId) {
        return response()->json(['status' => 'error', 'message' => 'Missing data'], 400);
    }
    $keywordHash = md5($keyword);

    //�|�X�g���ꂽ�����t�ɑΉ�����������T��
    $room = DB::table('rooms')->where('keyword', $keywordHash)->first();

    //�������Ȃ������ꍇ
    if (!$room) {
        $roomCount = DB::table('rooms')->count();
                //10�����ȏ゠������쐬���Ȃ�
        if ($roomCount >= 10) {
            return response()->json(['status' => 'error', 'message' => 'server is full'], 403);
        }
        //�����쐬
        $roomId = DB::table('rooms')->insertGetId([
            'keyword' => $keywordHash,
            'created_at' => now(),
        ]);
	$isHost = true;
	//��E�������_���Ō���
	$isCommander = (bool)random_int(0, 1);
        //�v���C���[�̏��ݒ�
        DB::table('players')->insert([
            'room_id' => $roomId,
	    'is_host'=>$isHost,
            'player_id' => $playerId,
	    'is_commander' => $isCommander,
        ]);

        return response()->json([
            'status' => 'created',
            'room_id' => $roomId,
	    'is_host'=>$isHost,
            'player_id' => $playerId,
            'is_commander' => $isCommander,
            'player_count' => 1,
        ]);
    }
    //�|�X�g���ꂽ�����t�ɑΉ������������������ꍇ
    else {
        $roomId = $room->id;

	//������������������Q���ł��Ȃ�
        $count = DB::table('players')->where('room_id', $roomId)->count();
        if ($count >= 2) {
            return response()->json(['status' => 'error', 'message' => 'room is full'], 403);
        }

	$isHost = false;
	//���v���C���[�̖�E���擾����
	$players = DB::table('players')->where('room_id', $roomId)->get();
	// �����N�����Ȃ������ꍇ����`�G���[�ɂȂ�Ȃ��悤�ɑ΍�B
	//$players���󂾂����ꍇ�ŏ��̐l�Ȃ̂Ŏi�ߊ��Ƃ���
	if ($players->isEmpty()) {
    	    $isCommander = true;
	} else {
	        //���Ȃ��悤�ɔ��]
    	    $existRole = $players[0]->is_commander;
    	    $isCommander = !$existRole;
	}
        //�����ɎQ��
        DB::table('players')->insert([
            'room_id' => $roomId,
	    'is_host'=>$isHost,
            'player_id' => $playerId,
	    'is_commander' => $isCommander,
        ]);

        return response()->json([
            'status' => 'joined',
            'room_id' => $roomId,
	    'is_host'=>$isHost,
            'player_id' => $playerId,
	    'is_commander' => $isCommander,
            'player_count' => $count + 1,
        ]);
    }
});

//�w�肳�ꂽ�����̃v���C���[�̐l�����擾�i���Ԋu�����ɌĂяo���j
Route::get('/room/{roomId}/playerCount', [RoomStatusController::class, 'getPlayerCountInRoom']);

//�����̍��W���|�X�g���čX�V����i�T�����j
Route::post('/room/{roomId}/position', [PositionController::class, 'updatePosition']);

//�v���C���[�̍��W���擾����i�ʐM�����j
Route::get('/room/{roomId}/position', [PositionController::class, 'getPosition']);

//�v���C���[���疾���I�ɒʐM�ؒf���ꂽ�Ƃ��Ƀ��[�����폜����
Route::delete('room/{roomId}', [RoomController::class, 'deleteRoom']);
?>
