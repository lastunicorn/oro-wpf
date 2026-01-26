# PowerShell script to create a minimalistic clock icon
# This script creates a simple clock icon with clock hands

Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms

# Define icon sizes
$sizes = @(16, 32, 48, 64, 128, 256)

# Create a temporary directory for icon images
$tempDir = Join-Path $env:TEMP "ClockIcon"
if (-not (Test-Path $tempDir)) {
    New-Item -ItemType Directory -Path $tempDir | Out-Null
}

$iconFiles = @()

foreach ($size in $sizes) {
    $bitmap = New-Object System.Drawing.Bitmap($size, $size)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $graphics.Clear([System.Drawing.Color]::Transparent)

    # Calculate dimensions
    $centerX = $size / 2
    $centerY = $size / 2
    $radius = ($size / 2) - ($size * 0.1)
    $penWidth = [Math]::Max(1, $size / 32)

    # Draw clock circle (minimalistic outline)
    $pen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 33, 33, 33), $penWidth)
    $graphics.DrawEllipse($pen, $centerX - $radius, $centerY - $radius, $radius * 2, $radius * 2)

    # Draw hour hand (pointing to 10 o'clock)
    $hourHandLength = $radius * 0.5
    $hourAngle = [Math]::PI * (10.0 / 6.0 - 0.5)
    $hourEndX = $centerX + $hourHandLength * [Math]::Cos($hourAngle)
    $hourEndY = $centerY + $hourHandLength * [Math]::Sin($hourAngle)
    $hourPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 33, 33, 33), ($penWidth * 1.5))
    $graphics.DrawLine($hourPen, $centerX, $centerY, $hourEndX, $hourEndY)

    # Draw minute hand (pointing to 2 o'clock)
    $minuteHandLength = $radius * 0.7
    $minuteAngle = [Math]::PI * (2.0 / 6.0 - 0.5)
    $minuteEndX = $centerX + $minuteHandLength * [Math]::Cos($minuteAngle)
    $minuteEndY = $centerY + $minuteHandLength * [Math]::Sin($minuteAngle)
    $minutePen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(255, 33, 33, 33), $penWidth)
    $graphics.DrawLine($minutePen, $centerX, $centerY, $minuteEndX, $minuteEndY)

    # Draw center dot
    $dotRadius = $penWidth * 1.5
    $brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(255, 33, 33, 33))
    $graphics.FillEllipse($brush, $centerX - $dotRadius, $centerY - $dotRadius, $dotRadius * 2, $dotRadius * 2)

    # Save the bitmap as PNG
    $pngPath = Join-Path $tempDir "icon_$size.png"
    $bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
    $iconFiles += $pngPath

    # Cleanup
    $graphics.Dispose()
    $bitmap.Dispose()
    $pen.Dispose()
    $hourPen.Dispose()
    $minutePen.Dispose()
    $brush.Dispose()
}

# Convert to ICO using ImageMagick (if available) or manual conversion
$outputPath = Join-Path $PSScriptRoot "app.ico"

# Try using ImageMagick if available
$magickPath = Get-Command "magick" -ErrorAction SilentlyContinue
if ($magickPath) {
    Write-Host "Creating ICO file using ImageMagick..."
    & magick $iconFiles $outputPath
    Write-Host "Icon created successfully at: $outputPath"
} else {
    # Fallback: Create a basic ICO file manually
    Write-Host "ImageMagick not found. Creating basic ICO file..."
    
    # For simplicity, we'll use the largest PNG and convert it
    $largestPng = $iconFiles[-1]
    $bitmap = [System.Drawing.Bitmap]::FromFile($largestPng)
    $icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())
    $stream = [System.IO.File]::Create($outputPath)
    $icon.Save($stream)
    $stream.Close()
    $bitmap.Dispose()
    
    Write-Host "Basic icon created at: $outputPath"
    Write-Host "Note: For a multi-resolution ICO file, install ImageMagick and run this script again."
}

# Cleanup temporary files
Remove-Item $tempDir -Recurse -Force

Write-Host "Done!"
