<?php
use Illuminate\Support\Facades\Route;

Route::get('/api/testweb', function () {
    return response()->json(['message' => 'from web']);
});
Route::get('/debug/path', function (\Illuminate\Http\Request $request) {
    dd([
        'full_url' => $request->fullUrl(),
        'path' => $request->path(),
        'uri' => $request->getRequestUri(),
    ]);
});
?>
