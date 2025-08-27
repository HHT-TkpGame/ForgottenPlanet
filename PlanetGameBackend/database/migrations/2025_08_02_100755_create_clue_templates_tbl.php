<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::create('clue_templates_tbl', function (Blueprint $table) {
            $table->unsignedBigInteger('clue_id');
            $table->unsignedBigInteger('truth_id');
            $table->string('title')->nullable();
            $table->text('description')->nullable();
            $table->timestamps();

            //複合主キー
            $table->primary(['truth_id','clue_id']);

            // truth_templates_tblのキー
            $table->foreign('truth_id')
                  ->references('truth_id')
                  ->on('truth_templates_tbl')
                  ->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('clue_templates_tbl');
    }
};
