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
         Schema::create('role_selections_tbl', function (Blueprint $table) {
            $table->id();
            $table->string('player_id');
            $table->unsignedBigInteger('room_id');
            $table->boolean('is_commander')->default(false);
            $table->boolean('is_locked')->default(false);
            $table->boolean('has_conflict')->default(false);
            $table->timestamps();

            $table->foreign('room_id')->references('id')->on('rooms')->onDelete('cascade');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('role_selections_tbl');
    }
};
