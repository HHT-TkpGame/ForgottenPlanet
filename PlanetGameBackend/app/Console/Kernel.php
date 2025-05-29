<?php

namespace App\Console;

use Illuminate\Console\Scheduling\Schedule;
use Illuminate\Foundation\Console\Kernel as ConsoleKernel;

class Kernel extends ConsoleKernel
{
    protected $commands = [
        \App\Console\Commands\RemoveInactiveRooms::class,
    ];

    protected function schedule(Schedule $schedule): void
    {
        // 1•ª‚²‚Æ‚ÉŽÀs‚·‚é
        $schedule->command('rooms:cleanup')->everyMinute();
    }

    protected function commands(): void
    {
        $this->load(__DIR__.'/Commands');
        require base_path('routes/console.php');
    }
}
