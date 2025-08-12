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
        Schema::create('truth_templates_tbl', function (Blueprint $table) {
            $table->id('truth_id');
            $table->string('name')->nullable(); // 真相名
            $table->text('description')->nullable(); // 説明
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('truth_templates_tbl');
    }
};
