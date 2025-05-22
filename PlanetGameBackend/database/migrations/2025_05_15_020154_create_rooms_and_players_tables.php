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
        Schema::create('rooms', function (Blueprint $table) {
            $table->id();
	    $table->string('keyword')->nullable();
            $table->timestamps();
        });
	Schema::create('players', function (Blueprint $table) {
            $table->id();
            $table->foreignId('room_id')->constrained('rooms')->onDelete('cascade');
            $table->string('player_id');
	    $table->boolean('is_host')->default(false);
	    $table->boolean('is_commander')->default(false);
            $table->timestamp('last_request_time')->nullable();
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
	Schema::dropIfExists('players');
        Schema::dropIfExists('rooms');
    }
};
