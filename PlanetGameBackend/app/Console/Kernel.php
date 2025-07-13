<?php

namespace App\Console;

use Illuminate\Console\Scheduling\Schedule;
use Illuminate\Foundation\Console\Kernel as ConsoleKernel;

class Kernel extends ConsoleKernel
{
    protected $commands = [
        //Commands\RemoveInactiveRooms::class,
    ];

    protected function schedule(Schedule $schedule): void
    {
        // 1分ごとに実行する
        //$schedule->command('examples:greet')->everyMinute();
    }

    protected function commands(): void
    {
        $this->load(__DIR__.'/Commands');
        require base_path('routes/console.php');
    }
}
