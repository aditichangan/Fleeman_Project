# Car Images Download Script
# Run this in PowerShell to help download remaining car images

$imagesFolder = "c:\Users\GIRI\Desktop\CDAC Project\Fleeman.NET\React_Frontend\public\images"

# List of remaining cars to download (55 remaining)
$remainingCars = @(
    "toyota_fortuner", "tata_nexon", "mercedes_c_class",
    "creta", "seltos", "harrier", "xuv700", "hector",
    "swift", "i20", "altroz", "kwid", "wagonr", "grand_i10",
    "tiago", "celerio", "ignis", "verna", "amaze", "slavia",
    "virtus", "yaris", "civic", "rapid", "sunny", "hyryder",
    "elevate", "bmw3", "a6", "bmw5", "q5", "glc", "xc60",
    "xf", "lexus_es", "innova", "ertiga", "carens", "marazzo",
    "rumion", "nexon_ev", "zs_ev", "kona_ev", "tiago_ev",
    "xuv400_ev", "venue", "sonet", "punch", "magnite", "kiger",
    "dmax", "hilux", "bolero_pickup", "yodha", "vcross"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Car Images Download Helper" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Images folder: $imagesFolder`n" -ForegroundColor Yellow

Write-Host "OPTION 1: Use AI Image Generator (Recommended)" -ForegroundColor Green
Write-Host "Visit: https://www.bing.com/images/create" -ForegroundColor White
Write-Host "Or: https://leonardo.ai/`n" -ForegroundColor White

Write-Host "OPTION 2: Download from Stock Photos" -ForegroundColor Green
Write-Host "Visit: https://unsplash.com/s/photos/cars" -ForegroundColor White
Write-Host "Or: https://www.pexels.com/search/cars/`n" -ForegroundColor White

Write-Host "Remaining cars to download ($($remainingCars.Count)):" -ForegroundColor Yellow
foreach ($car in $remainingCars) {
    Write-Host "  - $car.jpg" -ForegroundColor White
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Quick Tips:" -ForegroundColor Cyan
Write-Host "1. Save all images as .jpg format" -ForegroundColor White
Write-Host "2. Use exact filenames from the list above" -ForegroundColor White
Write-Host "3. Recommended size: 800x600px or larger" -ForegroundColor White
Write-Host "4. Side 3/4 angle view preferred" -ForegroundColor White
Write-Host "========================================`n" -ForegroundColor Cyan

# Check which images are already present
Write-Host "Checking existing images..." -ForegroundColor Yellow
$existingCount = 0
Get-ChildItem -Path $imagesFolder -Filter "*.jpg" -ErrorAction SilentlyContinue | ForEach-Object {
    $existingCount++
    Write-Host "  ✓ $($_.Name)" -ForegroundColor Green
}

Write-Host "`nProgress: $existingCount / 60 images" -ForegroundColor Cyan
Write-Host "Remaining: $($60 - $existingCount) images`n" -ForegroundColor Yellow
