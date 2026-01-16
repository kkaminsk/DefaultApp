---
name: Build MSIX
description: Build and sign the MSIX package using Azure Trusted Signing
category: Build
tags: [build, msix, signing, package]
---

# Build and Sign MSIX Package

Build the DefaultApp MSIX package and sign it with Azure Trusted Signing.

## Steps

1. **Build the MSIX package**
   ```powershell
   cd "C:/Temp/tsscat/DefaultApp"
   dotnet publish DefaultApp/DefaultApp.csproj -c Release -r win-x64 -p:Platform=x64 -p:GenerateAppxPackageOnBuild=true
   ```

2. **Sign the MSIX with Azure Trusted Signing**
   ```powershell
   powershell.exe -Command "& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe' sign /fd SHA256 /tr http://timestamp.acs.microsoft.com /td SHA256 /dlib 'C:\Temp\tsscat\CodeSigning\Microsoft.Trusted.Signing.Client.1.0.95\bin\x64\Azure.CodeSigning.Dlib.dll' /dmdf 'C:\Temp\tsscat\CodeSigning\metadata-privategpt.json' 'C:\Temp\tsscat\DefaultApp\DefaultApp\bin\x64\Release\net8.0-windows10.0.22000.0\win-x64\AppPackages\DefaultApp_1.0.0.0_x64_Test\DefaultApp_1.0.0.0_x64.msix'"
   ```

3. **Verify the signature**
   ```powershell
   powershell.exe -Command "& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe' verify /pa /v 'C:\Temp\tsscat\DefaultApp\DefaultApp\bin\x64\Release\net8.0-windows10.0.22000.0\win-x64\AppPackages\DefaultApp_1.0.0.0_x64_Test\DefaultApp_1.0.0.0_x64.msix'"
   ```

4. **Report the results** including:
   - Package location and size
   - Signing certificate (should be "Big Hat Group Inc.")
   - Timestamp status

## Configuration

| Setting | Value |
|---------|-------|
| SDK Version | 10.0.26100.0 (required - older versions don't recognize MSIX format) |
| Signing Method | Azure Trusted Signing |
| Metadata File | `C:\Temp\tsscat\CodeSigning\metadata-privategpt.json` |
| DLib Path | `C:\Temp\tsscat\CodeSigning\Microsoft.Trusted.Signing.Client.1.0.95\bin\x64\Azure.CodeSigning.Dlib.dll` |
| Certificate | Big Hat Group Inc. |
| Timestamp Server | http://timestamp.acs.microsoft.com |

## Output Location

The signed MSIX will be at:
```
DefaultApp/bin/x64/Release/net8.0-windows10.0.22000.0/win-x64/AppPackages/DefaultApp_1.0.0.0_x64_Test/DefaultApp_1.0.0.0_x64.msix
```

## Troubleshooting

- **"File format cannot be signed"**: Use SDK 10.0.26100.0 signtool, not older versions
- **"Publisher mismatch"**: Ensure Package.appxmanifest Publisher matches certificate subject
- **Azure auth errors**: Check Azure CLI login with `az account show`
