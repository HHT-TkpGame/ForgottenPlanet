<?php
namespace App\Enums;

/**
 * ゲーム進行のenum
 */
enum GameProgress: int
{
    case Select = 1;
    case InGame = 2;
    case Answer = 3;
    case Result = 4;
}
