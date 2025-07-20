<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Services\MatchingService;

/**
* マッチング処理のコントローラ
*/
class MatchingController extends Controller
{
    //MatchingServiceにインスタンス格納
    protected $matchingService;

    /**
     * MatchingService を受け取る
     *
     * @param MatchingService $matchingService
     */
    public function __construct(MatchingService $matchingService)
    {
        $this->matchingService = $matchingService;
    }

    /**
    *マッチング処理
    *@return json
    */
    public function match(Request $request)
    {
	//Serviceに処理を委譲する
        return $this->matchingService->handleMatch($request);
    }
}
