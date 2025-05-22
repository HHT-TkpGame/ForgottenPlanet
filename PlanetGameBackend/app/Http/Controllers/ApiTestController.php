<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class ApiTestController extends Controller
{
    public function hello()
    {
        return response()->json(['message' => 'Hello from apitest']);
    }
}
