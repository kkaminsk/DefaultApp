Add-Type -AssemblyName System.Drawing

$sourcePath = "C:\Temp\tsscat\DefaultApp\DefaultApp\sourcegraphics\appicon.png"
$assetsPath = "C:\Temp\tsscat\DefaultApp\DefaultApp\Assets"

# Load source image
$source = [System.Drawing.Image]::FromFile($sourcePath)

function Resize-Image {
    param(
        [System.Drawing.Image]$Source,
        [int]$Width,
        [int]$Height,
        [string]$OutputPath,
        [bool]$CenterOnCanvas = $false
    )

    $bitmap = New-Object System.Drawing.Bitmap($Width, $Height)
    $bitmap.SetResolution(96, 96)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality

    # Clear with transparent background
    $graphics.Clear([System.Drawing.Color]::Transparent)

    if ($CenterOnCanvas) {
        # For wide/splash: center the icon, scale to fit height with padding
        $iconSize = [Math]::Min($Height * 0.8, $Width * 0.4)
        $x = ($Width - $iconSize) / 2
        $y = ($Height - $iconSize) / 2
        $graphics.DrawImage($Source, $x, $y, $iconSize, $iconSize)
    } else {
        # Square: fill entire canvas
        $graphics.DrawImage($Source, 0, 0, $Width, $Height)
    }

    $graphics.Dispose()
    $bitmap.Save($OutputPath, [System.Drawing.Imaging.ImageFormat]::Png)
    $bitmap.Dispose()
    Write-Host "Created: $OutputPath"
}

# Square44x44Logo - App Icon
$sizes = @(
    @{Scale=100; Size=44},
    @{Scale=125; Size=55},
    @{Scale=150; Size=66},
    @{Scale=200; Size=88},
    @{Scale=400; Size=176}
)
foreach ($s in $sizes) {
    Resize-Image -Source $source -Width $s.Size -Height $s.Size -OutputPath "$assetsPath\Square44x44Logo.scale-$($s.Scale).png"
}

# Target sizes for Square44x44Logo
$targetSizes = @(16, 24, 32, 48, 256)
foreach ($size in $targetSizes) {
    Resize-Image -Source $source -Width $size -Height $size -OutputPath "$assetsPath\Square44x44Logo.targetsize-$size.png"
    # Also create unplated versions (same image, different name)
    Resize-Image -Source $source -Width $size -Height $size -OutputPath "$assetsPath\Square44x44Logo.targetsize-$size`_altform-unplated.png"
}

# Square150x150Logo - Medium Tile
$sizes = @(
    @{Scale=100; Size=150},
    @{Scale=125; Size=188},
    @{Scale=150; Size=225},
    @{Scale=200; Size=300},
    @{Scale=400; Size=600}
)
foreach ($s in $sizes) {
    Resize-Image -Source $source -Width $s.Size -Height $s.Size -OutputPath "$assetsPath\Square150x150Logo.scale-$($s.Scale).png"
}

# StoreLogo
$sizes = @(
    @{Scale=100; Size=50},
    @{Scale=125; Size=63},
    @{Scale=150; Size=75},
    @{Scale=200; Size=100},
    @{Scale=400; Size=200}
)
foreach ($s in $sizes) {
    Resize-Image -Source $source -Width $s.Size -Height $s.Size -OutputPath "$assetsPath\StoreLogo.scale-$($s.Scale).png"
}

# Wide310x150Logo - Wide Tile (icon centered)
$sizes = @(
    @{Scale=100; W=310; H=150},
    @{Scale=125; W=388; H=188},
    @{Scale=150; W=465; H=225},
    @{Scale=200; W=620; H=300},
    @{Scale=400; W=1240; H=600}
)
foreach ($s in $sizes) {
    Resize-Image -Source $source -Width $s.W -Height $s.H -OutputPath "$assetsPath\Wide310x150Logo.scale-$($s.Scale).png" -CenterOnCanvas $true
}

# SplashScreen (icon centered)
$sizes = @(
    @{Scale=100; W=620; H=300},
    @{Scale=125; W=775; H=375},
    @{Scale=150; W=930; H=450},
    @{Scale=200; W=1240; H=600},
    @{Scale=400; W=2480; H=1200}
)
foreach ($s in $sizes) {
    Resize-Image -Source $source -Width $s.W -Height $s.H -OutputPath "$assetsPath\SplashScreen.scale-$($s.Scale).png" -CenterOnCanvas $true
}

# Also create base files (without scale suffix) using scale-100 dimensions
Resize-Image -Source $source -Width 44 -Height 44 -OutputPath "$assetsPath\Square44x44Logo.png"
Resize-Image -Source $source -Width 150 -Height 150 -OutputPath "$assetsPath\Square150x150Logo.png"
Resize-Image -Source $source -Width 50 -Height 50 -OutputPath "$assetsPath\StoreLogo.png"
Resize-Image -Source $source -Width 310 -Height 150 -OutputPath "$assetsPath\Wide310x150Logo.png" -CenterOnCanvas $true
Resize-Image -Source $source -Width 620 -Height 300 -OutputPath "$assetsPath\SplashScreen.png" -CenterOnCanvas $true

$source.Dispose()
Write-Host "`nAll assets generated successfully!"
