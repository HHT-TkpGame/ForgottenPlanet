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
        Schema::create('positions', function (Blueprint $table) {
        $table->id();
        $table->unsignedBigInteger('room_id');
        $table->string('player_id');
        $table->float('x');
        $table->float('y');
        $table->float('z');
	$table->float('rot_y');
        $table->timestamps();

        $table->unique(['room_id', 'player_id']);

	//外部キー制約と、カスケード削除
	$table->foreign('room_id')->references('id')->on('rooms')->onDelete('cascade');
	});  
      }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('positions');
    }
};
