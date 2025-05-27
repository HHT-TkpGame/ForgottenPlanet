<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\Room;

class RoomController extends Controller
{
    //roomId�ɑΉ�����J�������폜����
    public function deleteRoom($roomId){
	$room = Room::find($roomId);
	if(!$room){
	    return response()->json(['error' => 'Room not found'], 404);	
	}
	//�J�X�P�[�h�폜��ݒ肵�Ă���̂ŁA
	//���[���ɕR�Â��Ă���players��positions�̃J�������ꏏ�ɍ폜�����	
	$room->delete();

	return response()->json(['status' => 'deleted']);
    }
}
