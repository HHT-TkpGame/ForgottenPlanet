<?php
use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('clues_tbl', function (Blueprint $table) {
            $table->id();
            $table->unsignedBigInteger('room_id'); 
            $table->unsignedBigInteger('truth_id'); 
            $table->unsignedBigInteger('clue_id');  
            $table->boolean('is_shared')->default(false); 
            $table->timestamps();

            // 外部キー制約
            $table->foreign('truth_id')
                  ->references('truth_id')->on('truth_templates_tbl')
                  ->onDelete('cascade');

            $table->foreign(['truth_id','clue_id'])
                  ->references(['truth_id','clue_id'])->on('clue_templates_tbl')
                  ->onDelete('cascade');

            $table->foreign('room_id')
                  ->references('id')->on('rooms')
                  ->onDelete('cascade');
        });
    }

    public function down(): void
    {
        Schema::dropIfExists('clues_tbl');
    }
};
