<?php
namespace App\Enums;

enum GameProgress: int
{
    case Matching = 0;
    case Select = 1;
    case InGame = 2;
    case Result = 3;
}
