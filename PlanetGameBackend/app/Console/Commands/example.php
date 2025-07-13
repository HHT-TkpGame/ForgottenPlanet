<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;

class example extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'examples:greet';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'TestCommand';

    /**
     * Execute the console command.
     */
    public function handle()
    {
        echo 'hello';
    }
}
