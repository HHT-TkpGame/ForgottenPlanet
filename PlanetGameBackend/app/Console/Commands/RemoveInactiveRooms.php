<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;
use Illuminate\Support\Facades\Log;
use App\Models\Room;
use Carbon\Carbon;

class RemoveInactiveRooms extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    //php artisanで表示されるコマンドの名前
    protected $signature = 'rooms:cleanup';

    /**
     * The console command description.
     *
     * @var string
     */
    //php artisanで表示されるコマンドの説明
    protected $description = 'Remove rooms where at least one player has timed out';

    /**
     * Execute the console command.
     */
    public function handle()
    {
        $timeoutSeconds = 30;
        $threshold = Carbon::now()->subSeconds($timeoutSeconds);

        // すべてのルームとそのプレイヤーを取得
        $rooms = Room::with('players')->get();

        foreach ($rooms as $room) {
            // 1人でもタイムアウトしていれば削除
            $anyTimedOut = $room->players->contains(function ($player) use ($threshold) {
                return $player->last_request_time < $threshold;
            });

            if ($anyTimedOut) {
		Log::info("Room ID {$room->id} has a timed-out player. Deleting.");
                $room->delete();
            }
        }
	Log::info("Room timeout cleanup completed.");
    }
}
