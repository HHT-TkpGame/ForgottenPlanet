<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

class TruthTemplatesSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        DB::table('truth_templates_tbl')->insert([
            ['truth_id' => 1, 'name' => 'AIの暴走', 'description' => null],
            ['truth_id' => 2, 'name' => '異星人の侵略', 'description' => null],
            ['truth_id' => 3, 'name' => '異常気象', 'description' => null],
            ['truth_id' => 4, 'name' => 'パンデミック', 'description' => null],
            ['truth_id' => 5, 'name' => '政治的内乱', 'description' => null],
            ['truth_id' => 6, 'name' => '次元の歪み', 'description' => null],
        ]);
    }
}
