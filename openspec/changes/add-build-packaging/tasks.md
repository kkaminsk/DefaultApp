## 1. GitHub Actions Workflow

- [ ] 1.1 Create .github/workflows/ci.yml
- [ ] 1.2 Configure workflow triggers (push, pull_request)
- [ ] 1.3 Set up Windows runner (windows-latest)
- [ ] 1.4 Install .NET 8 SDK
- [ ] 1.5 Install Windows App SDK workload

## 2. Build Steps

- [ ] 2.1 Add dotnet restore step
- [ ] 2.2 Add dotnet build step for Release configuration
- [ ] 2.3 Add dotnet test step with coverage
- [ ] 2.4 Add dotnet publish for win-x64
- [ ] 2.5 Add dotnet publish for win-arm64

## 3. MSIX Packaging

- [ ] 3.1 Configure MakeAppx.exe invocation
- [ ] 3.2 Generate MSIX bundle with x64 and ARM64
- [ ] 3.3 Set version from build number or tag
- [ ] 3.4 Upload MSIX artifacts

## 4. Code Signing

- [ ] 4.1 Configure Azure Artifact signing integration
- [ ] 4.2 Set up secure certificate access
- [ ] 4.3 Sign MSIX packages in pipeline
- [ ] 4.4 Verify signature after signing

## 5. Store Submission

- [ ] 5.1 Configure Store submission credentials
- [ ] 5.2 Set up Partner Center API access
- [ ] 5.3 Automate submission on release tags
- [ ] 5.4 Include runFullTrust capability justification

## 6. Debug Build Configuration

- [ ] 6.1 Add DEBUG conditional compilation
- [ ] 6.2 Enable verbose console logging in debug
- [ ] 6.3 Add debug menu/page for testing
- [ ] 6.4 Add mock data options for service testing
- [ ] 6.5 Exclude debug features from Release builds

## 7. Build Architectures

- [ ] 7.1 Configure x64 build target
- [ ] 7.2 Configure ARM64 build target
- [ ] 7.3 Configure AnyCPU for class libraries
- [ ] 7.4 Exclude x86 (32-bit) builds

## 8. Version Management

- [ ] 8.1 Configure SemVer versioning (1.0.0)
- [ ] 8.2 Auto-increment version on builds
- [ ] 8.3 Sync MSIX package version with assembly version

## 9. Artifact Management

- [ ] 9.1 Upload build artifacts
- [ ] 9.2 Upload test results
- [ ] 9.3 Upload coverage reports
- [ ] 9.4 Upload signed MSIX packages

## 10. Validation

- [ ] 10.1 CI pipeline runs successfully on push
- [ ] 10.2 All tests pass in CI
- [ ] 10.3 MSIX packages are created
- [ ] 10.4 Packages are signed correctly
- [ ] 10.5 ARM64 and x64 packages both work
