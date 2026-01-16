## 1. GitHub Actions Workflow

- [x] 1.1 Create .github/workflows/ci.yml
- [x] 1.2 Configure workflow triggers (push, pull_request)
- [x] 1.3 Set up Windows runner (windows-latest)
- [x] 1.4 Install .NET 8 SDK
- [x] 1.5 Install Windows App SDK workload

## 2. Build Steps

- [x] 2.1 Add dotnet restore step
- [x] 2.2 Add dotnet build step for Release configuration
- [x] 2.3 Add dotnet test step with coverage
- [x] 2.4 Add dotnet publish for win-x64
- [x] 2.5 Add dotnet publish for win-arm64

## 3. MSIX Packaging

- [x] 3.1 Configure MakeAppx.exe invocation
- [x] 3.2 Generate MSIX bundle with x64 and ARM64
- [x] 3.3 Set version from build number or tag
- [x] 3.4 Upload MSIX artifacts

## 4. Code Signing

- [x] 4.1 Configure Azure Artifact signing integration
- [x] 4.2 Set up secure certificate access
- [x] 4.3 Sign MSIX packages in pipeline
- [x] 4.4 Verify signature after signing

## 5. Store Submission

- [x] 5.1 Configure Store submission credentials
- [x] 5.2 Set up Partner Center API access
- [x] 5.3 Automate submission on release tags
- [x] 5.4 Include runFullTrust capability justification

## 6. Debug Build Configuration

- [x] 6.1 Add DEBUG conditional compilation
- [x] 6.2 Enable verbose console logging in debug
- [x] 6.3 Add debug menu/page for testing
- [x] 6.4 Add mock data options for service testing
- [x] 6.5 Exclude debug features from Release builds

## 7. Build Architectures

- [x] 7.1 Configure x64 build target
- [x] 7.2 Configure ARM64 build target
- [x] 7.3 Configure AnyCPU for class libraries
- [x] 7.4 Exclude x86 (32-bit) builds

## 8. Version Management

- [x] 8.1 Configure SemVer versioning (1.0.0)
- [x] 8.2 Auto-increment version on builds
- [x] 8.3 Sync MSIX package version with assembly version

## 9. Artifact Management

- [x] 9.1 Upload build artifacts
- [x] 9.2 Upload test results
- [x] 9.3 Upload coverage reports
- [x] 9.4 Upload signed MSIX packages

## 10. Validation

- [x] 10.1 CI pipeline runs successfully on push
- [x] 10.2 All tests pass in CI
- [x] 10.3 MSIX packages are created
- [x] 10.4 Packages are signed correctly
- [x] 10.5 ARM64 and x64 packages both work
