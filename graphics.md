# Graphics Assets Guide

This document specifies all graphics assets required for the MSIX package, Start menu, taskbar, and installer.

## Format Requirements

- **File format:** PNG (32-bit with alpha transparency)
- **Color space:** sRGB
- **Background:** Transparent (Windows applies plate colors automatically)

## Required Assets

### App Icon (Square44x44Logo)

Used in: Taskbar, App list, Start menu small, Title bar

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 44 x 44 px | `Square44x44Logo.scale-100.png` |
| 125% | 55 x 55 px | `Square44x44Logo.scale-125.png` |
| 150% | 66 x 66 px | `Square44x44Logo.scale-150.png` |
| 200% | 88 x 88 px | `Square44x44Logo.scale-200.png` |
| 400% | 176 x 176 px | `Square44x44Logo.scale-400.png` |

**Target sizes** (unplated, for taskbar/alt-tab):

| Size | Filename |
|------|----------|
| 16 x 16 px | `Square44x44Logo.targetsize-16.png` |
| 24 x 24 px | `Square44x44Logo.targetsize-24.png` |
| 32 x 32 px | `Square44x44Logo.targetsize-32.png` |
| 48 x 48 px | `Square44x44Logo.targetsize-48.png` |
| 256 x 256 px | `Square44x44Logo.targetsize-256.png` |

**Unplated variants** (no background plate applied by Windows):

| Size | Filename |
|------|----------|
| 16 x 16 px | `Square44x44Logo.targetsize-16_altform-unplated.png` |
| 24 x 24 px | `Square44x44Logo.targetsize-24_altform-unplated.png` |
| 32 x 32 px | `Square44x44Logo.targetsize-32_altform-unplated.png` |
| 48 x 48 px | `Square44x44Logo.targetsize-48_altform-unplated.png` |
| 256 x 256 px | `Square44x44Logo.targetsize-256_altform-unplated.png` |

### Medium Tile (Square150x150Logo)

Used in: Start menu medium tile

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 150 x 150 px | `Square150x150Logo.scale-100.png` |
| 125% | 188 x 188 px | `Square150x150Logo.scale-125.png` |
| 150% | 225 x 225 px | `Square150x150Logo.scale-150.png` |
| 200% | 300 x 300 px | `Square150x150Logo.scale-200.png` |
| 400% | 600 x 600 px | `Square150x150Logo.scale-400.png` |

### Wide Tile (Wide310x150Logo)

Used in: Start menu wide tile

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 310 x 150 px | `Wide310x150Logo.scale-100.png` |
| 125% | 388 x 188 px | `Wide310x150Logo.scale-125.png` |
| 150% | 465 x 225 px | `Wide310x150Logo.scale-150.png` |
| 200% | 620 x 300 px | `Wide310x150Logo.scale-200.png` |
| 400% | 1240 x 600 px | `Wide310x150Logo.scale-400.png` |

### Large Tile (Square310x310Logo) - Optional

Used in: Start menu large tile

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 310 x 310 px | `Square310x310Logo.scale-100.png` |
| 125% | 388 x 388 px | `Square310x310Logo.scale-125.png` |
| 150% | 465 x 465 px | `Square310x310Logo.scale-150.png` |
| 200% | 620 x 620 px | `Square310x310Logo.scale-200.png` |
| 400% | 1240 x 1240 px | `Square310x310Logo.scale-400.png` |

### Small Tile (Square71x71Logo) - Optional

Used in: Start menu small tile

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 71 x 71 px | `Square71x71Logo.scale-100.png` |
| 125% | 89 x 89 px | `Square71x71Logo.scale-125.png` |
| 150% | 107 x 107 px | `Square71x71Logo.scale-150.png` |
| 200% | 142 x 142 px | `Square71x71Logo.scale-200.png` |
| 400% | 284 x 284 px | `Square71x71Logo.scale-400.png` |

### Store Logo (StoreLogo)

Used in: Microsoft Store listing, App installer

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 50 x 50 px | `StoreLogo.scale-100.png` |
| 125% | 63 x 63 px | `StoreLogo.scale-125.png` |
| 150% | 75 x 75 px | `StoreLogo.scale-150.png` |
| 200% | 100 x 100 px | `StoreLogo.scale-200.png` |
| 400% | 200 x 200 px | `StoreLogo.scale-400.png` |

### Splash Screen (SplashScreen)

Used in: App launch splash screen

| Scale | Dimensions | Filename |
|-------|------------|----------|
| 100% | 620 x 300 px | `SplashScreen.scale-100.png` |
| 125% | 775 x 375 px | `SplashScreen.scale-125.png` |
| 150% | 930 x 450 px | `SplashScreen.scale-150.png` |
| 200% | 1240 x 600 px | `SplashScreen.scale-200.png` |
| 400% | 2480 x 1200 px | `SplashScreen.scale-400.png` |

## Minimum Required Assets

For a basic deployment, you need at minimum these base files (Windows will scale them):

| Asset | Dimensions | Filename |
|-------|------------|----------|
| App Icon | 44 x 44 px | `Square44x44Logo.png` |
| Medium Tile | 150 x 150 px | `Square150x150Logo.png` |
| Wide Tile | 310 x 150 px | `Wide310x150Logo.png` |
| Store Logo | 50 x 50 px | `StoreLogo.png` |
| Splash Screen | 620 x 300 px | `SplashScreen.png` |

## Recommended Asset Set

For best quality across all display scales:

| Priority | Asset | Key Sizes |
|----------|-------|-----------|
| High | Square44x44Logo | scale-100, scale-200, targetsize-256 |
| High | Square150x150Logo | scale-100, scale-200 |
| High | StoreLogo | scale-100, scale-200 |
| Medium | Wide310x150Logo | scale-100, scale-200 |
| Medium | SplashScreen | scale-100, scale-200 |
| Low | Square310x310Logo | scale-100, scale-200 |

## File Location

All assets should be placed in:

```
DefaultApp/Assets/
```

## Naming Convention

Windows automatically selects the appropriate asset based on filename qualifiers:

- **Scale:** `AssetName.scale-XXX.png` (100, 125, 150, 200, 400)
- **Target size:** `AssetName.targetsize-XX.png` (16, 24, 32, 48, 256)
- **Unplated:** `AssetName.targetsize-XX_altform-unplated.png`
- **Light theme:** `AssetName.targetsize-XX_altform-lightunplated.png`

## Design Guidelines

1. **Safe zone:** Keep important content within 66% of the center area for tiles
2. **Padding:** Include ~12.5% padding around the icon for target sizes
3. **Contrast:** Ensure icon is visible on both light and dark backgrounds
4. **Simplicity:** Icons should be recognizable at 16x16 pixels

## Tools

- **Visual Studio:** Image Asset Generator (right-click project > Add > New Item > Image Assets)
- **Windows App Certification Kit:** Validates assets meet requirements
- **Figma/Illustrator:** Export at 4x scale, then resize for each target
