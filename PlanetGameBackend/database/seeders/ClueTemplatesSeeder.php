<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

class ClueTemplatesSeeder extends Seeder
{
    private const MAX_TRUTH = 6;
    private const MAX_CLUE = 15;
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        // 真相マスタ
        $clues = [];
        for ($truthId = 1; $truthId <= self::MAX_TRUTH; $truthId++) {
            for($clueId = 1; $clueId <= self::MAX_CLUE; $clueId++){
                $clues[] = [
                    'truth_id' => $truthId,
                    'clue_id' => $clueId,
                    'title' => null, // 後で追加
                    'description'=> null, // 後で追加
                    'created_at' => now(),
                    'updated_at' => now(),
                ];
            }
        }
        DB::table('clue_templates_tbl')->insert($clues);
    }
}
