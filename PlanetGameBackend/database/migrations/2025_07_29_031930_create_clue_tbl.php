<?php
use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('clue_tbl', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('room_id')->unique(); // 1部屋1レコード
            $table->foreign('room_id')->references('id')->on('rooms')->onDelete('cascade');
            
        });
    }

    public function down(): void
    {
        Schema::dropIfExists('game_states_tbl');
    }
};
